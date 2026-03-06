using GameHub.Models;

namespace GameHub.Games;

// ══════════════════════════════════════════
//  TIC-TAC-TOE
// ══════════════════════════════════════════
public static class TicTacToe
{
    private static readonly int[][] Lines = {
        new[]{0,1,2}, new[]{3,4,5}, new[]{6,7,8},
        new[]{0,3,6}, new[]{1,4,7}, new[]{2,5,8},
        new[]{0,4,8}, new[]{2,4,6}
    };

    public static TttGameState CreateGameState(List<RoomPlayer> players) => new()
    {
        Board = new string?[9],
        CurrentTurn = players[0].Id,
        Players = new List<TttPlayer>
        {
            new() { Id=players[0].Id, Nickname=players[0].Nickname, Color=players[0].Color, Symbol="X" },
            new() { Id=players[1].Id, Nickname=players[1].Nickname, Color=players[1].Color, Symbol="O" }
        }
    };

    public static (int[]? line, string? winner) CheckWinner(string?[] board)
    {
        foreach (var line in Lines)
        {
            var a = board[line[0]]; var b = board[line[1]]; var c = board[line[2]];
            if (a != null && a == b && a == c) return (line, a);
        }
        return (null, null);
    }

    public static (string? error, string? evt, TttGameState gs, TttPlayer? winner) MakeMove(TttGameState gs, string playerId, int cellIndex)
    {
        if (gs.CurrentTurn != playerId) return ("not your turn", null, gs, null);
        if (gs.Board[cellIndex] != null) return ("cell taken", null, gs, null);
        if (gs.Winner != null || gs.IsDraw) return ("game over", null, gs, null);
        var player = gs.Players.FirstOrDefault(p => p.Id == playerId);
        if (player == null) return ("invalid player", null, gs, null);

        gs.Board[cellIndex] = player.Symbol;
        gs.MoveCount++;

        var (line, sym) = CheckWinner(gs.Board);
        if (sym != null) { gs.Winner = playerId; gs.WinLine = line; player.Score++; return (null, "win", gs, player); }
        if (gs.MoveCount == 9) { gs.IsDraw = true; return (null, "draw", gs, null); }

        gs.CurrentTurn = gs.Players.First(p => p.Id != playerId).Id;
        return (null, "move", gs, null);
    }
}

// ══════════════════════════════════════════
//  SNAKE
// ══════════════════════════════════════════
public static class Snake
{
    public const int GridSize = 30, TickRate = 150;
    public static readonly string[] Colors = { "#00ff88", "#ff4466", "#4488ff", "#ffaa00" };
    private static readonly Random Rng = new();

    public static SnakeGameState CreateGameState(List<RoomPlayer> players)
    {
        var starts = new[] { new Point{X=5,Y=5}, new Point{X=25,Y=25}, new Point{X=5,Y=25}, new Point{X=25,Y=5} };
        var snakes = players.Select((p, i) => new SnakePlayer
        {
            Id=p.Id, Nickname=p.Nickname, Color=Colors[i], Alive=true,
            Body=new List<Point>{new(){X=starts[i].X,Y=starts[i].Y},new(){X=starts[i].X-1,Y=starts[i].Y},new(){X=starts[i].X-2,Y=starts[i].Y}},
            Direction="RIGHT", NextDirection="RIGHT"
        }).ToList();
        var state = new SnakeGameState { Snakes=snakes, GridSize=GridSize };
        state.Food = GenFood(snakes);
        state.Players = players.Select((p,i) => (object)new{id=p.Id,nickname=p.Nickname,color=Colors[i],score=0,alive=true}).ToList();
        return state;
    }

    public static Point GenFood(List<SnakePlayer> snakes)
    {
        Point f; int t=0;
        do { f=new Point{X=Rng.Next(GridSize),Y=Rng.Next(GridSize)}; t++; }
        while (t<100 && snakes.Any(s=>s.Alive&&s.Body.Any(b=>b.X==f.X&&b.Y==f.Y)));
        return f;
    }

    public static void SetDirection(SnakeGameState gs, string playerId, string dir)
    {
        var s = gs.Snakes.FirstOrDefault(s=>s.Id==playerId);
        if (s==null||!s.Alive) return;
        var opp = new Dictionary<string,string>{{"UP","DOWN"},{"DOWN","UP"},{"LEFT","RIGHT"},{"RIGHT","LEFT"}};
        if (opp[dir]!=s.Direction) s.NextDirection=dir;
    }

