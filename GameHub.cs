using Microsoft.AspNetCore.SignalR;
using GameHub.Models;
using GameHub.Games;

namespace GameHub.Hubs;

public class GameHubSignalR : Hub
{
    private readonly RoomManager _rm;
    private readonly IHubContext<GameHubSignalR> _hubContext;
    private readonly ILogger<GameHubSignalR> _logger;

    private static readonly Dictionary<string, CancellationTokenSource> GameLoops = new();
    private static readonly object LoopLock = new();

    public GameHubSignalR(RoomManager rm, IHubContext<GameHubSignalR> hubContext, ILogger<GameHubSignalR> logger)
    { _rm=rm; _hubContext=hubContext; _logger=logger; }

    public async Task PlayerJoin(string nickname, string color)
    {
        if (string.IsNullOrWhiteSpace(nickname)) return;
        var p = _rm.CreatePlayer(Context.ConnectionId, nickname.Trim().Length>0?nickname.Trim():"Player", color.Length>0?color:"#00ff88");
        await Clients.Caller.SendAsync("player:joined", new{player=p});
        await Clients.Caller.SendAsync("stats:online", _rm.GetOnlineCountByGame());
    }

    public async Task RoomQuickPlay(string gameType)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return;
        if (!RoomManager.GameConfigs.TryGetValue(gameType, out var cfg)) return;
        var room = _rm.QuickPlay(gameType, cfg.MaxPlayers);
        var (err,_) = _rm.JoinRoom(Context.ConnectionId, room.Code);
        if (err!=null){await Clients.Caller.SendAsync("room:error",new{message=err});return;}
        await Groups.AddToGroupAsync(Context.ConnectionId, room.Code);
        await Clients.Group(room.Code).SendAsync("room:updated", _rm.GetRoomInfo(room));
        await Clients.All.SendAsync("stats:online", _rm.GetOnlineCountByGame());
    }

    public async Task RoomCreate(string gameType)
    {
        if (!RoomManager.GameConfigs.TryGetValue(gameType, out var cfg)) return;
        var room = _rm.CreateRoom(gameType, cfg.MaxPlayers, false);
        var (err,_) = _rm.JoinRoom(Context.ConnectionId, room.Code);
        if (err!=null){await Clients.Caller.SendAsync("room:error",new{message=err});return;}
        await Groups.AddToGroupAsync(Context.ConnectionId, room.Code);
        await Clients.Caller.SendAsync("room:created", new{roomCode=room.Code});
        await Clients.Group(room.Code).SendAsync("room:updated", _rm.GetRoomInfo(room));
        await Clients.All.SendAsync("stats:online", _rm.GetOnlineCountByGame());
    }

    // ── Play vs Bot ───────────────────────────────────────────
    public async Task RoomPlayVsBot(string gameType, string difficulty = "medium")
    {
        if (!RoomManager.GameConfigs.TryGetValue(gameType, out var cfg)) return;
        var room = _rm.CreateRoom(gameType, cfg.MaxPlayers, false);
        room.IsVsBot = true;
        room.BotDifficulty = difficulty;
        var (err,_) = _rm.JoinRoom(Context.ConnectionId, room.Code);
        if (err!=null){await Clients.Caller.SendAsync("room:error",new{message=err});return;}
        if(gameType=="poker") { for(int i=0;i<4;i++) room.Players.Add(BotAI.MakeBotPlayer(i)); }
        else _rm.AddBotToRoom(room);
        await Groups.AddToGroupAsync(Context.ConnectionId, room.Code);
        await Clients.Caller.SendAsync("room:created", new{roomCode=room.Code});
        await Clients.Group(room.Code).SendAsync("room:updated", _rm.GetRoomInfo(room));
        room.Players.ForEach(p=>p.IsReady=true);
        room.ReadyPlayers.Add(Context.ConnectionId);
        room.ReadyPlayers.Add(BotAI.BOT_ID);
        var hub=_hubContext;var rm=_rm;var log=_logger;
        _ = Task.Run(async()=>await StartCountdownBg(room,hub,rm,log));
        await Clients.All.SendAsync("stats:online", _rm.GetOnlineCountByGame());
    }

    public async Task RoomJoin(string roomCode)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return;
        var code = roomCode.ToUpperInvariant().Trim();
        var (err,room) = _rm.JoinRoom(Context.ConnectionId, code);
        if (err!=null||room==null){await Clients.Caller.SendAsync("room:error",new{message=err??"Loi"});return;}
        await Groups.AddToGroupAsync(Context.ConnectionId, code);
        await Clients.Group(code).SendAsync("room:updated", _rm.GetRoomInfo(room));
        await Clients.All.SendAsync("stats:online", _rm.GetOnlineCountByGame());
    }

    public async Task RoomLeave()
    {
        var player = _rm.GetPlayer(Context.ConnectionId);
        if (player?.RoomCode==null) return;
        var roomCode = player.RoomCode;
        var room = _rm.LeaveRoom(Context.ConnectionId, roomCode);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);
        StopAllLoops(roomCode);
        if (room?.Players.Count>0) await Clients.Group(roomCode).SendAsync("room:updated",_rm.GetRoomInfo(room));
        await Clients.All.SendAsync("stats:online", _rm.GetOnlineCountByGame());
    }

    public async Task RoomReady(bool isReady)
    {
        var player = _rm.GetPlayer(Context.ConnectionId);
        if (player?.RoomCode==null) return;
        var room = _rm.GetRoom(player.RoomCode);
        if (room==null||room.State!="WAITING") return;
        var rp = room.Players.FirstOrDefault(p=>p.Id==Context.ConnectionId);
        if (rp!=null) rp.IsReady=isReady;
        if (isReady) room.ReadyPlayers.Add(Context.ConnectionId);
        else room.ReadyPlayers.Remove(Context.ConnectionId);
        await Clients.Group(room.Code).SendAsync("room:updated", _rm.GetRoomInfo(room));
        var cfg = RoomManager.GameConfigs[room.GameType];
        if (room.Players.Count>=cfg.MinPlayers && room.Players.All(p=>p.IsReady))
        { var hub=_hubContext;var rm=_rm;var log=_logger; _ = Task.Run(async()=>await StartCountdownBg(room,hub,rm,log)); }
    }

    public async Task ChatMessage(string message)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return;
        var player = _rm.GetPlayer(Context.ConnectionId);
        if (player?.RoomCode==null) return;
        var room = _rm.GetRoom(player.RoomCode);
        if (room==null) return;
        var cleanMsg = RoomManager.SanitizeMessage(message);
        if (string.IsNullOrEmpty(cleanMsg)) return;
        await Clients.Group(room.Code).SendAsync("chat:message", new{playerId=Context.ConnectionId,nickname=player.Nickname,color=player.Color,message=cleanMsg});
    }

    // ── TicTacToe ─────────────────────────────────────────────
    public async Task TictactoeMove(int cellIndex)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return;
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="tictactoe"||room.State!="PLAYING"||room.GameState is not TttGameState gs) return;
        var (error,evt,newGs,winner) = TicTacToe.MakeMove(gs, Context.ConnectionId, cellIndex);
        if (error!=null){await Clients.Caller.SendAsync("game:error",new{message=error});return;}
        room.GameState=newGs;
        await Clients.Group(room.Code).SendAsync("tictactoe:updated",new{gameState=newGs});
        if (evt=="win") await EndGameBg(room,winner!.Id,_hubContext,_rm,_logger);
        else if (evt=="draw") await EndGameBg(room,null,_hubContext,_rm,_logger);
        else if (room.IsVsBot && newGs.CurrentTurn==BotAI.BOT_ID && !newGs.IsDraw && newGs.Winner==null)
        { var hub2=_hubContext;var rm2=_rm;var log2=_logger; _ = Task.Run(async()=>{await Task.Delay(500);await TttBotMoveAsync(room,hub2,rm2,log2);}); }
    }

    private static async Task TttBotMoveAsync(Room room, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger log)
    {
        if (room.GameState is not TttGameState gs) return;
        int cell = BotAI.TttBotMove(gs, room.BotDifficulty);
        var (error,evt,newGs,winner) = TicTacToe.MakeMove(gs, BotAI.BOT_ID, cell);
        if (error!=null) return;
        room.GameState=newGs;
        await hub.Clients.Group(room.Code).SendAsync("tictactoe:updated",new{gameState=newGs});
        if (evt=="win") await EndGameBg(room,winner!.Id,hub,rm,log);
        else if (evt=="draw") await EndGameBg(room,null,hub,rm,log);
    }

    // ── WordChain ──────────────────────────────────────────────
    public async Task WordChainSubmit(string word)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return;
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="wordchain"||room.State!="PLAYING"||room.GameState is not WordChainGameState gs) return;
        var (error, valid, newGs) = WordChain.SubmitWord(gs, Context.ConnectionId, word);
        if (error != null) { await Clients.Caller.SendAsync("game:error", new{message=error}); return; }
        room.GameState = newGs;
        await Clients.Group(room.Code).SendAsync("wordchain:updated", new{gameState=newGs});
        if (newGs.GameOver) { await EndGameBg(room, newGs.Winner, _hubContext, _rm, _logger); return; }
        // Trigger bot if it's bot's turn
        if (room.IsVsBot && BotAI.IsBot(newGs.CurrentTurn))
        { var h=_hubContext;var r=_rm;var l=_logger; _ = Task.Run(async()=>await WordChainBotTurnAsync(room,h,r,l)); }
    }

    private static async Task WordChainBotTurnAsync(Room room, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger log)
    {
        if (room.GameState is not WordChainGameState gs || gs.GameOver) return;
        var (delayMs, word) = BotAI.WordChainBotMove(gs, room.BotDifficulty);
        await Task.Delay(Math.Min(delayMs, gs.TurnTimeLimit * 1000 - 500));
        if (room.GameState is not WordChainGameState gs2 || gs2.GameOver || !BotAI.IsBot(gs2.CurrentTurn)) return;
        if (word == null)
        {
            // Bot timed out intentionally — let timer handle elimination
            return;
        }
        var (error, valid, newGs) = WordChain.SubmitWord(gs2, BotAI.BOT_ID, word);
        if (error != null) return;
        room.GameState = newGs;
        await hub.Clients.Group(room.Code).SendAsync("wordchain:updated", new{gameState=newGs});
        if (newGs.GameOver) { await EndGameBg(room, newGs.Winner, hub, rm, log); return; }
        // Chain: if still bot's turn (multiplayer bot), keep going
        if (BotAI.IsBot(newGs.CurrentTurn))
        { _ = Task.Run(async()=>await WordChainBotTurnAsync(room,hub,rm,log)); }
    }

    // ── Snake ─────────────────────────────────────────────────
    public Task SnakeDirection(string direction)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return Task.CompletedTask;
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="snake"||room.State!="PLAYING"||room.GameState is not SnakeGameState gs) return Task.CompletedTask;
        Snake.SetDirection(gs, Context.ConnectionId, direction);
        return Task.CompletedTask;
    }

    // ── Pong ──────────────────────────────────────────────────
    public Task PongPaddle(string? direction)
    {
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="pong"||room.State!="PLAYING"||room.GameState is not PongGameState gs) return Task.CompletedTask;
        Pong.SetPaddleMoving(gs, Context.ConnectionId, direction);
        return Task.CompletedTask;
    }

    // ── Chess ─────────────────────────────────────────────────
    public async Task ChessMove(int from, int to, string? promotion)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return;
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="chess"||room.State!="PLAYING"||room.GameState is not ChessGameState gs) return;

        var (error,newGs,notation,isCheckmate,isCheck) = Chess.TryMove(gs, Context.ConnectionId, from, to, promotion);
        if (error!=null){await Clients.Caller.SendAsync("game:error",new{message=error});return;}
        room.GameState=newGs;

        await Clients.Group(room.Code).SendAsync("chess:updated", new{gameState=newGs,lastMove=new{from,to},notation,isCheck,isCheckmate});

        if (!newGs.GameOver)
        {
            var nextPlayer = newGs.Players.FirstOrDefault(p=>p.Side==newGs.CurrentTurn);
            var legalMoves = Chess.GetAllLegalMoves(newGs, newGs.CurrentTurn);
            if (nextPlayer!=null && nextPlayer.Id!=BotAI.BOT_ID)
                await Clients.Client(nextPlayer.Id).SendAsync("chess:legalMoves",new{moves=legalMoves});
            else if (room.IsVsBot && nextPlayer?.Id==BotAI.BOT_ID)
            { var hub2=_hubContext;var rm2=_rm;var log2=_logger; _ = Task.Run(async()=>{await Task.Delay(800);await ChessBotMoveAsync(room,hub2,rm2,log2);}); }
        }
        else await EndGameBg(room,newGs.Winner,_hubContext,_rm,_logger);
    }

    private static async Task ChessBotMoveAsync(Room room, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger log)
    {
        if (room.GameState is not ChessGameState gs || gs.GameOver) return;
        var (from, to, promo) = BotAI.ChessBotMove(gs, room.BotDifficulty);
        if (from<0||to<0) return;
        var (error,newGs,notation,isCheckmate,isCheck) = Chess.TryMove(gs, BotAI.BOT_ID, from, to, promo);
        if (error!=null) return;
        room.GameState=newGs;
        await hub.Clients.Group(room.Code).SendAsync("chess:updated",new{gameState=newGs,lastMove=new{from,to},notation,isCheck,isCheckmate});
        if (!newGs.GameOver)
        {
            var nextPlayer=newGs.Players.FirstOrDefault(p=>p.Side==newGs.CurrentTurn);
            var legalMoves=Chess.GetAllLegalMoves(newGs,newGs.CurrentTurn);
            if(nextPlayer!=null&&nextPlayer.Id!=BotAI.BOT_ID)
                await hub.Clients.Client(nextPlayer.Id).SendAsync("chess:legalMoves",new{moves=legalMoves});
        }
        else await EndGameBg(room,newGs.Winner,hub,rm,log);
    }

    public async Task ChessGetLegalMoves(int square)
    {
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="chess"||room.State!="PLAYING"||room.GameState is not ChessGameState gs) return;
        var moves = Chess.GetLegalMoves(gs, square);
        await Clients.Caller.SendAsync("chess:squareMoves",new{square,moves});
    }

    public async Task ChessResign()
    {
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="chess"||room.State!="PLAYING"||room.GameState is not ChessGameState gs) return;
        Chess.Resign(gs, Context.ConnectionId);
        room.GameState=gs;
        await Clients.Group(room.Code).SendAsync("chess:updated",new{gameState=gs,notation="resign",isCheck=false,isCheckmate=false});
        await EndGameBg(room,gs.Winner,_hubContext,_rm,_logger);
    }

    // ── Math Quiz ─────────────────────────────────────────────
    public async Task MathquizAnswer(string answer)
    {
        if (!_rm.CheckRateLimit(Context.ConnectionId)) return;
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.GameType!="mathquiz"||room.State!="PLAYING"||room.GameState is not MathGameState gs) return;
        var (valid,correct,allAnswered,correctAnswer) = MathQuiz.SubmitAnswer(gs, Context.ConnectionId, answer);
        if (!valid) return;
        await Clients.Group(room.Code).SendAsync("mathquiz:answered",new{playerId=Context.ConnectionId,correct,correctAnswer=correct?correctAnswer:(int?)null,players=gs.Players});
        if (allAnswered)
        {
            StopLoop(room.Code+"_math");
            var hub=_hubContext;var rm=_rm;var log=_logger;
            _ = Task.Run(async()=>{await Task.Delay(2000);if(room.State=="PLAYING")await SendNextQuestionBg(room,hub,rm,log);});
        }
    }


    // ── Poker ────────────────────────────────────────────────────
    public async Task PokerAction(string action, int amount)
    {
        if(!_rm.CheckRateLimit(Context.ConnectionId))return;
        var room=_rm.GetRoomByPlayer(Context.ConnectionId);
        if(room?.GameType!="poker"||room.State!="PLAYING"||room.GameState is not PokerGameState gs||gs.GameOver)return;
        await HandlePokerAction(room,gs,Context.ConnectionId,action,amount,_hubContext,_rm,_logger);
    }

    private static async Task HandlePokerAction(Room room,PokerGameState gs,string playerId,string action,int amount,IHubContext<GameHubSignalR> hub,RoomManager rm,ILogger log)
    {
        var(err,phaseOver)=Poker.Act(gs,playerId,action,amount);
        if(err!=null){await hub.Clients.Client(playerId).SendAsync("game:error",new{message=err});return;}
        room.GameState=gs;
        if(phaseOver&&gs.Winner!=null){
            // Won by everyone else folding
            await BroadcastPokerState(room,gs,hub,null);
            await Task.Delay(2000);
            if(gs.Players.Count<2||gs.Players.Count(p=>p.Chips>0)<2){gs.GameOver=true;await EndGameBg(room,gs.Winner,hub,rm,log);return;}
            gs.Winner=null;gs.DealerIndex=(gs.DealerIndex+1)%gs.Players.Count;gs.Round++;
            Poker.DealNewRound(gs);room.GameState=gs;
            await BroadcastPokerState(room,gs,hub,null);
            MaybeScheduleBot(room,gs,hub,rm,log);
        } else if(phaseOver){
            await BroadcastPokerState(room,gs,hub,null);
            await Task.Delay(600);
            bool handDone=Poker.DealNextStreet(gs);
            room.GameState=gs;
            if(handDone){
                string wid=Poker.DoShowdown(gs);
                await BroadcastPokerState(room,gs,hub,null);
                await Task.Delay(3000);
                if(gs.Players.Count<2||gs.Players.Count(p=>p.Chips>0)<2){gs.GameOver=true;await EndGameBg(room,wid,hub,rm,log);return;}
                gs.Winner=null;gs.DealerIndex=(gs.DealerIndex+1)%gs.Players.Count;gs.Round++;
                Poker.DealNewRound(gs);room.GameState=gs;
                await BroadcastPokerState(room,gs,hub,null);
                MaybeScheduleBot(room,gs,hub,rm,log);
            } else {
                await BroadcastPokerState(room,gs,hub,null);
                MaybeScheduleBot(room,gs,hub,rm,log);
            }
        } else {
            await BroadcastPokerState(room,gs,hub,null);
            MaybeScheduleBot(room,gs,hub,rm,log);
        }
    }

    private static void MaybeScheduleBot(Room room,PokerGameState gs,IHubContext<GameHubSignalR> hub,RoomManager rm,ILogger log)
    {
        if(!room.IsVsBot||gs.GameOver)return;
        var cur=gs.Players.ElementAtOrDefault(gs.CurrentPlayerIndex);
        if(cur==null||!BotAI.IsBot(cur.Id))return;
        _=Task.Run(async()=>{await Task.Delay(900+new Random().Next(600));await PokerBotTurn(room,hub,rm,log);});
    }

    private static async Task PokerBotTurn(Room room,IHubContext<GameHubSignalR> hub,RoomManager rm,ILogger log)
    {
        if(room.GameState is not PokerGameState gs||gs.GameOver)return;
        var cur=gs.Players.ElementAtOrDefault(gs.CurrentPlayerIndex);
        if(cur==null||!BotAI.IsBot(cur.Id))return;
        var(action,amount)=BotAI.PokerBotAction(gs,room.BotDifficulty,cur.Id);
        await HandlePokerAction(room,gs,cur.Id,action,amount,hub,rm,log);
    }

    private static async Task BroadcastPokerState(Room room,PokerGameState gs,IHubContext<GameHubSignalR> hub,string? showAllId)
    {
        bool showdown=gs.Phase=="showdown"||gs.Winner!=null;
        // Send each real player their private view
        foreach(var rp in room.Players.Where(p=>!BotAI.IsBot(p.Id)))
            await hub.Clients.Client(rp.Id).SendAsync("poker:state",MkPokerView(gs,showdown?null:rp.Id));
        // Also send full state to any spectators / Bot (no-op)
    }

    private static object MkPokerView(PokerGameState gs, string? viewerId)
    {
        bool showdown=gs.Phase=="showdown"||gs.Winner!=null;
        var players=gs.Players.Select(p=>new{
            p.Id,p.Nickname,p.Color,p.Chips,p.CurrentBet,p.Folded,p.AllIn,p.Score,
            hand=(showdown||p.Id==viewerId)?(object)p.Hand:p.Hand.Select(_=>(object)new{hidden=true}).ToList()
        }).ToList();
        return new{gs.Pot,gs.CurrentBet,gs.Phase,gs.CommunityCards,gs.CurrentPlayerIndex,
                   gs.SmallBlind,gs.BigBlind,gs.GameOver,gs.Winner,gs.WinReason,gs.Round,
                   gs.ActionLog,gs.MinRaise,gs.DealerIndex,players};
    }

    public async Task GamePlayAgain()
    {
        var room = _rm.GetRoomByPlayer(Context.ConnectionId);
        if (room?.State!="FINISHED") return;
        room.State="WAITING";room.GameState=null;room.ReadyPlayers.Clear();
        room.Players.ForEach(p=>{p.IsReady=false;p.Score=0;});
        StopAllLoops(room.Code);
        await Clients.Group(room.Code).SendAsync("room:updated",_rm.GetRoomInfo(room));
        await Clients.Group(room.Code).SendAsync("game:reset");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var player = _rm.GetPlayer(Context.ConnectionId);
        if (player?.RoomCode!=null)
        {
            var room = _rm.GetRoom(player.RoomCode);
            if (room!=null)
            {
                await Clients.Group(player.RoomCode).SendAsync("room:playerLeft",new{playerId=Context.ConnectionId,nickname=player.Nickname});
                var upd = _rm.LeaveRoom(Context.ConnectionId, player.RoomCode);
                if (upd?.Players.Count>0)
                {
                    await Clients.Group(player.RoomCode).SendAsync("room:updated",_rm.GetRoomInfo(upd));
                    if (room.State=="PLAYING"&&upd.Players.Count<RoomManager.GameConfigs[room.GameType].MinPlayers)
                    {
                        if (room.GameType=="chess"&&room.GameState is ChessGameState cgs)
                        {
                            Chess.Resign(cgs,Context.ConnectionId);
                            await Clients.Group(player.RoomCode).SendAsync("chess:updated",new{gameState=cgs,notation="disconnect",isCheck=false,isCheckmate=false});
                        }
                        await EndGameBg(upd,upd.Players.FirstOrDefault()?.Id,_hubContext,_rm,_logger);
                    }
                }
                else StopAllLoops(player.RoomCode);
            }
        }
        _rm.RemovePlayer(Context.ConnectionId);
        await Clients.All.SendAsync("stats:online",_rm.GetOnlineCountByGame());
        await base.OnDisconnectedAsync(exception);
    }

    // ══════════════════════════════════════════════════════════
    //  BACKGROUND HELPERS
    // ══════════════════════════════════════════════════════════
    private static async Task StartCountdownBg(Room room, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        if (room.State!="WAITING") return;
        room.State="READY";
        try { for(int i=3;i>0;i--){await hub.Clients.Group(room.Code).SendAsync("room:countdown",new{count=i});await Task.Delay(1000);}await StartGameBg(room,hub,rm,logger); }
        catch(Exception ex){logger.LogError(ex,"Countdown {Code}",room.Code);}
    }

    private static async Task StartGameBg(Room room, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        room.State="PLAYING";
        if (room.GameType=="tictactoe")
        {
            room.GameState=TicTacToe.CreateGameState(room.Players);
            await hub.Clients.Group(room.Code).SendAsync("game:start",new{gameType="tictactoe",gameState=room.GameState});
        }
        else if (room.GameType=="snake")
        {
            var gs=Snake.CreateGameState(room.Players); room.GameState=gs;
            await hub.Clients.Group(room.Code).SendAsync("game:start",new{gameType="snake",gameState=gs});
            _ = Task.Run(async()=>await RunSnakeLoop(room,gs,hub,rm,logger));
        }
        else if (room.GameType=="pong")
        {
            var gs=Pong.CreateGameState(room.Players); room.GameState=gs;
            await hub.Clients.Group(room.Code).SendAsync("game:start",new{gameType="pong",gameState=gs});
            _ = Task.Run(async()=>await RunPongLoop(room,gs,hub,rm,logger));
        }
        else if (room.GameType=="chess")
        {
            var gs=Chess.CreateGameState(room.Players); room.GameState=gs;
            await hub.Clients.Group(room.Code).SendAsync("game:start",new{gameType="chess",gameState=gs});
            var whitePlayer=gs.Players.FirstOrDefault(p=>p.Side=="white");
            if (whitePlayer!=null&&whitePlayer.Id!=BotAI.BOT_ID)
            {
                var whiteMoves=Chess.GetAllLegalMoves(gs,"white");
                await hub.Clients.Client(whitePlayer.Id).SendAsync("chess:legalMoves",new{moves=whiteMoves});
            }
            else if(room.IsVsBot&&whitePlayer?.Id==BotAI.BOT_ID)
            { var hub3=hub;var rm3=rm;var log3=logger; _ = Task.Run(async()=>{await Task.Delay(1000);await ChessBotMoveAsync(room,hub3,rm3,log3);}); }
            _ = Task.Run(async()=>await RunChessTimer(room,gs,hub,rm,logger));
        }
        else if (room.GameType=="mathquiz")
        {
            var gs=MathQuiz.CreateGameState(room.Players); room.GameState=gs;
            await hub.Clients.Group(room.Code).SendAsync("game:start",new{gameType="mathquiz",gameState=gs});
            _ = Task.Run(async()=>{await Task.Delay(1000);await SendNextQuestionBg(room,hub,rm,logger);});
        }
        else if (room.GameType=="poker")
        {
            var gs=Poker.CreateGameState(room.Players); room.GameState=gs;
            await hub.Clients.Group(room.Code).SendAsync("game:start",new{gameType="poker",gameState=MkPokerView(gs,null)});
            // Send private state to each player
            foreach(var rp in room.Players.Where(p=>!BotAI.IsBot(p.Id)))
                await hub.Clients.Client(rp.Id).SendAsync("poker:state",MkPokerView(gs,rp.Id));
            if(room.IsVsBot){var h4=hub;var r4=rm;var l4=logger;_=Task.Run(async()=>{await Task.Delay(800);if(BotAI.IsBot(gs.Players[gs.CurrentPlayerIndex].Id))await PokerBotTurn(room,h4,r4,l4);});}
        }
        else if (room.GameType=="wordchain")
        {
            var gs=WordChain.CreateGameState(room.Players); room.GameState=gs;
            await hub.Clients.Group(room.Code).SendAsync("game:start",new{gameType="wordchain",gameState=gs});
            _ = Task.Run(async()=>await RunWordChainTimer(room,gs,hub,rm,logger));
            // If bot goes first
            if(room.IsVsBot && BotAI.IsBot(gs.CurrentTurn))
            { var h5=hub;var r5=rm;var l5=logger; _ = Task.Run(async()=>{await Task.Delay(2000);await WordChainBotTurnAsync(room,h5,r5,l5);}); }
        }
        await hub.Clients.All.SendAsync("stats:online",rm.GetOnlineCountByGame());
    }

    private static async Task RunSnakeLoop(Room room, SnakeGameState gs, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        var cts=new CancellationTokenSource(); lock(LoopLock){GameLoops[room.Code]=cts;}
        try { while(!cts.IsCancellationRequested&&room.State=="PLAYING"){await Task.Delay(Snake.TickRate);if(cts.IsCancellationRequested||room.State!="PLAYING")break;
            // Bot AI: update direction each tick
            if(room.IsVsBot){var botDir=BotAI.SnakeBotDirection(gs,room.BotDifficulty);Snake.SetDirection(gs,BotAI.BOT_ID,botDir);}
            Snake.Tick(gs);await hub.Clients.Group(room.Code).SendAsync("snake:tick",gs);if(gs.GameOver){StopLoop(room.Code);await EndGameBg(room,gs.Winner,hub,rm,logger);break;}} }
        catch(Exception ex){logger.LogError(ex,"Snake {Code}",room.Code);}
    }

    private static async Task RunPongLoop(Room room, PongGameState gs, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        var cts=new CancellationTokenSource(); lock(LoopLock){GameLoops[room.Code]=cts;}
        try { while(!cts.IsCancellationRequested&&room.State=="PLAYING"){await Task.Delay(Pong.TickRate);if(cts.IsCancellationRequested||room.State!="PLAYING")break;
            if(room.IsVsBot){var botDir=BotAI.PongBotDirection(gs,room.BotDifficulty);Pong.SetPaddleMoving(gs,BotAI.BOT_ID,botDir);}
            Pong.Tick(gs);await hub.Clients.Group(room.Code).SendAsync("pong:tick",gs);if(gs.GameOver){StopLoop(room.Code);await EndGameBg(room,gs.Winner,hub,rm,logger);break;}} }
        catch(Exception ex){logger.LogError(ex,"Pong {Code}",room.Code);}
    }

    private static async Task RunChessTimer(Room room, ChessGameState gs, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        var cts=new CancellationTokenSource(); lock(LoopLock){GameLoops[room.Code+"_chess"]=cts;}
        try
        {
            while(!cts.IsCancellationRequested&&room.State=="PLAYING"&&!gs.GameOver)
            {
                await Task.Delay(1000);
                if(cts.IsCancellationRequested||room.State!="PLAYING"||gs.GameOver) break;
                if(gs.CurrentTurn=="white") gs.WhiteTime=Math.Max(0,gs.WhiteTime-1);
                else gs.BlackTime=Math.Max(0,gs.BlackTime-1);
                await hub.Clients.Group(room.Code).SendAsync("chess:timer",new{whiteTime=gs.WhiteTime,blackTime=gs.BlackTime,currentTurn=gs.CurrentTurn});
                if(gs.WhiteTime<=0||gs.BlackTime<=0)
                {
                    gs.GameOver=true;gs.WinReason="timeout";
                    gs.Winner=gs.WhiteTime<=0?gs.Players.FirstOrDefault(p=>p.Side=="black")?.Id:gs.Players.FirstOrDefault(p=>p.Side=="white")?.Id;
                    await hub.Clients.Group(room.Code).SendAsync("chess:updated",new{gameState=gs,notation="timeout",isCheck=false,isCheckmate=false});
                    StopLoop(room.Code+"_chess");
                    await EndGameBg(room,gs.Winner,hub,rm,logger); break;
                }
            }
        }
        catch(Exception ex){logger.LogError(ex,"Chess timer {Code}",room.Code);}
    }

    private static async Task RunWordChainTimer(Room room, WordChainGameState gs, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        var cts = new CancellationTokenSource(); lock(LoopLock){GameLoops[room.Code+"_wc"]=cts;}
        try
        {
            while (!cts.IsCancellationRequested && room.State == "PLAYING" && !gs.GameOver)
            {
                await Task.Delay(1000);
                if (cts.IsCancellationRequested || room.State != "PLAYING" || gs.GameOver) break;
                // Re-read from room in case bot updated it
                if (room.GameState is WordChainGameState latest) gs = latest;
                var elapsed = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - gs.TurnStartedAt) / 1000;
                int remaining = Math.Max(0, gs.TurnTimeLimit - (int)elapsed);
                await hub.Clients.Group(room.Code).SendAsync("wordchain:timer", new{remaining, currentTurn=gs.CurrentTurn});
                if (remaining <= 0 && !gs.GameOver)
                {
                    var (newGs, nick) = WordChain.EliminateCurrentPlayer(gs);
                    gs = newGs; room.GameState = gs;
                    await hub.Clients.Group(room.Code).SendAsync("wordchain:updated", new{gameState=gs});
                    if (gs.GameOver) { StopLoop(room.Code+"_wc"); await EndGameBg(room, gs.Winner, hub, rm, logger); break; }
                    // Trigger bot if next turn is bot
                    if (room.IsVsBot && BotAI.IsBot(gs.CurrentTurn))
                    { _ = Task.Run(async()=>await WordChainBotTurnAsync(room,hub,rm,logger)); }
                }
            }
        }
        catch (Exception ex) { logger.LogError(ex, "WordChain timer {Code}", room.Code); }
    }

    private static async Task EndGameBg(Room room, string? winnerId, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        room.State="FINISHED"; room.FinishedAt=DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        StopAllLoops(room.Code);
        var winner=room.Players.FirstOrDefault(p=>p.Id==winnerId);
        if(winner!=null){var wp=rm.GetPlayer(winnerId!);if(wp!=null)wp.Wins++;}
        await hub.Clients.Group(room.Code).SendAsync("game:over",new{winnerId,winnerNickname=winner?.Nickname,players=room.Players,gameState=room.GameState});
        await hub.Clients.All.SendAsync("stats:online",rm.GetOnlineCountByGame());
    }

    private static async Task SendNextQuestionBg(Room room, IHubContext<GameHubSignalR> hub, RoomManager rm, ILogger logger)
    {
        if(room.GameState is not MathGameState gs||room.State!="PLAYING") return;
        MathQuiz.NextQuestion(gs);
        if(gs.GameOver){await EndGameBg(room,gs.Winner,hub,rm,logger);return;}
        await hub.Clients.Group(room.Code).SendAsync("mathquiz:question",new{question=gs.CurrentQuestion,questionIndex=gs.QuestionIndex,totalQuestions=gs.TotalQuestions,timeLeft=MathQuiz.TimePerQ});
        // Bot answers after random delay
        if(room.IsVsBot){ var (delay,ans)=BotAI.MathBotAnswer(gs,room.BotDifficulty); _ = Task.Run(async()=>{await Task.Delay(delay);if(room.State=="PLAYING"&&gs.Phase=="QUESTION"){var (v,c,all,ca)=MathQuiz.SubmitAnswer(gs,BotAI.BOT_ID,ans);if(v){await hub.Clients.Group(room.Code).SendAsync("mathquiz:answered",new{playerId=BotAI.BOT_ID,correct=c,correctAnswer=c?ca:(int?)null,players=gs.Players});if(all){StopLoop(room.Code+"_math");await Task.Delay(2000);if(room.State=="PLAYING")await SendNextQuestionBg(room,hub,rm,logger);}}}});}
        var cts=new CancellationTokenSource(); lock(LoopLock){GameLoops[room.Code+"_math"]=cts;}
        _ = Task.Run(async()=>
        {
            try
            {
                var tl=MathQuiz.TimePerQ;
                while(tl>0&&!cts.IsCancellationRequested&&room.State=="PLAYING")
                {await Task.Delay(1000);if(cts.IsCancellationRequested||room.State!="PLAYING")return;tl--;await hub.Clients.Group(room.Code).SendAsync("mathquiz:timer",new{timeLeft=tl});}
                if(!cts.IsCancellationRequested&&room.State=="PLAYING")
                {await hub.Clients.Group(room.Code).SendAsync("mathquiz:timeUp",new{correctAnswer=gs.CorrectAnswer,players=gs.Players});await Task.Delay(2000);if(!cts.IsCancellationRequested&&room.State=="PLAYING")await SendNextQuestionBg(room,hub,rm,logger);}
            }
            catch(Exception ex){logger.LogError(ex,"Math {Code}",room.Code);}
        });
    }

    private static void StopLoop(string key)
    { lock(LoopLock){if(GameLoops.TryGetValue(key,out var cts)){cts.Cancel();cts.Dispose();GameLoops.Remove(key);}} }

    private static void StopAllLoops(string roomCode)
    { lock(LoopLock){foreach(var k in new[]{roomCode,roomCode+"_chess",roomCode+"_math",roomCode+"_poker"}) if(GameLoops.TryGetValue(k,out var cts)){cts.Cancel();cts.Dispose();GameLoops.Remove(k);}} }
}