    public static SnakeGameState Tick(SnakeGameState gs)
    {
        if (gs.GameOver) return gs;
        gs.Tick++;
        var alive = gs.Snakes.Where(s=>s.Alive).ToList();
        foreach (var s in alive)
        {
            s.Direction=s.NextDirection;
            var h=new Point{X=s.Body[0].X,Y=s.Body[0].Y};
            if (s.Direction=="UP") h.Y--; else if (s.Direction=="DOWN") h.Y++;
            else if (s.Direction=="LEFT") h.X--; else h.X++;
            // Wrap-around: đi xuyên tường sang tường đối diện
            h.X = ((h.X % GridSize) + GridSize) % GridSize;
            h.Y = ((h.Y % GridSize) + GridSize) % GridSize;
            s.Body.Insert(0,h);
            if (h.X==gs.Food.X&&h.Y==gs.Food.Y){s.Score++;gs.Food=GenFood(gs.Snakes);}
            else s.Body.RemoveAt(s.Body.Count-1);
        }
        foreach (var s in alive)
        {
            var h=s.Body[0];
            // Không chết vì tường - chỉ chết vì đâm rắn khác
            // Tự đâm thân mình thì vẫn chết
            for (int i=1;i<s.Body.Count;i++)
                if (h.X==s.Body[i].X&&h.Y==s.Body[i].Y){s.Alive=false;break;}
            if (s.Alive) foreach (var o in gs.Snakes)
            {
                if (o.Id==s.Id||!o.Alive) continue;
                if (o.Body.Any(b=>b.X==h.X&&b.Y==h.Y)){s.Alive=false;break;}
            }
        }
        gs.Players=gs.Snakes.Select(s=>(object)new{id=s.Id,nickname=s.Nickname,color=s.Color,score=s.Score,alive=s.Alive}).ToList();
        var still=gs.Snakes.Where(s=>s.Alive).ToList();
        if (still.Count<=1){gs.GameOver=true;if(still.Count==1){gs.Winner=still[0].Id;still[0].Score+=5;}}
        return gs;
    }
}

// ══════════════════════════════════════════
//  PONG
// ══════════════════════════════════════════
public static class Pong
{
    public const int W=800,H=500,PW=12,PH=80,BS=10,PS=6,WIN=5;
    public const double IBV=5.0;
    public const int TickRate=16;
    private static readonly Random Rng=new();

    public static PongGameState CreateGameState(List<RoomPlayer> players) => new()
    {
        Width=W,Height=H,
        Ball=new PongBall{X=W/2,Y=H/2,Vx=IBV*(Rng.NextDouble()>.5?1:-1),Vy=IBV*(Rng.NextDouble()>.5?1:-1),Size=BS},
        Paddles=new Dictionary<string,PongPaddle>{
            [players[0].Id]=new(){X=20,Y=H/2-PH/2,W=PW,H=PH,Nickname=players[0].Nickname,Side="left"},
            [players[1].Id]=new(){X=W-20-PW,Y=H/2-PH/2,W=PW,H=PH,Nickname=players[1].Nickname,Side="right"}
        },
        Players=new List<object>{
            new{id=players[0].Id,nickname=players[0].Nickname,score=0,side="left"},
            new{id=players[1].Id,nickname=players[1].Nickname,score=0,side="right"}
        }
    };

    public static void SetPaddleMoving(PongGameState gs,string playerId,string? dir)
    { if (gs.Paddles.TryGetValue(playerId,out var p)) p.Moving=dir; }

    public static PongGameState Tick(PongGameState gs)
    {
        if (gs.GameOver) return gs;
        var ball=gs.Ball;
        foreach (var (_,p) in gs.Paddles)
        { if (p.Moving=="up") p.Y=Math.Max(0,p.Y-PS); else if (p.Moving=="down") p.Y=Math.Min(H-PH,p.Y+PS); }
        ball.X+=ball.Vx; ball.Y+=ball.Vy;
        if (ball.Y<=0){ball.Y=0;ball.Vy=Math.Abs(ball.Vy);}
        if (ball.Y>=H-BS){ball.Y=H-BS;ball.Vy=-Math.Abs(ball.Vy);}
        foreach (var (id,p) in gs.Paddles)
        {
            if (ball.X<p.X+p.W&&ball.X+BS>p.X&&ball.Y<p.Y+p.H&&ball.Y+BS>p.Y)
            {
                var hit=(ball.Y+BS/2.0)-(p.Y+PH/2.0);
                var sp=Math.Min(IBV*(1+Math.Floor(gs.HitCount/5.0)*0.1),15);
                var rad=hit/(PH/2.0)*60*Math.PI/180;
                gs.HitCount++;
                if (p.Side=="left"){ball.X=p.X+p.W;ball.Vx=sp*Math.Cos(rad);}
                else{ball.X=p.X-BS;ball.Vx=-sp*Math.Cos(rad);}
                ball.Vy=sp*Math.Sin(rad); break;
            }
        }
        var rp=gs.Paddles.FirstOrDefault(kv=>kv.Value.Side=="right");
        var lp=gs.Paddles.FirstOrDefault(kv=>kv.Value.Side=="left");
        if (ball.X<0){rp.Value.Score++;if(rp.Value.Score>=WIN){gs.GameOver=true;gs.Winner=rp.Key;}else ResetBall(gs);}
        if (ball.X>W){lp.Value.Score++;if(lp.Value.Score>=WIN){gs.GameOver=true;gs.Winner=lp.Key;}else ResetBall(gs);}
        gs.Players=gs.Paddles.Select(kv=>(object)new{id=kv.Key,nickname=kv.Value.Nickname,score=kv.Value.Score,side=kv.Value.Side}).ToList();
        return gs;
    }

    public static void ResetBall(PongGameState gs)
    { gs.Ball=new PongBall{X=W/2,Y=H/2,Vx=IBV*(new Random().NextDouble()>.5?1:-1),Vy=IBV*(new Random().NextDouble()*2-1),Size=BS};gs.HitCount=0; }
}

// ══════════════════════════════════════════
//  CHESS
// ══════════════════════════════════════════
public static class Chess
{
    // Piece codes: w/b + K Q R B N P
    private static readonly string[] InitBoard = {
        "bR","bN","bB","bQ","bK","bB","bN","bR",
        "bP","bP","bP","bP","bP","bP","bP","bP",
        "","","","","","","","",
        "","","","","","","","",
        "","","","","","","","",
        "","","","","","","","",
        "wP","wP","wP","wP","wP","wP","wP","wP",
        "wR","wN","wB","wQ","wK","wB","wN","wR",
    };

    public static ChessGameState CreateGameState(List<RoomPlayer> players)
    {
        var board = InitBoard.Select(s => s == "" ? null : s).ToArray();
        return new ChessGameState
        {
            Board = board,
            CurrentTurn = "white",
            Players = new List<ChessPlayer>
            {
                new(){ Id=players[0].Id, Nickname=players[0].Nickname, Color=players[0].Color, Side="white" },
                new(){ Id=players[1].Id, Nickname=players[1].Nickname, Color=players[1].Color, Side="black" }
            },
            LastMoveAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
    }

    public static string? GetSide(ChessGameState gs, string playerId)
        => gs.Players.FirstOrDefault(p => p.Id == playerId)?.Side;

    // Returns null if move is valid, error string if invalid
    public static (string? error, ChessGameState gs, string? moveNotation, bool isCheckmate, bool isCheck) TryMove(
        ChessGameState gs, string playerId, int from, int to, string? promotion = null)
    {
        var side = GetSide(gs, playerId);
        if (side == null) return ("invalid player", gs, null, false, false);
        if (gs.CurrentTurn != side) return ("not your turn", gs, null, false, false);
        if (gs.GameOver) return ("game over", gs, null, false, false);

        var piece = gs.Board[from];
        if (piece == null || !piece.StartsWith(side[0].ToString())) return ("no piece", gs, null, false, false);

        var legal = GetLegalMoves(gs, from);
        if (!legal.Contains(to)) return ("illegal move", gs, null, false, false);

        // Execute move
        var notation = BuildNotation(gs, from, to, promotion);
        ExecuteMove(gs, from, to, promotion);

        // Update timer
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var elapsed = (int)((now - gs.LastMoveAt) / 1000);
        if (side == "white") gs.WhiteTime = Math.Max(0, gs.WhiteTime - elapsed);
        else gs.BlackTime = Math.Max(0, gs.BlackTime - elapsed);
        gs.LastMoveAt = now;

        // Switch turn
        var nextSide = side == "white" ? "black" : "white";
        gs.CurrentTurn = nextSide;
        gs.MoveCount++;
        gs.MoveHistory.Add(notation);

        // Check / checkmate detection
        gs.WhiteInCheck = IsInCheck(gs, "white");
        gs.BlackInCheck = IsInCheck(gs, "black");
        var currentInCheck = nextSide == "white" ? gs.WhiteInCheck : gs.BlackInCheck;
        var hasLegal = HasAnyLegalMove(gs, nextSide);

        bool isCheckmate = false;
        if (!hasLegal)
        {
            gs.GameOver = true;
            isCheckmate = currentInCheck;
            if (isCheckmate)
            {
                gs.WinReason = "checkmate";
                gs.Winner = playerId; // the one who moved wins
            }
            else
            {
                // Stalemate = draw, no winner
                gs.WinReason = "stalemate";
                gs.Winner = null;
            }
        }

        return (null, gs, notation, isCheckmate, currentInCheck);
    }

    public static void Resign(ChessGameState gs, string playerId)
    {
        gs.GameOver = true;
        gs.WinReason = "resign";
        gs.Winner = gs.Players.FirstOrDefault(p => p.Id != playerId)?.Id;
    }

    // ── Core chess logic ──────────────────────────────────────

    // Public wrapper for BotAI
    public static void ExecuteMovePublic(ChessGameState gs, int from, int to, string? promotion)
        => ExecuteMove(gs, from, to, promotion);

    private static void ExecuteMove(ChessGameState gs, int from, int to, string? promotion)
    {
        var board = gs.Board;
        var piece = board[from]!;
        var pType = piece[1];
        var side = piece[0] == 'w' ? "white" : "black";

        // En passant capture
        if (pType == 'P' && to == gs.EnPassantTarget)
        {
            var dir = side == "white" ? 1 : -1;
            board[to + dir * 8] = null;
        }

        // Castling
        if (pType == 'K')
        {
            var diff = to - from;
            if (Math.Abs(diff) == 2)
            {
                if (diff == 2) { board[from + 3] = null; board[from + 1] = (side == "white" ? "w" : "b") + "R"; }
                else { board[from - 4] = null; board[from - 1] = (side == "white" ? "w" : "b") + "R"; }
            }
            if (side == "white") { gs.WhiteCanCastleK = false; gs.WhiteCanCastleQ = false; }
            else { gs.BlackCanCastleK = false; gs.BlackCanCastleQ = false; }
        }

        // Rook move removes castling rights
        if (pType == 'R')
        {
            if (from == 0) gs.BlackCanCastleQ = false;
            if (from == 7) gs.BlackCanCastleK = false;
            if (from == 56) gs.WhiteCanCastleQ = false;
            if (from == 63) gs.WhiteCanCastleK = false;
        }

        // Set en passant target
        gs.EnPassantTarget = -1;
        if (pType == 'P' && Math.Abs(to - from) == 16)
            gs.EnPassantTarget = (from + to) / 2;

        // Promotion
        board[to] = (pType == 'P' && (to < 8 || to >= 56))
            ? (piece[0].ToString() + (promotion?.ToUpper() ?? "Q"))
            : piece;
        board[from] = null;
    }

    public static List<int> GetLegalMoves(ChessGameState gs, int sq)
    {
        var piece = gs.Board[sq];
        if (piece == null) return new();
        var side = piece[0] == 'w' ? "white" : "black";
        var pseudo = GetPseudoMoves(gs, sq);
        // Filter moves that leave own king in check
        return pseudo.Where(to =>
        {
            var copy = CloneState(gs);
            ExecuteMove(copy, sq, to, "Q");
            return !IsInCheck(copy, side);
        }).ToList();
    }

    private static bool HasAnyLegalMove(ChessGameState gs, string side)
    {
        for (int i = 0; i < 64; i++)
        {
            var p = gs.Board[i];
            if (p == null) continue;
            if ((side == "white" && p[0] == 'w') || (side == "black" && p[0] == 'b'))
                if (GetLegalMoves(gs, i).Count > 0) return true;
        }
        return false;
    }

    public static bool IsInCheck(ChessGameState gs, string side)
    {
        // Find king position
        var kingPiece = side == "white" ? "wK" : "bK";
        int kingPos = Array.IndexOf(gs.Board, kingPiece);
        if (kingPos < 0) return false;
        var opp = side == "white" ? "black" : "white";
        // Check if any opponent piece attacks king
        for (int i = 0; i < 64; i++)
        {
            var p = gs.Board[i];
            if (p == null) continue;
            if ((opp == "white" && p[0] == 'w') || (opp == "black" && p[0] == 'b'))
                if (GetPseudoMoves(gs, i).Contains(kingPos)) return true;
        }
        return false;
    }

    private static List<int> GetPseudoMoves(ChessGameState gs, int sq)
    {
        var board = gs.Board;
        var piece = board[sq];
        if (piece == null) return new();
        var side = piece[0] == 'w' ? "white" : "black";
        var opp = side == "white" ? "black" : "white";
        var pType = piece[1];
        var moves = new List<int>();
        int row = sq / 8, col = sq % 8;

        bool IsOpp(int s) { var p=board[s]; return p!=null && ((opp=="white"&&p[0]=='w')||(opp=="black"&&p[0]=='b')); }
        bool IsOwn(int s) { var p=board[s]; return p!=null && ((side=="white"&&p[0]=='w')||(side=="black"&&p[0]=='b')); }
        bool Empty(int s) => board[s] == null;

        void Slide(int dr, int dc)
        {
            int r=row+dr, c=col+dc;
            while (r>=0&&r<8&&c>=0&&c<8)
            {
                int s=r*8+c;
                if (IsOwn(s)) break;
                moves.Add(s);
                if (IsOpp(s)) break;
                r+=dr; c+=dc;
            }
        }

        void Jump(int dr, int dc)
        {
            int r=row+dr, c=col+dc;
            if (r>=0&&r<8&&c>=0&&c<8) { int s=r*8+c; if(!IsOwn(s)) moves.Add(s); }
        }

        switch (pType)
        {
            case 'P':
                int dir = side=="white" ? -1 : 1;
                int startRow = side=="white" ? 6 : 1;
                int fwd = sq + dir*8;
                if (fwd>=0&&fwd<64&&Empty(fwd)) {
                    moves.Add(fwd);
                    if (row==startRow && Empty(sq+dir*16)) moves.Add(sq+dir*16);
                }
                // Captures
                foreach (int dc2 in new[]{-1,1}) {
                    int c2=col+dc2, r2=row+dir;
                    if (c2>=0&&c2<8&&r2>=0&&r2<8) {
                        int s=r2*8+c2;
                        if (IsOpp(s)||s==gs.EnPassantTarget) moves.Add(s);
                    }
                }
                break;
            case 'N':
                foreach (var (dr,dc) in new[]{(-2,-1),(-2,1),(-1,-2),(-1,2),(1,-2),(1,2),(2,-1),(2,1)})
                    Jump(dr,dc);
                break;
            case 'B': Slide(-1,-1);Slide(-1,1);Slide(1,-1);Slide(1,1); break;
            case 'R': Slide(-1,0);Slide(1,0);Slide(0,-1);Slide(0,1); break;
            case 'Q': Slide(-1,-1);Slide(-1,1);Slide(1,-1);Slide(1,1);Slide(-1,0);Slide(1,0);Slide(0,-1);Slide(0,1); break;
            case 'K':
                foreach (var (dr,dc) in new[]{(-1,-1),(-1,0),(-1,1),(0,-1),(0,1),(1,-1),(1,0),(1,1)})
                    Jump(dr,dc);
                // Castling
                if (side=="white"&&sq==60) {
                    if (gs.WhiteCanCastleK&&Empty(61)&&Empty(62)) moves.Add(62);
                    if (gs.WhiteCanCastleQ&&Empty(59)&&Empty(58)&&Empty(57)) moves.Add(58);
                }
                if (side=="black"&&sq==4) {
                    if (gs.BlackCanCastleK&&Empty(5)&&Empty(6)) moves.Add(6);
                    if (gs.BlackCanCastleQ&&Empty(3)&&Empty(2)&&Empty(1)) moves.Add(2);
                }
                break;
        }
        return moves;
    }

    private static string BuildNotation(ChessGameState gs, int from, int to, string? promo)
    {
        var piece = gs.Board[from]!;
        var pType = piece[1];
        var cols = "abcdefgh";
        var fromStr = $"{cols[from%8]}{8-from/8}";
        var toStr = $"{cols[to%8]}{8-to/8}";
        return pType == 'P' ? toStr + (promo != null ? "="+promo.ToUpper() : "") : pType + toStr;
    }

    private static ChessGameState CloneState(ChessGameState gs) => new()
    {
        Board = (string?[])gs.Board.Clone(),
        CurrentTurn = gs.CurrentTurn,
        Players = gs.Players,
        EnPassantTarget = gs.EnPassantTarget,
        WhiteCanCastleK = gs.WhiteCanCastleK,
        WhiteCanCastleQ = gs.WhiteCanCastleQ,
        BlackCanCastleK = gs.BlackCanCastleK,
        BlackCanCastleQ = gs.BlackCanCastleQ,
    };

    // Get all legal moves for a side as dict from->list of to
    public static Dictionary<int,List<int>> GetAllLegalMoves(ChessGameState gs, string side)
    {
        var result = new Dictionary<int,List<int>>();
        for (int i=0;i<64;i++)
        {
            var p=gs.Board[i]; if(p==null) continue;
            if ((side=="white"&&p[0]=='w')||(side=="black"&&p[0]=='b'))
            {
                var m=GetLegalMoves(gs,i);
                if(m.Count>0) result[i]=m;
            }
        }
        return result;
    }
}

// ══════════════════════════════════════════
//  MATH QUIZ
// ══════════════════════════════════════════
public static class MathQuiz
{
    public const int Total=10, TimePerQ=8;
    private static readonly Random Rng=new();

    public static MathQuestion GenQ(int diff)
    {
        int a,b,answer; string op;
        if (diff<=3){a=Rng.Next(1,11);b=Rng.Next(1,11);op=Rng.NextDouble()>.5?"+":"-";if(op=="-"&&a<b)(a,b)=(b,a);answer=op=="+"?a+b:a-b;}
        else if(diff<=6){var t=Rng.NextDouble();if(t<.4){a=Rng.Next(2,11);b=Rng.Next(2,11);op="x";answer=a*b;}else if(t<.7){b=Rng.Next(2,11);answer=Rng.Next(2,11);a=b*answer;op="/";}else{a=Rng.Next(10,60);b=Rng.Next(10,60);op=Rng.NextDouble()>.5?"+":"-";if(op=="-"&&a<b)(a,b)=(b,a);answer=op=="+"?a+b:a-b;}}
        else{a=Rng.Next(20,120);b=Rng.Next(5,25);op=Rng.NextDouble()>.5?"+":"-";if(op=="-"&&a<b)(a,b)=(b,a);answer=op=="+"?a+b:a-b;}
        return new MathQuestion{Question=$"{a} {op} {b} = ?",Answer=answer};
    }

    public static MathGameState CreateGameState(List<RoomPlayer> players) => new()
    { Players=players.Select(p=>new MathPlayer{Id=p.Id,Nickname=p.Nickname,Color=p.Color}).ToList(), TotalQuestions=Total };

    public static MathGameState NextQuestion(MathGameState gs)
    {
        if (gs.QuestionIndex>=Total) return EndGame(gs);
        var diff=(int)Math.Ceiling((gs.QuestionIndex+1.0)/(Total/3.0));
        var q=GenQ(Math.Min(diff,9));
        gs.CurrentQuestion=q.Question;gs.CorrectAnswer=q.Answer;gs.TimeLeft=TimePerQ;gs.Phase="QUESTION";gs.FirstCorrect=null;gs.QuestionIndex++;
        gs.Players.ForEach(p=>p.Answered=false);
        return gs;
    }

    public static (bool valid,bool correct,bool allAnswered,int? correctAnswer) SubmitAnswer(MathGameState gs,string playerId,string answer)
    {
        if (gs.Phase!="QUESTION") return (false,false,false,null);
        var player=gs.Players.FirstOrDefault(p=>p.Id==playerId);
        if (player==null||player.Answered) return (false,false,false,null);
        player.Answered=true;
        var correct=answer.Trim()==gs.CorrectAnswer?.ToString();
        if(correct){if(gs.FirstCorrect==null){gs.FirstCorrect=playerId;player.Score+=3;}else player.Score+=1;}
        return (true,correct,gs.Players.All(p=>p.Answered),gs.CorrectAnswer);
    }

    public static MathGameState EndGame(MathGameState gs)
    {
        gs.GameOver=true;gs.Phase="FINISHED";
        var max=gs.Players.Max(p=>p.Score);
        gs.Winner=gs.Players.FirstOrDefault(p=>p.Score==max)?.Id;
        return gs;
    }
}

// ══════════════════════════════════════════
//  BOT AI - Tất cả 5 game
// ══════════════════════════════════════════
public static class BotAI
{
    public const string BOT_ID   = "BOT_AI_001";
    public const string BOT_NAME = "🤖 Bot";
    public const string BOT_COLOR= "#cc44ff";
    private static readonly Random Rng = new();

    // ── Tạo RoomPlayer cho Bot ────────────────────────────────
    public static RoomPlayer MakeBotPlayer() => new()
    {
        Id = BOT_ID, Nickname = BOT_NAME, Color = BOT_COLOR, IsReady = true
    };

    // ══════════════════════════════════════
    //  TIC-TAC-TOE  –  Minimax (bất bại)
    // ══════════════════════════════════════
    public static int TttBotMove(TttGameState gs)
    {
        // Bot always plays as 'O' (second player)
        string botSymbol = gs.Players.FirstOrDefault(p => p.Id == BOT_ID)?.Symbol ?? "O";
        string humanSymbol = botSymbol == "O" ? "X" : "O";

        int bestScore = int.MinValue, bestCell = -1;
        for (int i = 0; i < 9; i++)
        {
            if (gs.Board[i] != null) continue;
            gs.Board[i] = botSymbol;
            int score = TttMinimax(gs.Board, 0, false, botSymbol, humanSymbol);
            gs.Board[i] = null;
            if (score > bestScore) { bestScore = score; bestCell = i; }
        }
        return bestCell;
    }

    private static int TttMinimax(string?[] board, int depth, bool isMax, string bot, string human)
    {
        var (_, winner) = TicTacToe.CheckWinner(board);
        if (winner == bot)   return 10 - depth;
        if (winner == human) return depth - 10;
        if (board.All(c => c != null)) return 0;

        if (isMax)
        {
            int best = int.MinValue;
            for (int i = 0; i < 9; i++) {
                if (board[i] != null) continue;
                board[i] = bot;
                best = Math.Max(best, TttMinimax(board, depth+1, false, bot, human));
                board[i] = null;
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            for (int i = 0; i < 9; i++) {
                if (board[i] != null) continue;
                board[i] = human;
                best = Math.Min(best, TttMinimax(board, depth+1, true, bot, human));
                board[i] = null;
            }
            return best;
        }
    }

    // ══════════════════════════════════════
    //  SNAKE  –  Pathfinding (BFS to food)
    // ══════════════════════════════════════
    public static string SnakeBotDirection(SnakeGameState gs)
    {
        var bot = gs.Snakes.FirstOrDefault(s => s.Id == BOT_ID);
        if (bot == null || !bot.Alive || bot.Body.Count == 0) return "RIGHT";

        var head = bot.Body[0];
        var food = gs.Food;
        int gs2 = gs.GridSize;

        // Build occupied set (all snake bodies)
        var occupied = new HashSet<(int,int)>();
        foreach (var s in gs.Snakes)
            foreach (var b in s.Body) occupied.Add((b.X, b.Y));

        // BFS to food
        var dirs = new[] { ("UP",0,-1), ("DOWN",0,1), ("LEFT",-1,0), ("RIGHT",1,0) };
        var queue = new Queue<(int x, int y, string dir)>();
        var visited = new HashSet<(int,int)> { (head.X, head.Y) };

        foreach (var (d, dx, dy) in dirs)
        {
            int nx = head.X+dx, ny = head.Y+dy;
            if (nx<0||nx>=gs2||ny<0||ny>=gs2) continue;
            if (occupied.Contains((nx,ny))) continue;
            queue.Enqueue((nx, ny, d));
            visited.Add((nx, ny));
        }

        while (queue.Count > 0)
        {
            var (x, y, firstDir) = queue.Dequeue();
            if (x == food.X && y == food.Y) return firstDir;
            foreach (var (_, dx, dy) in dirs)
            {
                int nx = x+dx, ny = y+dy;
                if (nx<0||nx>=gs2||ny<0||ny>=gs2) continue;
                if (visited.Contains((nx,ny))||occupied.Contains((nx,ny))) continue;
                visited.Add((nx,ny));
                queue.Enqueue((nx, ny, firstDir));
            }
        }

        // Fallback: pick any safe direction
        foreach (var (d, dx, dy) in dirs)
        {
            int nx = head.X+dx, ny = head.Y+dy;
            if (nx>=0&&nx<gs2&&ny>=0&&ny<gs2&&!occupied.Contains((nx,ny)))
                return d;
        }
        return bot.Direction; // stay same if trapped
    }

    // ══════════════════════════════════════
    //  PONG  –  Bot tracks ball
    // ══════════════════════════════════════
    public static string? PongBotDirection(PongGameState gs)
    {
        if (!gs.Paddles.TryGetValue(BOT_ID, out var paddle)) return null;
        var ball = gs.Ball;
        double paddleMid = paddle.Y + Pong.PH / 2.0;
        double ballMid   = ball.Y + Pong.BS / 2.0;
        double diff = ballMid - paddleMid;
        // Dead zone ±10px to avoid jitter
        if (diff < -10) return "up";
        if (diff >  10) return "down";
        return null;
    }

    // ══════════════════════════════════════
    //  CHESS  –  Minimax depth-3 + eval
    // ══════════════════════════════════════
    private static readonly Dictionary<char,int> PieceVal = new()
    { {'P',100},{'N',320},{'B',330},{'R',500},{'Q',900},{'K',20000} };

    public static (int from, int to, string? promo) ChessBotMove(ChessGameState gs)
    {
        string botSide = gs.Players.FirstOrDefault(p => p.Id == BOT_ID)?.Side ?? "black";
        var (from, to, promo, _) = ChessAlphaBeta(gs, 3, int.MinValue, int.MaxValue, true, botSide);
        return (from, to, promo);
    }

    private static (int from, int to, string? promo, int score) ChessAlphaBeta(
        ChessGameState gs, int depth, int alpha, int beta, bool maximizing, string botSide)
    {
        if (depth == 0 || gs.GameOver)
            return (-1, -1, null, ChessEval(gs, botSide));

        var allMoves = Chess.GetAllLegalMoves(gs, gs.CurrentTurn);
        if (allMoves.Count == 0)
            return (-1, -1, null, ChessEval(gs, botSide));

        int bestFrom=-1, bestTo=-1; string? bestPromo=null;
        // Flatten & shuffle for variety
        var moveList = allMoves.SelectMany(kv => kv.Value.Select(t => (kv.Key, t))).ToList();
        moveList = moveList.OrderBy(_ => Rng.Next()).ToList(); // shuffle

        if (maximizing)
        {
            int best = int.MinValue;
            foreach (var (f, t) in moveList)
            {
                var clone = ChessClone(gs);
                string? promo = null;
                if (clone.Board[f]?[1] == 'P' && (t < 8 || t >= 56)) promo = "Q";
                Chess.ExecuteMovePublic(clone, f, t, promo);
                clone.CurrentTurn = clone.CurrentTurn == "white" ? "black" : "white";
                var (_, _, _, sc) = ChessAlphaBeta(clone, depth-1, alpha, beta, false, botSide);
                if (sc > best) { best=sc; bestFrom=f; bestTo=t; bestPromo=promo; }
                alpha = Math.Max(alpha, best);
                if (beta <= alpha) break;
            }
            return (bestFrom, bestTo, bestPromo, best);
        }
        else
        {
            int best = int.MaxValue;
            foreach (var (f, t) in moveList)
            {
                var clone = ChessClone(gs);
                string? promo = null;
                if (clone.Board[f]?[1] == 'P' && (t < 8 || t >= 56)) promo = "Q";
                Chess.ExecuteMovePublic(clone, f, t, promo);
                clone.CurrentTurn = clone.CurrentTurn == "white" ? "black" : "white";
                var (_, _, _, sc) = ChessAlphaBeta(clone, depth-1, alpha, beta, true, botSide);
                if (sc < best) { best=sc; bestFrom=f; bestTo=t; bestPromo=promo; }
                beta = Math.Min(beta, best);
                if (beta <= alpha) break;
            }
            return (bestFrom, bestTo, bestPromo, best);
        }
    }

    private static int ChessEval(ChessGameState gs, string botSide)
    {
        if (gs.GameOver)
        {
            if (gs.Winner == null) return 0;
            var winPlayer = gs.Players.FirstOrDefault(p => p.Id == gs.Winner);
            return winPlayer?.Side == botSide ? 100000 : -100000;
        }
        int score = 0;
        foreach (var p in gs.Board)
        {
            if (p == null) continue;
            int val = PieceVal[p[1]];
            score += p[0] == (botSide=="white"?'w':'b') ? val : -val;
        }
        return score;
    }

    private static ChessGameState ChessClone(ChessGameState gs) => new()
    {
        Board = (string?[])gs.Board.Clone(),
        CurrentTurn = gs.CurrentTurn,
        Players = gs.Players,
        GameOver = gs.GameOver,
        Winner = gs.Winner,
        EnPassantTarget = gs.EnPassantTarget,
        WhiteCanCastleK = gs.WhiteCanCastleK, WhiteCanCastleQ = gs.WhiteCanCastleQ,
        BlackCanCastleK = gs.BlackCanCastleK, BlackCanCastleQ = gs.BlackCanCastleQ,
        WhiteTime = gs.WhiteTime, BlackTime = gs.BlackTime,
        LastMoveAt = gs.LastMoveAt,
        MoveHistory = new List<string>(gs.MoveHistory),
        MoveCount = gs.MoveCount,
    };

    // ══════════════════════════════════════
    //  MATH QUIZ  –  Bot trả lời ngẫu nhiên
    // ══════════════════════════════════════
    public static (int delay, string answer) MathBotAnswer(MathGameState gs)
    {
        // Bot trả lời sau 1-5 giây, đúng 70% trả lời đúng
        int delayMs = Rng.Next(1000, 5000);
        bool correct = Rng.NextDouble() < 0.70;
        string answer = correct
            ? gs.CorrectAnswer?.ToString() ?? "0"
            : (gs.CorrectAnswer.GetValueOrDefault() + Rng.Next(-5,6)).ToString();
        return (delayMs, answer);
    }
}