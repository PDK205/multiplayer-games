namespace GameHub;

// ══════════════════════════════════════════════════════════════
//  HTML PAGES - Giống 100% bản Node.js, chỉ thay socket.io → SignalR
// ══════════════════════════════════════════════════════════════
public static class HtmlPages
{
    // CSS giống hệt bản Node.js
    private const string CSS = @"
@import url('https://fonts.googleapis.com/css2?family=Orbitron:wght@400;700;900&family=Rajdhani:wght@400;500;600;700&display=swap');
:root{--bg:#0a0a0f;--bg2:#12121a;--bg3:#1a1a26;--ac:#00ff88;--ac2:#ff4466;--ac3:#4488ff;--ac4:#ffaa00;--tx:#e0e0f0;--dim:#6a6a8a;--br:#2a2a3a;--glow:0 0 20px rgba(0,255,136,.3);--fd:'Orbitron',monospace;--fb:'Rajdhani',sans-serif;--r:12px}
*{margin:0;padding:0;box-sizing:border-box}
html,body{background:var(--bg);color:var(--tx);font-family:var(--fb);min-height:100vh;overflow-x:hidden}
body::before{content:'';position:fixed;inset:0;background-image:linear-gradient(rgba(0,255,136,.03) 1px,transparent 1px),linear-gradient(90deg,rgba(0,255,136,.03) 1px,transparent 1px);background-size:50px 50px;pointer-events:none;z-index:0}
h1,h2,h3{font-family:var(--fd);letter-spacing:.05em}
::-webkit-scrollbar{width:6px}::-webkit-scrollbar-track{background:var(--bg)}::-webkit-scrollbar-thumb{background:var(--br);border-radius:3px}
.hub-header{position:relative;z-index:10;padding:16px 24px;display:flex;align-items:center;justify-content:space-between;border-bottom:1px solid var(--br);background:rgba(10,10,15,.9);backdrop-filter:blur(10px);flex-wrap:wrap;gap:10px}
.hub-logo{font-family:var(--fd);font-size:1.3rem;font-weight:900;color:var(--ac);text-shadow:var(--glow);text-decoration:none}
.hub-logo span{color:var(--tx)}
.player-avatar{border-radius:50%;display:flex;align-items:center;justify-content:center;font-weight:700;font-family:var(--fd);border:2px solid rgba(255,255,255,.2);flex-shrink:0}
.input-field{background:var(--bg3);border:1px solid var(--br);color:var(--tx);padding:8px 14px;border-radius:8px;font-family:var(--fb);font-size:.95rem;outline:none;transition:border-color .2s,box-shadow .2s}
.input-field:focus{border-color:var(--ac);box-shadow:0 0 0 2px rgba(0,255,136,.1)}
.input-field::placeholder{color:var(--dim)}
.btn{padding:8px 18px;border:none;border-radius:8px;cursor:pointer;font-family:var(--fb);font-size:.95rem;font-weight:600;letter-spacing:.05em;transition:all .2s;display:inline-flex;align-items:center;gap:6px;white-space:nowrap}
.btn-primary{background:var(--ac);color:#000}.btn-primary:hover{background:#00cc70;transform:translateY(-1px);box-shadow:var(--glow)}
.btn-danger{background:var(--ac2);color:#fff}.btn-danger:hover{background:#cc3355;transform:translateY(-1px)}
.btn-secondary{background:transparent;color:var(--tx);border:1px solid var(--br)}.btn-secondary:hover{border-color:var(--ac);color:var(--ac)}
.btn-ghost{background:transparent;color:var(--dim);padding:6px 12px}.btn-ghost:hover{color:var(--tx)}
.btn:disabled{opacity:.5;cursor:not-allowed;transform:none!important}
.games-grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(200px,1fr));gap:18px;padding:28px;max-width:1100px;margin:0 auto;position:relative;z-index:1}
.game-card{background:var(--bg2);border:1px solid var(--br);border-radius:var(--r);padding:22px 18px;cursor:pointer;transition:all .25s;position:relative;overflow:hidden;text-decoration:none;display:block}
.game-card::before{content:'';position:absolute;top:0;left:0;right:0;height:3px;background:linear-gradient(90deg,var(--cc,var(--ac)),transparent);opacity:0;transition:opacity .2s}
.game-card:hover{border-color:var(--cc,var(--ac));transform:translateY(-4px);box-shadow:0 10px 40px rgba(0,0,0,.5)}
.game-card:hover::before{opacity:1}
.game-icon{font-size:2.4rem;margin-bottom:10px;display:block}
.game-title{font-family:var(--fd);font-size:.85rem;font-weight:700;color:var(--tx);margin-bottom:5px;letter-spacing:.08em}
.game-desc{font-size:.78rem;color:var(--dim);margin-bottom:12px;line-height:1.4}
.game-online{display:flex;align-items:center;gap:6px;font-size:.8rem;color:var(--dim)}
.online-dot{width:7px;height:7px;border-radius:50%;background:var(--ac);box-shadow:0 0 6px var(--ac);animation:pulse 2s infinite}
@keyframes pulse{0%,100%{opacity:1}50%{opacity:.4}}
.lobby-container{max-width:860px;margin:24px auto;padding:0 18px;position:relative;z-index:1}
.lobby-header{display:flex;align-items:center;gap:14px;margin-bottom:20px}
.lobby-title{font-family:var(--fd);font-size:1.2rem;color:var(--ac)}
.room-code-box{background:var(--bg2);border:1px solid var(--br);border-radius:var(--r);padding:14px 18px;margin-bottom:18px;display:flex;align-items:center;justify-content:space-between;flex-wrap:wrap;gap:10px}
.room-code{font-family:var(--fd);font-size:1.5rem;letter-spacing:.2em;color:var(--ac);text-shadow:var(--glow)}
.lobby-grid{display:grid;grid-template-columns:1fr 280px;gap:18px}
@media(max-width:650px){.lobby-grid{grid-template-columns:1fr}}
.player-list{background:var(--bg2);border:1px solid var(--br);border-radius:var(--r);padding:18px}
.player-list-title{font-size:.75rem;color:var(--dim);letter-spacing:.1em;text-transform:uppercase;margin-bottom:12px;font-family:var(--fd)}
.player-item{display:flex;align-items:center;gap:10px;padding:9px 0;border-bottom:1px solid var(--br);animation:fadeIn .3s ease}
.player-item:last-child{border-bottom:none}
.player-item-name{flex:1;font-weight:600}
.player-item-status{font-size:.78rem;padding:2px 8px;border-radius:4px}
.status-ready{background:rgba(0,255,136,.15);color:var(--ac)}
.status-waiting{background:rgba(106,106,138,.15);color:var(--dim)}
.waiting-slot{opacity:.4;padding:9px 0;display:flex;align-items:center;gap:10px;font-size:.88rem;color:var(--dim);border-bottom:1px dashed var(--br)}
.chat-box{background:var(--bg2);border:1px solid var(--br);border-radius:var(--r);display:flex;flex-direction:column;min-height:280px}
.chat-messages{flex:1;overflow-y:auto;padding:12px;display:flex;flex-direction:column;gap:5px;min-height:160px;max-height:260px}
.chat-msg{font-size:.83rem;line-height:1.4;animation:fadeIn .2s ease}
.chat-msg .msg-author{font-weight:700;margin-right:5px}
.chat-msg.is-correct{color:var(--ac);font-weight:600}
.chat-msg.is-system{color:var(--dim);font-style:italic}
.chat-input-row{display:flex;gap:7px;padding:9px;border-top:1px solid var(--br)}
.chat-input{flex:1;background:var(--bg);border:1px solid var(--br);color:var(--tx);padding:7px 11px;border-radius:6px;font-family:var(--fb);font-size:.88rem;outline:none}
.chat-input:focus{border-color:var(--ac)}
.lobby-actions{display:flex;gap:9px;margin-top:16px;flex-wrap:wrap}
.countdown-overlay{position:fixed;inset:0;background:rgba(0,0,0,.8);display:flex;align-items:center;justify-content:center;z-index:1000;animation:fadeIn .2s ease}
.countdown-number{font-family:var(--fd);font-size:8rem;font-weight:900;color:var(--ac);text-shadow:0 0 60px var(--ac);animation:countAnim .9s ease}
@keyframes countAnim{0%{transform:scale(1.5);opacity:0}50%{transform:scale(1);opacity:1}100%{transform:scale(.8);opacity:0}}
.game-header{display:flex;align-items:center;justify-content:space-between;padding:12px 22px;background:var(--bg2);border-bottom:1px solid var(--br);flex-wrap:wrap;gap:8px}
.scoreboard{display:flex;gap:18px;align-items:center;flex-wrap:wrap}
.score-item{display:flex;align-items:center;gap:8px;font-family:var(--fd)}
.score-name{font-size:.78rem;color:var(--dim)}
.score-value{font-size:1.2rem;font-weight:700}
.timer-box{font-family:var(--fd);font-size:1.2rem;font-weight:700;color:var(--ac4);min-width:38px;text-align:center}
.timer-box.urgent{color:var(--ac2);animation:blink .5s infinite}
@keyframes blink{0%,100%{opacity:1}50%{opacity:.5}}
.canvas-wrapper{display:flex;justify-content:center;align-items:center;padding:18px}
canvas{border-radius:8px;border:1px solid var(--br);display:block;max-width:100%}
.game-over-overlay{position:fixed;inset:0;background:rgba(0,0,0,.85);display:flex;align-items:center;justify-content:center;z-index:1000;animation:fadeIn .4s ease}
.game-over-card{background:var(--bg2);border:1px solid var(--br);border-radius:20px;padding:36px;text-align:center;max-width:380px;width:90%;animation:slideUp .4s ease}
.game-over-title{font-family:var(--fd);font-size:1.7rem;margin-bottom:7px}
.winner-name{font-size:1.1rem;color:var(--ac);margin-bottom:22px}
.result-list{list-style:none;margin-bottom:26px;text-align:left}
.result-item{display:flex;align-items:center;gap:11px;padding:9px 0;border-bottom:1px solid var(--br);font-size:.95rem}
.result-rank{font-family:var(--fd);font-size:.78rem;width:26px;text-align:center;color:var(--dim)}
.result-rank.gold{color:var(--ac4)}
.result-score{margin-left:auto;font-weight:700;font-family:var(--fd)}
.ttt-board{display:grid;grid-template-columns:repeat(15,1fr);gap:2px;max-width:min(600px,97vw);margin:10px auto;padding:8px;background:var(--bg2);border-radius:8px;border:2px solid var(--br);}
.ttt-cell{aspect-ratio:1;background:var(--bg3);border:1px solid rgba(255,255,255,0.07);border-radius:3px;cursor:pointer;font-size:clamp(.6rem,2.5vw,1rem);font-weight:800;transition:background .15s;display:flex;align-items:center;justify-content:center;color:var(--tx);-webkit-tap-highlight-color:transparent;touch-action:manipulation;}
.ttt-cell:hover:not(.taken){background:rgba(255,255,255,0.1);border-color:var(--ac)}
.ttt-cell.taken{cursor:not-allowed}
.ttt-cell.symbol-X{color:var(--ac)}
.ttt-cell.symbol-O{color:var(--ac2)}
.ttt-cell.winning{background:rgba(0,255,136,.25);border-color:var(--ac);animation:winPulse .5s ease 3}
.ttt-cell.winning.symbol-O{background:rgba(255,68,102,.2);border-color:var(--ac2)}
.turn-indicator{text-align:center;font-family:var(--fd);font-size:.88rem;color:var(--dim);padding:9px}
.turn-indicator span{color:var(--ac);font-weight:700}
.draw-layout{display:grid;grid-template-columns:1fr 260px;gap:0;height:calc(100vh - 58px)}
@media(max-width:680px){.draw-layout{grid-template-columns:1fr;height:auto}}
.draw-canvas-area{display:flex;flex-direction:column;padding:14px;gap:10px}
.draw-tools{display:flex;align-items:center;gap:7px;flex-wrap:wrap}
.color-swatch{width:26px;height:26px;border-radius:50%;cursor:pointer;border:2px solid transparent;transition:all .2s;flex-shrink:0}
.color-swatch:hover,.color-swatch.active{border-color:#fff;transform:scale(1.15)}
.draw-word-hint{font-family:var(--fd);font-size:1.2rem;letter-spacing:.12em;color:var(--ac);text-align:center}
.math-container{max-width:680px;margin:30px auto;padding:0 18px;position:relative;z-index:1}
.math-question{font-family:var(--fd);font-size:2.8rem;font-weight:700;text-align:center;color:var(--ac);text-shadow:var(--glow);padding:36px 18px;animation:questionIn .4s ease}
@keyframes questionIn{from{transform:scale(.8);opacity:0}to{transform:scale(1);opacity:1}}
.math-answer-area{display:flex;gap:9px;margin-bottom:20px}
.math-input{flex:1;background:var(--bg2);border:2px solid var(--br);color:var(--tx);padding:13px 18px;border-radius:10px;font-family:var(--fd);font-size:1.4rem;text-align:center;outline:none;transition:border-color .2s}
.math-input:focus{border-color:var(--ac)}
.pong-layout{display:flex;flex-direction:column;align-items:center;justify-content:center;min-height:calc(100vh - 58px);gap:14px;padding:18px}
.snake-layout{display:grid;grid-template-columns:1fr 220px;gap:18px;padding:18px;max-width:1050px;margin:0 auto}
@media(max-width:680px){.snake-layout{grid-template-columns:1fr}}
.progress-bar{height:4px;background:var(--br);border-radius:2px;overflow:hidden;margin-bottom:7px}
.progress-fill{height:100%;background:var(--ac);border-radius:2px;transition:width .9s linear}
.progress-fill.urgent{background:var(--ac2)}
.network-status{position:fixed;bottom:18px;left:50%;transform:translateX(-50%);background:var(--bg2);border:1px solid var(--ac2);color:var(--ac2);padding:7px 18px;border-radius:20px;font-size:.83rem;z-index:999}
@keyframes fadeIn{from{opacity:0}to{opacity:1}}
@keyframes slideUp{from{transform:translateY(30px);opacity:0}to{transform:translateY(0);opacity:1}}
@keyframes winPulse{0%,100%{transform:scale(1)}50%{transform:scale(1.06)}}
.hidden{display:none!important}
@media(max-width:480px){.hub-header{padding:12px 14px}.games-grid{grid-template-columns:1fr 1fr;padding:14px;gap:10px}.game-card{padding:14px 12px}.math-question{font-size:2rem}.room-code{font-size:1.1rem}}
/* ── Mobile touch controls ── */
.touch-dpad{display:none;position:fixed;bottom:24px;left:24px;z-index:200;user-select:none;}
@media(hover:none)and(pointer:coarse){.touch-dpad-auto{display:block!important;}}
.touch-dpad-row{display:flex;justify-content:center;gap:4px;margin:2px 0;}
.touch-btn{width:56px;height:56px;background:rgba(255,255,255,.12);border:2px solid rgba(255,255,255,.25);border-radius:12px;display:flex;align-items:center;justify-content:center;font-size:1.4rem;cursor:pointer;-webkit-tap-highlight-color:transparent;touch-action:manipulation;active-color:rgba(255,255,255,.3);}
.touch-btn:active{background:rgba(255,255,255,.3);}
.touch-paddle-area{display:none;position:fixed;bottom:0;z-index:200;width:100%;height:42%;pointer-events:none;}
.touch-paddle-half{position:absolute;bottom:0;width:50%;height:100%;pointer-events:auto;-webkit-tap-highlight-color:transparent;touch-action:none;display:flex;flex-direction:column;}
.touch-paddle-up{flex:1;display:flex;align-items:center;justify-content:center;font-size:1.8rem;opacity:.35;}
.touch-paddle-down{flex:1;display:flex;align-items:center;justify-content:center;font-size:1.8rem;opacity:.35;}
.touch-paddle-half:active .touch-paddle-up,.touch-paddle-half:active .touch-paddle-down{opacity:.7;}
@media(max-width:768px){
  .touch-dpad{display:block;}
  .touch-paddle-area{display:block;}
  .game-header{padding:8px 12px;gap:6px;}
  .scoreboard{gap:10px;}
  .score-value{font-size:1rem;}
  .chess-info{display:none!important;}
  .pong-layout{padding:8px;gap:8px;min-height:auto;}
  .snake-layout{grid-template-columns:1fr;padding:8px;}
  .math-container{margin:16px auto;padding:0 12px;}
  .math-question{font-size:2.2rem;padding:20px 12px;}
  .math-input{font-size:1.2rem;padding:10px 14px;}
  .ttt-board{max-width:98vw;padding:4px;gap:1px;}
  .ttt-cell{font-size:.55rem;}
  .lobby-container{padding:0 10px;margin:14px auto;}
  .room-code{font-size:1.2rem;}
  .btn{padding:10px 14px;font-size:.9rem;}
  .game-over-card{padding:22px 16px;}
}";

    // ── SignalR Client JS (thay thế socket.io) ─────────────────
    // Đây là phần khác duy nhất so với Node.js: dùng SignalR thay socket.io
    private const string SignalRClientJS = @"
const connection = new signalR.HubConnectionBuilder().withUrl('/gamehub').withAutomaticReconnect().build();
// Tạo wrapper 'socket' giống socket.io để các game JS không cần đổi
const socket = {
  _handlers: {},
  emit(event, ...args) {
    const method = event.replace(/:/g, '_').replace(/_([a-z])/g, (_, c) => c.toUpperCase());
    const map = {
      'player:join': 'PlayerJoin',
      'room:quickPlay': 'RoomQuickPlay',
      'room:create': 'RoomCreate',
      'room:join': 'RoomJoin',
      'room:leave': 'RoomLeave',
      'room:ready': 'RoomReady',
      'chat:message': 'ChatMessage',
      'tictactoe:move': 'TictactoeMove',
      'snake:direction': 'SnakeDirection',
      'pong:paddle': 'PongPaddle',
      'draw:stroke': 'DrawStroke',
      'draw:clear': 'DrawClear',
      'mathquiz:answer': 'MathquizAnswer',
      'game:playAgain': 'GamePlayAgain',
      'chess:move': 'ChessMove',
      'chess:getLegalMoves': 'ChessGetLegalMoves',
      'chess:getSquareMoves': 'ChessGetLegalMoves',
      'chess:resign': 'ChessResign',
      'room:playVsBot': 'RoomPlayVsBot',
      'poker:action': 'PokerAction',
      'wordchain:submit': 'WordChainSubmit'
    };
    const methodName = map[event] || event;
    const payload = args[0];
    let callArgs = [];
    if (payload !== undefined && payload !== null && typeof payload === 'object') {
      if (event === 'player:join') callArgs = [payload.nickname, payload.color];
      else if (event === 'room:quickPlay') callArgs = [payload.gameType];
      else if (event === 'room:create') callArgs = [payload.gameType];
      else if (event === 'room:join') callArgs = [payload.roomCode];
      else if (event === 'room:ready') callArgs = [payload.isReady];
      else if (event === 'chat:message') callArgs = [payload.message];
      else if (event === 'tictactoe:move') callArgs = [payload.cellIndex];
      else if (event === 'snake:direction') callArgs = [payload.direction];
      else if (event === 'pong:paddle') callArgs = [payload.direction ?? null];
      else if (event === 'draw:stroke') callArgs = [payload];
      else if (event === 'mathquiz:answer') callArgs = [payload.answer];
      else if (event === 'chess:move') callArgs = [payload.from, payload.to, payload.promotion ?? null];
      else if (event === 'chess:getLegalMoves') callArgs = [payload.square];
      else if (event === 'chess:getSquareMoves') callArgs = [payload.square];
      else if (event === 'chess:resign') callArgs = [];
      else if (event === 'room:playVsBot') callArgs = [payload.gameType, payload.difficulty||'medium'];
      else if (event === 'poker:action') callArgs = [payload.action, payload.amount||0];
      else if (event === 'wordchain:submit') callArgs = [payload.word];
      else callArgs = [payload];
    }
    connection.invoke(methodName, ...callArgs).catch(e => console.error(event, e));
  },
  on(event, handler) {
    if (!this._handlers[event]) this._handlers[event] = [];
    this._handlers[event].push(handler);
    connection.on(event, (...args) => handler(...args));
  }
};
let currentPlayer = null;
const PLAYER_COLORS=['#00ff88','#ff4466','#4488ff','#ffaa00','#cc44ff','#00ccff','#ff8800','#ff44cc'];
function getRandomColor(){return PLAYER_COLORS[Math.floor(Math.random()*PLAYER_COLORS.length)];}
function initPlayer(){return{nickname:localStorage.getItem('playerNickname')||'',color:localStorage.getItem('playerColor')||getRandomColor()};}
function registerPlayer(nickname,color){localStorage.setItem('playerNickname',nickname);localStorage.setItem('playerColor',color);socket.emit('player:join',{nickname,color});}
socket.on('player:joined',({player})=>{currentPlayer=player;window.currentPlayer=player;updatePlayerUI(player);if(window._pendingGameStart){const evt=window._pendingGameStart;window._pendingGameStart=null;setTimeout(()=>{if(window._gameStartHandlers)window._gameStartHandlers.forEach(fn=>fn(evt));},100);}});
window._gameStartHandlers=[];window._pendingGameStart=null;
function updatePlayerUI(p){
  const a=document.getElementById('headerAvatar'),n=document.getElementById('headerName'),w=document.getElementById('headerWins');
  if(a){a.style.background=p.color;a.textContent=p.nickname.charAt(0).toUpperCase();}
  if(n)n.textContent=p.nickname;
  if(w)w.textContent='🏆 '+(localStorage.getItem('sessionWins')||0);
}
connection.onreconnecting(()=>showNetworkStatus('🔴 Mất kết nối - Đang kết nối lại...'));
connection.onreconnected(()=>{hideNetworkStatus();const s=initPlayer();if(s.nickname)registerPlayer(s.nickname,s.color);});
socket.on('game:start',(data)=>{if(!window.currentPlayer){window._pendingGameStart=data;return;}if(window._gameStartHandlers)window._gameStartHandlers.forEach(fn=>fn(data));});
function showNetworkStatus(msg){let e=document.getElementById('networkStatus');if(!e){e=document.createElement('div');e.id='networkStatus';e.className='network-status';document.body.appendChild(e);}e.textContent=msg;e.style.display='block';}
function hideNetworkStatus(){const e=document.getElementById('networkStatus');if(e)e.style.display='none';}
function createAvatar(nickname,color,size=36){const d=document.createElement('div');d.className='player-avatar';d.style.cssText='background:'+color+';width:'+size+'px;height:'+size+'px;font-size:'+(size*.32)+'px;';d.textContent=nickname.charAt(0).toUpperCase();return d;}
async function copyToClipboard(text){try{await navigator.clipboard.writeText(text);}catch{const e=document.createElement('textarea');e.value=text;document.body.appendChild(e);e.select();document.execCommand('copy');document.body.removeChild(e);}}
function showToast(msg,type='info',dur=2500){const t=document.createElement('div');t.style.cssText='position:fixed;top:18px;right:18px;z-index:9999;background:'+(type==='success'?'var(--ac)':type==='error'?'var(--ac2)':'var(--bg2)')+';color:'+(type==='success'?'#000':'#fff')+';padding:9px 18px;border-radius:8px;font-size:.88rem;font-family:var(--fb);font-weight:600;box-shadow:0 4px 20px rgba(0,0,0,.4);animation:fadeIn .2s ease;';t.textContent=msg;document.body.appendChild(t);setTimeout(()=>t.remove(),dur);}
window.createAvatar=createAvatar;window.showToast=showToast;window.copyToClipboard=copyToClipboard;window.PLAYER_COLORS=PLAYER_COLORS;window.getRandomColor=getRandomColor;window.registerPlayer=registerPlayer;window.initPlayer=initPlayer;window.socket=socket;
connection.start().then(()=>{const s=initPlayer();if(s.nickname)registerPlayer(s.nickname,s.color);}).catch(e=>console.error('SignalR error:',e));";

    private const string LobbyJS = @"
let currentRoom=null;
function initLobby(gameType){
  const params=new URLSearchParams(window.location.search);
  const rc=params.get('room');
  if(rc)setTimeout(()=>socket.emit('room:join',{roomCode:rc}),600);
  const qp=document.getElementById('quickPlayBtn');if(qp)qp.addEventListener('click',()=>socket.emit('room:quickPlay',{gameType}));
  const vb=document.getElementById('vsBotBtn');
  if(vb)vb.addEventListener('click',()=>{
    const diffBtn=document.querySelector('.diff-btn.active');
    const diff=diffBtn?diffBtn.dataset.diff:'medium';
    socket.emit('room:playVsBot',{gameType,difficulty:diff});
  });
  document.querySelectorAll('.diff-btn').forEach(btn=>{
    btn.addEventListener('click',()=>{
      document.querySelectorAll('.diff-btn').forEach(b=>{
        b.style.background='transparent'; b.classList.remove('active');
      });
      btn.classList.add('active');
      const c=btn.style.color; btn.style.background=c+'22';
    });
  });
  const cr=document.getElementById('createRoomBtn');if(cr)cr.addEventListener('click',()=>socket.emit('room:create',{gameType}));
  const jrb=document.getElementById('joinRoomBtn'),jri=document.getElementById('joinRoomInput');
  if(jrb&&jri){jrb.addEventListener('click',()=>{const c=jri.value.trim().toUpperCase();c.length===6?socket.emit('room:join',{roomCode:c}):showToast('Mã phòng phải có 6 ký tự','error');});jri.addEventListener('keydown',e=>{if(e.key==='Enter')jrb.click();});}
  const rb=document.getElementById('readyBtn');if(rb){let r=false;rb.addEventListener('click',()=>{r=!r;socket.emit('room:ready',{isReady:r});rb.textContent=r?'⏳ Hủy Ready':'🚀 Ready';rb.className=r?'btn btn-secondary':'btn btn-primary';});}
  const cc=document.getElementById('copyCodeBtn');if(cc)cc.addEventListener('click',async()=>{if(!currentRoom)return;const u=window.location.origin+window.location.pathname+'?room='+currentRoom.code;await copyToClipboard(u);showToast('✅ Đã sao chép link!','success');});
  const bb=document.getElementById('backBtn');if(bb)bb.addEventListener('click',e=>{e.preventDefault();socket.emit('room:leave');window.location.href='/';});
  const cs=document.getElementById('chatSendBtn'),ci=document.getElementById('chatInput');
  if(cs&&ci){cs.addEventListener('click',()=>sendChat(ci));ci.addEventListener('keydown',e=>{if(e.key==='Enter')sendChat(ci);});}
}
function sendChat(input){const m=input.value.trim();if(!m||!currentRoom)return;socket.emit('chat:message',{message:m});input.value='';}
socket.on('room:updated',room=>{currentRoom=room;window.currentRoom=room;renderPlayers(room);renderCode(room.code);updateLobbyUI(room);});
socket.on('room:created',({roomCode})=>{showToast('✅ Tạo phòng: '+roomCode,'success');showLobbyView();});
socket.on('room:error',({message})=>showToast(message,'error'));
socket.on('room:playerLeft',({nickname})=>{if(nickname)addSysChat(nickname+' đã rời phòng');});
socket.on('room:countdown',({count})=>showCountdown(count));
socket.on('chat:message',({nickname,color,message,isCorrect,isSystem})=>addChatMessage(nickname,color,message,isCorrect,isSystem));
function renderPlayers(room){
  const el=document.getElementById('playerList');if(!el)return;el.innerHTML='';
  room.players.forEach(p=>{
    const item=document.createElement('div');item.className='player-item';
    const av=createAvatar(p.nickname,p.color||'#888');
    const nm=document.createElement('span');nm.className='player-item-name';nm.textContent=p.nickname+(window.currentPlayer&&p.id===window.currentPlayer.id?' (bạn)':'');
    const st=document.createElement('span');st.className='player-item-status '+(p.isReady?'status-ready':'status-waiting');st.textContent=p.isReady?'✓ Ready':'Chờ...';
    item.appendChild(av);item.appendChild(nm);item.appendChild(st);el.appendChild(item);
  });
  for(let i=room.players.length;i<room.maxPlayers;i++){const s=document.createElement('div');s.className='waiting-slot';s.innerHTML='<div style=""width:36px;height:36px;border-radius:50%;border:2px dashed var(--br);""></div><span>Chờ người chơi...</span>';el.appendChild(s);}
  const ce=document.getElementById('playerCount');if(ce)ce.textContent=room.players.length+'/'+room.maxPlayers;
}
function renderCode(code){const e=document.getElementById('roomCode');if(e)e.textContent=code;}
function updateLobbyUI(room){const rb=document.getElementById('readyBtn');if(rb)rb.disabled=room.state!=='WAITING';}
function showLobbyView(){const j=document.getElementById('joinArea'),l=document.getElementById('lobbyArea');if(j)j.classList.add('hidden');if(l)l.classList.remove('hidden');}
function showCountdown(count){showLobbyView();const old=document.getElementById('cdOverlay');if(old)old.remove();if(!count)return;const o=document.createElement('div');o.id='cdOverlay';o.className='countdown-overlay';o.innerHTML='<div class=""countdown-number"">'+count+'</div>';document.body.appendChild(o);setTimeout(()=>o.remove(),900);}
function addChatMessage(nickname,color,message,isCorrect=false,isSystem=false){
  const el=document.getElementById('chatMessages');if(!el)return;
  const m=document.createElement('div');m.className='chat-msg'+(isCorrect?' is-correct':'')+(isSystem?' is-system':'');
  if(isSystem)m.textContent=message;else m.innerHTML='<span class=""msg-author"" style=""color:'+color+'"">'+esc(nickname)+':</span>'+esc(message);
  el.appendChild(m);el.scrollTop=el.scrollHeight;while(el.children.length>60)el.removeChild(el.firstChild);
}
function addSysChat(msg){addChatMessage('','',msg,false,true);}
function esc(t){const d=document.createElement('div');d.textContent=t;return d.innerHTML;}
window.initLobby=initLobby;window.addChatMessage=addChatMessage;window.addSysChat=addSysChat;window.showLobbyView=showLobbyView;window.currentRoom=currentRoom;";

    private const string GameOverJS = @"
function showGameOver(winnerId,winnerNickname,players){
  const o=document.getElementById('gameOverOverlay');if(!o)return;o.classList.remove('hidden');
  const t=document.getElementById('gameOverTitle'),w=document.getElementById('gameOverWinner'),l=document.getElementById('resultList');
  if(winnerId===window.currentPlayer?.id){if(t){t.textContent='🏆 Bạn thắng!';t.style.color='var(--ac)';}const wins=parseInt(localStorage.getItem('sessionWins')||'0')+1;localStorage.setItem('sessionWins',wins);}
  else if(!winnerId){if(t)t.textContent='🤝 Hòa!';}
  else{if(t){t.textContent='😞 Bạn thua!';t.style.color='var(--ac2)';}}
  if(w)w.textContent=winnerNickname?winnerNickname+' thắng!':'';
  if(l&&players){const s=[...players].sort((a,b)=>b.score-a.score);l.innerHTML=s.map((p,i)=>'<li class=""result-item""><span class=""result-rank '+(i===0?'gold':'')+'"">'+(i===0?'🥇':i===1?'🥈':i===2?'🥉':(i+1))+'</span><div class=""player-avatar"" style=""background:'+(p.color||'#888')+';width:26px;height:26px;font-size:.72rem;"">'+p.nickname.charAt(0).toUpperCase()+'</div><span>'+p.nickname+'</span><span class=""result-score"">'+p.score+' điểm</span></li>').join('');}
  document.getElementById('playAgainBtn')?.addEventListener('click',()=>{o.classList.add('hidden');socket.emit('game:playAgain',{});},{once:true});
  document.getElementById('backHomeBtn')?.addEventListener('click',()=>{socket.emit('room:leave');window.location.href='/';},{once:true});
}";

    private const string TttJS = @"
const CARO_SIZE=15;
let tttState=null;
function initBoard(){const b=document.getElementById('tttBoard');b.innerHTML='';for(let i=0;i<CARO_SIZE*CARO_SIZE;i++){const c=document.createElement('div');c.className='ttt-cell';c.dataset.index=i;c.addEventListener('click',()=>{if(tttState&&tttState.currentTurn===window.currentPlayer?.id)socket.emit('tictactoe:move',{cellIndex:i});else showToast('Chưa đến lượt bạn!','error');});b.appendChild(c);}}
function renderBoard(gs){tttState=gs;const cells=document.querySelectorAll('.ttt-cell');gs.board.forEach((v,i)=>{const c=cells[i];if(v){c.textContent=v;c.classList.add('taken','symbol-'+v);}else{c.textContent='';c.className='ttt-cell';}});if(gs.winLine)gs.winLine.forEach(i=>cells[i].classList.add('winning'));updateTurnInfo(gs);}
function updateTurnInfo(gs){const e=document.getElementById('turnIndicator');if(!e)return;if(gs.winner||gs.isDraw){e.innerHTML=gs.isDraw?'🤝 Hòa!':'';return;}const cp=gs.players.find(p=>p.id===gs.currentTurn);if(!cp)return;if(gs.currentTurn===window.currentPlayer?.id){e.innerHTML='Lượt của bạn <span>('+gs.players.find(p=>p.id===window.currentPlayer.id)?.symbol+')</span>';e.style.color='var(--ac)';}else{e.innerHTML='Lượt của <span>'+cp.nickname+'</span>';e.style.color='var(--dim)';}}
function updateTTTScore(gs){const e=document.getElementById('scoreboard');if(!e||!gs.players)return;e.innerHTML=gs.players.map(p=>'<div class=""score-item""><div class=""player-avatar"" style=""background:'+(p.color||'#888')+';width:28px;height:28px;font-size:.75rem;"">'+p.nickname.charAt(0).toUpperCase()+'</div><div><div class=""score-name"">'+p.nickname+' ('+p.symbol+')</div><div class=""score-value"" style=""color:'+(p.symbol==='X'?'var(--ac)':'var(--ac2)')+'"">'+(p.score||0)+'</div></div></div>').join('');}
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{if(gameType!=='tictactoe')return;document.getElementById('lobbyArea').classList.add('hidden');document.getElementById('gameArea').classList.remove('hidden');document.getElementById('joinArea').classList.add('hidden');initBoard();renderBoard(gameState);updateTTTScore(gameState);const my=gameState.players.find(p=>p.id===window.currentPlayer?.id);if(my)showToast('Bạn là '+my.symbol+' — 5 liên tiếp để thắng!','info');});
socket.on('tictactoe:updated',({gameState})=>{renderBoard(gameState);updateTTTScore(gameState);});
socket.on('game:over',({winnerId,winnerNickname,players,gameState})=>{if(gameState){renderBoard(gameState);updateTTTScore(gameState);}showGameOver(winnerId,winnerNickname,players);});
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');tttState=null;});";

    private const string SnakeJS = @"
let snakeCanvas,snakeCtx,snakeState=null;const GS=16;
function initSnakeCanvas(gridSize){
  snakeCanvas=document.getElementById('snakeCanvas');
  const maxSz=Math.min(window.innerWidth*0.96,window.innerHeight*0.65,500);
  const sz=gridSize*GS;
  snakeCanvas.width=sz;snakeCanvas.height=sz;
  const disp=Math.min(sz,maxSz);
  snakeCanvas.style.width=disp+'px';snakeCanvas.style.height=disp+'px';
  snakeCtx=snakeCanvas.getContext('2d');
}
function renderSnake(gs){snakeState=gs;if(!snakeCanvas||!snakeCtx)return;const grid=gs.gridSize||gs.GridSize||30;const ctx=snakeCtx,s=GS;ctx.fillStyle='#0d0d14';ctx.fillRect(0,0,snakeCanvas.width,snakeCanvas.height);ctx.strokeStyle='rgba(255,255,255,.03)';ctx.lineWidth=.5;for(let i=0;i<=grid;i++){ctx.beginPath();ctx.moveTo(i*s,0);ctx.lineTo(i*s,snakeCanvas.height);ctx.stroke();ctx.beginPath();ctx.moveTo(0,i*s);ctx.lineTo(snakeCanvas.width,i*s);ctx.stroke();}const f=gs.food||gs.Food;ctx.fillStyle='#ff4466';ctx.shadowColor='#ff4466';ctx.shadowBlur=10;ctx.beginPath();ctx.arc(f.x*s+s/2,f.y*s+s/2,s/2-1,0,Math.PI*2);ctx.fill();ctx.shadowBlur=0;(gs.snakes||gs.Snakes||[]).forEach(sn=>{if(!(sn.alive??sn.Alive??true))ctx.globalAlpha=.2;(sn.body||sn.Body||[]).forEach((b,i)=>{const isH=i===0;ctx.fillStyle=sn.color;ctx.shadowColor=isH?sn.color:'transparent';ctx.shadowBlur=isH?8:0;const p=isH?1:2;ctx.fillRect(b.x*s+p,b.y*s+p,s-p*2,s-p*2);});ctx.shadowBlur=0;const _b0=(sn.body||sn.Body||[])[0];if(_b0){ctx.fillStyle=sn.color;ctx.font='bold 9px monospace';ctx.textAlign='center';ctx.fillText((sn.nickname||sn.Nickname||'').substring(0,6),_b0.x*s+s/2,_b0.y*s-2);}ctx.globalAlpha=1;});updateSnakeScore(gs);}
function updateSnakeScore(gs){const e=document.getElementById('scoreboard');if(!e)return;const _pl=gs.players||gs.Players||[];e.innerHTML=_pl.map(p=>'<div class=""score-item""><div style=""width:12px;height:12px;border-radius:2px;background:'+p.color+';box-shadow:0 0 6px '+p.color+';""></div><div><div class=""score-name"" style=""'+(p.alive?'':'text-decoration:line-through;opacity:.5;')+'"">'+(p.nickname||'')+'</div><div class=""score-value"" style=""color:'+p.color+'"">'+(p.score||0)+'</div></div></div>').join('');const ae=document.getElementById('aliveCount');if(ae)ae.textContent=_pl.filter(p=>p.alive||p.Alive).length+' rắn còn sống';}
const DK={'ArrowUp':'UP','w':'UP','W':'UP','ArrowDown':'DOWN','s':'DOWN','S':'DOWN','ArrowLeft':'LEFT','a':'LEFT','A':'LEFT','ArrowRight':'RIGHT','d':'RIGHT','D':'RIGHT'};
document.addEventListener('keydown',e=>{const d=DK[e.key];if(d&&snakeState&&!snakeState.gameOver){e.preventDefault();socket.emit('snake:direction',{direction:d});}});
// Touch swipe
let _stx=0,_sty=0;
document.addEventListener('touchstart',e=>{_stx=e.touches[0].clientX;_sty=e.touches[0].clientY;},{passive:true});
document.addEventListener('touchend',e=>{
  if(!snakeState||snakeState.gameOver)return;
  const dx=e.changedTouches[0].clientX-_stx,dy=e.changedTouches[0].clientY-_sty;
  if(Math.abs(dx)<20&&Math.abs(dy)<20)return;
  let dir;
  if(Math.abs(dx)>Math.abs(dy))dir=dx>0?'RIGHT':'LEFT';
  else dir=dy>0?'DOWN':'UP';
  socket.emit('snake:direction',{direction:dir});
},{passive:true});
function _snakeDpad(dir){if(snakeState&&!snakeState.gameOver)socket.emit('snake:direction',{direction:dir});}
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{
  if(gameType!=='snake')return;
  document.getElementById('joinArea').classList.add('hidden');
  document.getElementById('lobbyArea').classList.add('hidden');
  document.getElementById('gameArea').classList.remove('hidden');
  // Delay 1 frame so DOM renders before canvas init
  requestAnimationFrame(()=>{
    const gridSz=gameState.gridSize||gameState.GridSize||30;
    initSnakeCanvas(gridSz);
    renderSnake(gameState);
    let dp=document.getElementById('snakeDpad');
    if(!dp){dp=document.createElement('div');dp.id='snakeDpad';dp.className='touch-dpad';
    dp.innerHTML='<div class=""touch-dpad-row""><div class=""touch-btn"" onclick=""_snakeDpad(&quot;UP&quot;)"">▲</div></div><div class=""touch-dpad-row""><div class=""touch-btn"" onclick=""_snakeDpad(&quot;LEFT&quot;)"">◀</div><div class=""touch-btn"" style=""opacity:.2"">·</div><div class=""touch-btn"" onclick=""_snakeDpad(&quot;RIGHT&quot;)"">▶</div></div><div class=""touch-dpad-row""><div class=""touch-btn"" onclick=""_snakeDpad(&quot;DOWN&quot;)"">▼</div></div>';
    document.body.appendChild(dp);}
    const isTouchDevice=('ontouchstart' in window)||navigator.maxTouchPoints>0;
    if(isTouchDevice) dp.style.display='block';
    const isMobile=window.innerWidth<=768;
    showToast(isTouchDevice?'🐍 Vuốt hoặc dùng D-pad':'🐍 WASD để di chuyển','info',3000);
  });
});
socket.on('snake:tick',gs=>renderSnake(gs));
socket.on('game:over',({winnerId,winnerNickname,players})=>showGameOver(winnerId,winnerNickname,players));
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');snakeState=null;const dp=document.getElementById('snakeDpad');if(dp)dp.style.display='none';});";

    private const string PongJS = @"
let pongCanvas,pongCtx,pongState=null;const keysDown=new Set();
function initPongCanvas(w,h){
  pongCanvas=document.getElementById('pongCanvas');
  pongCanvas.width=w;pongCanvas.height=h;
  pongCtx=pongCanvas.getContext('2d');
  const maxW=window.innerWidth*0.98, maxH=window.innerHeight*0.5;
  const ratio=Math.min(maxW/w,maxH/h,1);
  pongCanvas.style.width=(w*ratio)+'px';pongCanvas.style.height=(h*ratio)+'px';
  window.onresize=()=>{if(!pongCanvas)return;const mw=window.innerWidth*0.98,mh=window.innerHeight*0.5;const r=Math.min(mw/w,mh/h,1);pongCanvas.style.width=(w*r)+'px';pongCanvas.style.height=(h*r)+'px';};
}
function renderPong(gs){pongState=gs;const ctx=pongCtx,W=pongCanvas.width,H=pongCanvas.height;ctx.fillStyle='#080810';ctx.fillRect(0,0,W,H);ctx.setLineDash([8,8]);ctx.strokeStyle='rgba(255,255,255,.12)';ctx.lineWidth=2;ctx.beginPath();ctx.moveTo(W/2,0);ctx.lineTo(W/2,H);ctx.stroke();ctx.setLineDash([]);Object.entries(gs.paddles).forEach(([id,p])=>{const me=id===window.currentPlayer?.id;ctx.fillStyle=me?'#00ff88':'#ff4466';ctx.shadowColor=me?'#00ff88':'#ff4466';ctx.shadowBlur=10;ctx.fillRect(p.x,p.y,p.w,p.h);ctx.shadowBlur=0;ctx.fillStyle=me?'#00ff88':'#ff4466';ctx.font='bold 11px monospace';ctx.textAlign='center';const nx=p.side==='left'?p.x+p.w+30:p.x-30;ctx.fillText(p.nickname.substring(0,8),nx,p.y+p.h/2+4);});const b=gs.ball;ctx.fillStyle='#fff';ctx.shadowColor='#fff';ctx.shadowBlur=12;ctx.fillRect(b.x,b.y,b.size,b.size);ctx.shadowBlur=0;if(gs.players){ctx.font='bold 46px monospace';ctx.fillStyle='rgba(255,255,255,.5)';ctx.textAlign='center';const lp=gs.players.find(p=>p.side==='left'),rp=gs.players.find(p=>p.side==='right');if(lp)ctx.fillText(lp.score,W/4,68);if(rp)ctx.fillText(rp.score,W*3/4,68);}}
document.addEventListener('keydown',e=>{if(!pongState||pongState.gameOver)return;if(['w','W','s','S','ArrowUp','ArrowDown'].includes(e.key))e.preventDefault();if(!keysDown.has(e.key)){keysDown.add(e.key);updatePaddle();}});
document.addEventListener('keyup',e=>{keysDown.delete(e.key);updatePaddle();});
function updatePaddle(){const id=window.currentPlayer?.id;if(!id||!pongState?.paddles[id])return;const side=pongState.paddles[id]?.side;let dir=null;if(side==='left'){if(keysDown.has('w')||keysDown.has('W'))dir='up';else if(keysDown.has('s')||keysDown.has('S'))dir='down';}else{if(keysDown.has('ArrowUp'))dir='up';else if(keysDown.has('ArrowDown'))dir='down';}socket.emit('pong:paddle',{direction:dir});}
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{
  if(gameType!=='pong')return;
  document.getElementById('lobbyArea').classList.add('hidden');
  document.getElementById('gameArea').classList.remove('hidden');
  document.getElementById('joinArea').classList.add('hidden');
  initPongCanvas(gameState.width,gameState.height);
  const myId=window.currentPlayer?.id;
  if(myId&&gameState.paddles[myId]){const side=gameState.paddles[myId].side,h=document.getElementById('pongHint');
    if(h){const isTch=('ontouchstart' in window)||navigator.maxTouchPoints>0;h.textContent=side==='left'?(isTch?'🟢 Bạn: TRÁI | Chạm để di chuyển':'🟢 Bạn: bên TRÁI | W/S'):(isTch?'🔴 Bạn: PHẢI | Chạm để di chuyển':'🔴 Bạn: bên PHẢI | ↑/↓');h.style.color=side==='left'?'var(--ac)':'var(--ac2)';}}
  renderPong(gameState);
  if(window._makePongTouchArea){window._makePongTouchArea();const pa=document.getElementById('pongTouchArea');if(pa)pa.style.display='block';}
});
// Touch paddle control

// pong touch handled inline in makePaddleArea
(function(){
  function makePaddleArea(){
    if(!('ontouchstart' in window)&&navigator.maxTouchPoints===0) return;
    let area=document.getElementById('pongTouchArea');
    if(area){area.style.display='block';return;}
    area=document.createElement('div');area.id='pongTouchArea';
    area.style.cssText='position:fixed;bottom:0;left:0;width:100%;height:48%;z-index:200;display:flex;flex-direction:column;pointer-events:auto;';
    // Top half = UP
    const U=document.createElement('div');
    U.style.cssText='flex:1;display:flex;align-items:center;justify-content:center;font-size:2.2rem;opacity:.3;border-bottom:1px solid rgba(255,255,255,.1);-webkit-tap-highlight-color:transparent;';
    U.textContent='▲';
    // Bottom half = DOWN
    const D=document.createElement('div');
    D.style.cssText='flex:1;display:flex;align-items:center;justify-content:center;font-size:2.2rem;opacity:.3;-webkit-tap-highlight-color:transparent;';
    D.textContent='▼';
    let curDir=null;
    function onTouch(e){
      e.preventDefault();
      const rect=area.getBoundingClientRect();
      const mid=rect.top+rect.height/2;
      const touch=e.touches[0]||e.changedTouches[0];
      const d=touch.clientY<mid?'up':'down';
      if(d!==curDir){curDir=d;socket.emit('pong:paddle',{direction:d});
        U.style.opacity=d==='up'?'0.8':'0.3';
        D.style.opacity=d==='down'?'0.8':'0.3';}
    }
    function onEnd(){curDir=null;socket.emit('pong:paddle',{direction:null});U.style.opacity='0.3';D.style.opacity='0.3';}
    area.addEventListener('touchstart',onTouch,{passive:false});
    area.addEventListener('touchmove',onTouch,{passive:false});
    area.addEventListener('touchend',onEnd,{passive:false});
    area.appendChild(U);area.appendChild(D);
    document.body.appendChild(area);
  }
  window._makePongTouchArea=makePaddleArea;
})();
socket.on('pong:tick',gs=>renderPong(gs));
socket.on('game:over',({winnerId,winnerNickname,players})=>showGameOver(winnerId,winnerNickname,players));
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');pongState=null;keysDown.clear();const pa=document.getElementById('pongTouchArea');if(pa)pa.style.display='none';});";

    private const string ChessJS = @"

let chessCanvas=null,chessCtx=null,CELL=0,chessState=null,myColor=null;
let selectedSq=null,highlightedSqs=new Set(),legalFromServer={},lastMove=null;

function initChessCanvas(){
  var wrap=document.getElementById('chessBoardWrap');
  if(!wrap)return;
  // Tính từ window - KHÔNG dùng wrap.clientWidth (gây shift mỗi nước đi)
  var size=Math.floor(Math.min(560,window.innerWidth*0.96)/8)*8;
  if(chessCanvas&&CELL===size/8)return; // size không đổi, bỏ qua
  CELL=size/8;
  if(!chessCanvas){
    chessCanvas=document.createElement('canvas');
    chessCanvas.id='chessCanvas';
    chessCanvas.style.cssText='display:block;cursor:pointer;border-radius:4px;touch-action:none;';
    chessCanvas.addEventListener('click',onCanvasClick);
    chessCanvas.addEventListener('touchend',function(e){
      e.preventDefault();
      var t=e.changedTouches[0];
      var r=chessCanvas.getBoundingClientRect();
      var x=Math.floor((t.clientX-r.left)/r.width*8);
      var y=Math.floor((t.clientY-r.top)/r.height*8);
      if(x>=0&&x<8&&y>=0&&y<8){var sq=myColor==='black'?(7-y)*8+(7-x):y*8+x;onSquareClick(sq);}
    },{passive:false});
    wrap.appendChild(chessCanvas);
  }
  chessCanvas.width=size;
  chessCanvas.height=size;
  chessCanvas.style.width=size+'px';
  chessCanvas.style.height=size+'px';
  chessCtx=chessCanvas.getContext('2d');
}

// ── Chess Pieces ──

function drawPiece(ctx, pc, cx, cy, C) {
  var isWhite = pc[0] === 'w';
  var type = pc[1];
  var symbols = { K:'♚', Q:'♛', R:'♜', B:'♝', N:'♞', P:'♟' };
  var sym = symbols[type] || '♟';

  ctx.save();
  ctx.shadowColor   = 'rgba(0,0,0,0.5)';
  ctx.shadowBlur    = C * 0.08;
  ctx.shadowOffsetX = C * 0.02;
  ctx.shadowOffsetY = C * 0.04;

  var fontSize = Math.floor(C * 0.72);
  ctx.font      = fontSize + 'px serif';
  ctx.textAlign = 'center';
  ctx.textBaseline = 'middle';

  // Draw outline for contrast
  ctx.strokeStyle = isWhite ? '#5a3a10' : '#ffffff';
  ctx.lineWidth   = Math.max(1.5, C * 0.04);
  ctx.lineJoin    = 'round';
  ctx.strokeText(sym, cx, cy + C * 0.04);

  ctx.fillStyle = isWhite ? '#ffffff' : '#1a1a1a';
  ctx.fillText(sym, cx, cy + C * 0.04);

  ctx.restore();
}





function drawBoard(gs){
  chessState=gs;
  if(!chessCtx||!chessCanvas)return;
  var ctx=chessCtx, C=CELL;
  var flip=(myColor==='black');

  function sq2rc(sq){ var r=Math.floor(sq/8),c=sq%8; return flip?[7-r,7-c]:[r,c]; }
  function idx2rc(idx){ return sq2rc(idx); }

  // Draw squares
  for(var r=0;r<8;r++){
    for(var c=0;c<8;c++){
      var sq=flip?(7-r)*8+(7-c):r*8+c;
      var light=(r+c)%2===0;
      if(highlightedSqs.has(sq)&&sq===selectedSq) ctx.fillStyle='#7fc97f';
      else if(highlightedSqs.has(sq)&&sq!==selectedSq) ctx.fillStyle=light?'#cde6a0':'#a5c46e';
      else if(lastMove&&(sq===lastMove.from||sq===lastMove.to)) ctx.fillStyle=light?'#f6f669':'#d4d428';
      else ctx.fillStyle=light?'#f0d9b5':'#b58863';
      ctx.fillRect(c*C,r*C,C,C);
      if(gs.whiteInCheck&&gs.board[sq]==='wK'){ctx.fillStyle='rgba(220,50,50,0.75)';ctx.fillRect(c*C,r*C,C,C);}
      if(gs.blackInCheck&&gs.board[sq]==='bK'){ctx.fillStyle='rgba(220,50,50,0.75)';ctx.fillRect(c*C,r*C,C,C);}
      if(highlightedSqs.has(sq)&&sq!==selectedSq){
        var hasPiece=gs.board[sq]!=null;
        if(hasPiece){
          ctx.strokeStyle='rgba(0,0,0,0.35)';ctx.lineWidth=C*0.08;
          ctx.beginPath();ctx.arc(c*C+C/2,r*C+C/2,C*0.46,0,Math.PI*2);ctx.stroke();
        } else {
          ctx.fillStyle='rgba(0,0,0,0.22)';
          ctx.beginPath();ctx.arc(c*C+C/2,r*C+C/2,C*0.16,0,Math.PI*2);ctx.fill();
        }
      }
    }
  }
  // Selected square border
  if(selectedSq!==null){
    var rc=idx2rc(selectedSq);
    ctx.strokeStyle='#3a9a3a'; ctx.lineWidth=4;
    ctx.strokeRect(rc[1]*C+2,rc[0]*C+2,C-4,C-4);
  }
  // Rank/file labels
  ctx.font='bold '+Math.floor(C*0.22)+'px monospace';
  ctx.textBaseline='top';
  for(var i=0;i<8;i++){
    var rankNum=flip?(i+1):(8-i);
    var fileLetter=String.fromCharCode(97+(flip?7-i:i));
    ctx.fillStyle=(i%2===0)?'#b58863':'#f0d9b5';
    ctx.textAlign='left'; ctx.fillText(rankNum,i*C+2,(i===0?C*0.05:0)+i*C);
    ctx.textAlign='right'; ctx.fillText(fileLetter,(i+1)*C-2,7*C+C*0.72);
  }
  // Draw pieces
  ctx.shadowColor='transparent'; ctx.shadowBlur=0; ctx.shadowOffsetX=0; ctx.shadowOffsetY=0;
  for(var idx=0;idx<64;idx++){
    var piece=gs.board[idx];
    if(!piece) continue;
    var rcc=idx2rc(idx);
    drawPiece(ctx,piece,rcc[1]*C+C/2,rcc[0]*C+C/2,C);
  }
  ctx.shadowColor='transparent'; ctx.shadowBlur=0; ctx.shadowOffsetX=0; ctx.shadowOffsetY=0;
  updateChessStatus(gs);
}

function renderBoard(gs){drawBoard(gs);}
function initChessBoard(){initChessCanvas();}

function onCanvasClick(e){
  if(!chessCtx||!CELL)return;
  var r=chessCanvas.getBoundingClientRect();
  // Dùng r.width/height (CSS size) để tính cột/hàng, không dùng canvas.width
  var x=Math.floor((e.clientX-r.left)/r.width*8);
  var y=Math.floor((e.clientY-r.top)/r.height*8);
  if(x<0||x>=8||y<0||y>=8)return;
  var sq=myColor==='black'?(7-y)*8+(7-x):y*8+x;
  onSquareClick(sq);
}

function onSquareClick(sq){
  if(!chessState||chessState.gameOver)return;
  var myId=window.currentPlayer&&window.currentPlayer.id;
  var myPlayer=chessState.players.find(function(p){return p.id===myId;});
  if(!myPlayer||chessState.currentTurn!==myPlayer.side)return;
  if(selectedSq!==null&&highlightedSqs.has(sq)&&sq!==selectedSq){
    var movingPiece=chessState.board[selectedSq];
    var isPromo=movingPiece&&movingPiece[1]==='P'&&(sq<8||sq>=56);
    if(isPromo){showPromoDialog(selectedSq,sq);}
    else{socket.emit('chess:move',{from:selectedSq,to:sq,promotion:null});clearSelection();}
    return;
  }
  var piece=chessState.board[sq];
  if(piece&&piece.startsWith(myPlayer.side[0])){
    selectedSq=sq; highlightedSqs=new Set([sq]);
    var moves=legalFromServer[sq];
    if(moves){moves.forEach(function(m){highlightedSqs.add(m);});}
    else{socket.emit('chess:getLegalMoves',{square:sq});}
    drawBoard(chessState);
  } else {clearSelection(); drawBoard(chessState);}
}

function clearSelection(){selectedSq=null;highlightedSqs=new Set();}

function showPromoDialog(from,to){
  var d=document.getElementById('promoDialog'); if(!d)return;
  d.style.display='flex';
  var side=chessState.board[from][0];
  var syms={Q:{w:'♛',b:'♛'},R:{w:'♜',b:'♜'},B:{w:'♝',b:'♝'},N:{w:'♞',b:'♞'}};
  var labels={Q:'Hậu',R:'Xe',B:'Tượng',N:'Mã'};
  ['Q','R','B','N'].forEach(function(p){
    var btn=document.getElementById('promo'+p);
    if(btn){
      btn.innerHTML='';
      var cv=document.createElement('canvas');cv.width=64;cv.height=64;
      var cx2=cv.getContext('2d');
      cx2.fillStyle=side==='w'?'#2a2a3a':'#1a1a2a';
      cx2.beginPath();cx2.roundRect(2,2,60,60,10);cx2.fill();
      cx2.shadowColor='rgba(0,0,0,0.6)';cx2.shadowBlur=4;cx2.shadowOffsetY=2;
      cx2.font='bold 38px serif';
      cx2.textAlign='center';cx2.textBaseline='middle';
      cx2.strokeStyle=side==='w'?'#5a3a10':'#ffffff';
      cx2.lineWidth=2;cx2.lineJoin='round';
      cx2.strokeText(syms[p][side]||syms[p].w,32,34);
      cx2.fillStyle=side==='w'?'#ffffff':'#1a1a1a';
      cx2.fillText(syms[p][side]||syms[p].w,32,34);
      cx2.shadowBlur=0;
      cx2.font='bold 10px sans-serif';
      cx2.fillStyle='rgba(255,255,255,0.6)';
      cx2.fillText(labels[p],32,58);
      cv.style.cssText='cursor:pointer;border-radius:8px;border:1px solid rgba(255,255,255,0.15);';
      cv.title=labels[p];
      btn.appendChild(cv);
      btn.title=labels[p];
      btn.onclick=function(){d.style.display='none';socket.emit('chess:move',{from:from,to:to,promotion:p});clearSelection();};
    }
  });
}

function updateChessStatus(gs){
  var myId=window.currentPlayer&&window.currentPlayer.id;
  var myPlayer=gs.players.find(function(p){return p.id===myId;});
  var oppPlayer=gs.players.find(function(p){return p.id!==myId;});
  var myTimeEl=document.getElementById('myTime');
  var oppTimeEl=document.getElementById('oppTime');
  var turnEl=document.getElementById('chessTurn');
  if(myTimeEl&&myPlayer)myTimeEl.textContent=fmtTime(myPlayer.side==='white'?gs.whiteTime:gs.blackTime);
  if(oppTimeEl&&oppPlayer)oppTimeEl.textContent=fmtTime(oppPlayer.side==='white'?gs.whiteTime:gs.blackTime);
  var isMyTurn=myPlayer&&gs.currentTurn===myPlayer.side;
  if(turnEl){turnEl.textContent=gs.gameOver?'Game over':(isMyTurn?'Your turn':'Waiting...');
    turnEl.style.color=isMyTurn?'var(--ac)':'var(--dim)';}
  var hist=document.getElementById('moveHistory');
  if(hist&&gs.moveHistory){hist.innerHTML='';gs.moveHistory.forEach(function(m,i){var d=document.createElement('span');d.className='chess-move';d.textContent=(i%2===0?(Math.floor(i/2)+1)+'. ':' ')+m+' ';hist.appendChild(d);});hist.scrollTop=hist.scrollHeight;}
}

function fmtTime(s){var m=Math.floor(s/60);var sc=s%60;return m+':'+(sc<10?'0':'')+sc;}

socket.on('chess:legalMoves',function(data){
  var moves=data.moves; legalFromServer={};
  if(moves)Object.keys(moves).forEach(function(k){legalFromServer[parseInt(k)]=moves[k];});
  if(selectedSq!==null&&legalFromServer[selectedSq]){
    highlightedSqs=new Set([selectedSq]);
    legalFromServer[selectedSq].forEach(function(m){highlightedSqs.add(m);});
    if(chessState)drawBoard(chessState);
  }
});

socket.on('chess:updated',function(data){
  var gs=data.gameState, lm=data.lastMove;
  lastMove=lm||null;
  drawBoard(gs);
});

socket.on('game:over',function(data){showGameOver(data.winnerId,data.winnerNickname,data.players);});

(window._gameStartHandlers=window._gameStartHandlers||[]).push(function(data){
  var gameType=data.gameType, gameState=data.gameState;
  if(gameType!=='chess')return;
  chessState=gameState; lastMove=null; legalFromServer={}; clearSelection();
  var myId=window.currentPlayer&&window.currentPlayer.id;
  var myPlayer=gameState.players.find(function(p){return p.id===myId;});
  myColor=myPlayer?myPlayer.side:null;
  document.getElementById('lobbyArea').classList.add('hidden');
  document.getElementById('gameArea').classList.remove('hidden');
  document.getElementById('joinArea').classList.add('hidden');
  var myColorEl=document.getElementById('myColorLabel');
  if(myColorEl)myColorEl.textContent=myColor==='white'?'You: White (first)':'You: Black (second)';
  requestAnimationFrame(function(){initChessCanvas();drawBoard(gameState);});
});

socket.on('game:reset',function(){
  document.getElementById('gameArea').classList.add('hidden');
  document.getElementById('lobbyArea').classList.remove('hidden');
  var o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');
  chessState=null;chessCanvas=null;chessCtx=null;CELL=0;clearSelection();lastMove=null;
});

document.getElementById('resignBtn')&&document.getElementById('resignBtn').addEventListener('click',function(){if(confirm('Resign?'))socket.emit('chess:resign',{});});
";

    private const string MathJS = @"
let mathPlayers=[],hasAnswered=false,maxTime=8;
function updateMathScore(players){mathPlayers=players;const e=document.getElementById('scoreboard');if(!e)return;e.innerHTML=players.map(p=>'<div class=""score-item""><div class=""player-avatar"" style=""background:'+(p.color||'#888')+';width:28px;height:28px;font-size:.75rem;"">'+p.nickname.charAt(0).toUpperCase()+'</div><div><div class=""score-name"">'+p.nickname+'</div><div class=""score-value"" style=""color:var(--ac2)"">'+p.score+'</div></div></div>').join('');}
function updateStatus(players){const e=document.getElementById('playersStatus');if(!e)return;e.innerHTML=players.map(p=>'<div style=""display:flex;align-items:center;gap:5px;padding:5px 11px;background:'+(p.answered?'rgba(0,255,136,.1)':'var(--bg2)')+';border:1px solid '+(p.answered?'var(--ac)':'var(--br)')+';border-radius:20px;font-size:.83rem;""><div class=""player-avatar"" style=""background:'+(p.color||'#888')+';width:20px;height:20px;font-size:.6rem;"">'+p.nickname.charAt(0).toUpperCase()+'</div>'+p.nickname+' '+(p.answered?'✓':'...')+'</div>').join('');}
function setInputEnabled(en){const i=document.getElementById('mathInput'),b=document.getElementById('submitBtn');if(i){i.disabled=!en;if(en){i.value='';i.focus();}}if(b)b.disabled=!en;}
function showFeedback(txt,color){const e=document.getElementById('answerFeedback');if(e){e.textContent=txt;e.style.color=color;setTimeout(()=>{if(e)e.textContent='';},1600);}}
function submitMath(){if(hasAnswered)return;const i=document.getElementById('mathInput');if(!i||!i.value.trim())return;socket.emit('mathquiz:answer',{answer:i.value.trim()});hasAnswered=true;setInputEnabled(false);}
document.getElementById('submitBtn')?.addEventListener('click',submitMath);
document.getElementById('mathInput')?.addEventListener('keydown',e=>{if(e.key==='Enter')submitMath();});
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{if(gameType!=='mathquiz')return;document.getElementById('lobbyArea').classList.add('hidden');document.getElementById('gameArea').classList.remove('hidden');document.getElementById('joinArea').classList.add('hidden');updateMathScore(gameState.players);setInputEnabled(false);});
socket.on('mathquiz:question',({question,questionIndex,totalQuestions,timeLeft})=>{hasAnswered=false;maxTime=timeLeft;const q=document.getElementById('mathQuestion');if(q)q.textContent=question;const p=document.getElementById('questionProgress');if(p)p.textContent=questionIndex+'/'+totalQuestions;const t=document.getElementById('timerDisplay');if(t){t.textContent=timeLeft;t.className='timer-box';}const b=document.getElementById('timerBar');if(b){b.style.width='100%';b.className='progress-fill';}setInputEnabled(true);if(mathPlayers.length>0)updateStatus(mathPlayers.map(p=>({...p,answered:false})));});
socket.on('mathquiz:timer',({timeLeft})=>{const t=document.getElementById('timerDisplay'),b=document.getElementById('timerBar');if(t){t.textContent=timeLeft;t.className='timer-box'+(timeLeft<=3?' urgent':'');}if(b){b.style.width=(timeLeft/maxTime*100)+'%';b.className='progress-fill'+(timeLeft<=3?' urgent':'');}});
socket.on('mathquiz:answered',({playerId,correct,correctAnswer,players})=>{updateMathScore(players);updateStatus(players);if(playerId===window.currentPlayer?.id)showFeedback(correct?'✅ Đúng! +điểm':'❌ Sai!',correct?'var(--ac)':'var(--ac2)');});
socket.on('mathquiz:timeUp',({correctAnswer,players})=>{hasAnswered=true;setInputEnabled(false);showFeedback('⏰ Đáp án: '+correctAnswer,'var(--ac4)');updateMathScore(players);});
socket.on('game:over',({winnerId,winnerNickname,players})=>showGameOver(winnerId,winnerNickname,players));
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');hasAnswered=false;mathPlayers=[];});";

    // ── HTML Builders ─────────────────────────────────────────
    private static string SignalRScripts => @"<script src=""https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js""></script>";

    private static string GameOverOverlay => @"
<div id=""gameOverOverlay"" class=""game-over-overlay hidden"">
  <div class=""game-over-card"">
    <div class=""game-over-title"" id=""gameOverTitle"">🏁 Kết thúc!</div>
    <div class=""winner-name"" id=""gameOverWinner""></div>
    <ul class=""result-list"" id=""resultList""></ul>
    <div style=""display:flex;gap:10px;justify-content:center;"">
      <button id=""playAgainBtn"" class=""btn btn-primary"">🔄 Chơi lại</button>
      <button id=""backHomeBtn"" class=""btn btn-secondary"">🏠 Trang chủ</button>
    </div>
  </div>
</div>";

    private static string BaseScripts(string gameJs, string gameType) => $@"
{SignalRScripts}
<script>{SignalRClientJS}</script>
<script>{LobbyJS}</script>
<script>{GameOverJS}</script>
<script>{gameJs}</script>
<script>
  const saved=initPlayer();
  if(saved.nickname){{
    connection.start().then(()=>registerPlayer(saved.nickname,saved.color)).catch(e=>console.error(e));
  }} else {{ window.location.href='/'; }}
  document.getElementById('quickPlayBtn2')?.addEventListener('click',()=>socket.emit('room:quickPlay',{{gameType:'{gameType}'}}));
  socket.on('room:updated',(room)=>{{if(!room||room.state==='WAITING'||room.state==='READY')showLobbyView();}});
  initLobby('{gameType}');
</script>";

    private static string Header => @"
<header class=""hub-header"">
  <a href=""/"" class=""hub-logo"" id=""backBtn"">⬅ GAME<span>HUB</span></a>
  <div style=""display:flex;align-items:center;gap:10px;"">
    <div class=""player-avatar"" id=""headerAvatar"" style=""width:34px;height:34px;font-size:.85rem;""></div>
    <span id=""headerName"" style=""font-weight:600;font-family:var(--fd);font-size:.83rem;""></span>
    <span id=""headerWins"" style=""font-size:.82rem;color:var(--ac4);font-weight:600;""></span>
  </div>
</header>";

    private static string JoinPanel(string icon, string title, string color, string gameType, string desc) => $@"
<div id=""joinArea"" style=""max-width:480px;margin:36px auto;padding:0 18px;position:relative;z-index:1;"">
  <h2 style=""font-family:var(--fd);color:{color};margin-bottom:22px;text-align:center;"">{icon} {title}</h2>
  <div style=""background:var(--bg2);border:1px solid var(--br);border-radius:var(--r);padding:22px;"">
    <div style=""font-size:.78rem;color:var(--dim);margin-bottom:12px;"">{desc}</div>
    <div style=""display:flex;flex-direction:column;gap:11px;"">
      <button id=""quickPlayBtn"" class=""btn btn-primary"" style=""width:100%;justify-content:center;padding:12px;"">⚡ Quick Play</button>
      <div style=""background:#1a1a2e;border:1px solid #cc44ff44;border-radius:8px;padding:10px;"">
        <div style=""font-size:.72rem;color:#cc44ff;margin-bottom:7px;letter-spacing:.05em;"">⚙️ ĐỘ KHÓ BOT</div>
        <div style=""display:flex;gap:6px;margin-bottom:8px;"">
          <button class=""diff-btn"" data-diff=""easy"" style=""flex:1;padding:7px;border-radius:6px;border:2px solid #44ff88;background:transparent;color:#44ff88;cursor:pointer;font-size:.8rem;font-weight:700;"">😊 Dễ</button>
          <button class=""diff-btn active"" data-diff=""medium"" style=""flex:1;padding:7px;border-radius:6px;border:2px solid #ffaa00;background:#ffaa0022;color:#ffaa00;cursor:pointer;font-size:.8rem;font-weight:700;"">😐 TB</button>
          <button class=""diff-btn"" data-diff=""hard"" style=""flex:1;padding:7px;border-radius:6px;border:2px solid #ff4466;background:transparent;color:#ff4466;cursor:pointer;font-size:.8rem;font-weight:700;"">💀 Khó</button>
        </div>
        <button id=""vsBotBtn"" class=""btn btn-secondary"" style=""width:100%;justify-content:center;padding:12px;border-color:#cc44ff;color:#cc44ff;"">🤖 Chơi vs Bot</button>
      </div>
      <div style=""display:flex;gap:7px;"">
        <input type=""text"" id=""joinRoomInput"" class=""input-field"" placeholder=""Mã phòng..."" maxlength=""6"" style=""flex:1;text-transform:uppercase;letter-spacing:.2em;font-family:var(--fd);"">
        <button id=""joinRoomBtn"" class=""btn btn-secondary"">Vào</button>
      </div>
      <button id=""createRoomBtn"" class=""btn btn-secondary"" style=""width:100%;justify-content:center;"">➕ Tạo phòng mới</button>
    </div>
  </div>
</div>";

    private static string LobbyPanel(string icon, string title, string color, int maxP) => $@"
<div id=""lobbyArea"" class=""lobby-container hidden"">
  <div class=""lobby-header""><h2 class=""lobby-title"" style=""color:{color};"">{icon} {title}</h2></div>
  <div class=""room-code-box"">
    <div><div style=""font-size:.73rem;color:var(--dim);margin-bottom:4px;"">MÃ PHÒNG</div><div class=""room-code"" id=""roomCode"">------</div></div>
    <button id=""copyCodeBtn"" class=""btn btn-secondary"">📋 Copy Link</button>
  </div>
  <div class=""lobby-grid"">
    <div>
      <div class=""player-list""><div class=""player-list-title"">PLAYERS (<span id=""playerCount"">0/{maxP}</span>)</div><div id=""playerList""></div></div>
      <div class=""lobby-actions""><button id=""quickPlayBtn2"" class=""btn btn-secondary"">⚡ Quick Play</button><button id=""readyBtn"" class=""btn btn-primary"">🚀 Ready</button></div>
    </div>
    <div class=""chat-box"">
      <div style=""padding:9px 13px;border-bottom:1px solid var(--br);font-size:.78rem;color:var(--dim);font-family:var(--fd);"">CHAT</div>
      <div class=""chat-messages"" id=""chatMessages""></div>
      <div class=""chat-input-row""><input type=""text"" id=""chatInput"" class=""chat-input"" placeholder=""Nhắn tin..."" maxlength=""200""><button id=""chatSendBtn"" class=""btn btn-primary"" style=""padding:7px 11px;"">➤</button></div>
    </div>
  </div>
</div>";

    // ══════════════════════════════════════
    //  PUBLIC PAGES
    // ══════════════════════════════════════
    public static string Index => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no"">
<title>🎮 Multiplayer Games Hub</title><style>{CSS}</style></head><body>
<header class=""hub-header"">
  <a href=""/"" class=""hub-logo"">GAME<span>HUB</span></a>
  <div id=""playerSetup"" style=""display:flex;align-items:center;gap:9px;flex-wrap:wrap;"">
    <div id=""nickForm"" style=""display:flex;gap:7px;align-items:center;flex-wrap:wrap;"">
      <input type=""text"" id=""nicknameInput"" class=""input-field"" placeholder=""Nhập nickname..."" maxlength=""20"" style=""width:140px;"">
      <input type=""color"" id=""colorPicker"" value=""#00ff88"" style=""width:34px;height:34px;border:none;border-radius:50%;cursor:pointer;background:none;"">
      <button id=""joinBtn"" class=""btn btn-primary"">▶ Vào chơi</button>
    </div>
    <div id=""playerInfo"" style=""display:none;align-items:center;gap:9px;"">
      <div class=""player-avatar"" id=""headerAvatar"" style=""width:34px;height:34px;font-size:.85rem;""></div>
      <span id=""headerName"" style=""font-weight:600;font-family:var(--fd);font-size:.83rem;""></span>
      <span id=""headerWins"" style=""font-size:.82rem;color:var(--ac4);font-weight:600;""></span>
      <button id=""changeNickBtn"" class=""btn btn-ghost"" style=""font-size:.72rem;"">✏️</button>
    </div>
  </div>
</header>
<div style=""text-align:center;padding:36px 18px 8px;position:relative;z-index:1;"">
  <h1 style=""font-size:clamp(1.4rem,4vw,2.3rem);color:var(--ac);text-shadow:var(--glow);margin-bottom:7px;"">MULTIPLAYER GAMES HUB</h1>
  <p style=""color:var(--dim);font-size:.95rem;"">5 games • Chơi ngay không cần đăng ký • Real-time</p>
</div>
<main class=""games-grid"">
  <a href=""/tictactoe"" class=""game-card"" style=""--cc:#00ff88;""><span class=""game-icon"">⬜</span><div class=""game-title"">CỜ CARO</div><div class=""game-desc"">2 người • X vs O • 15×15 • 5 liên tiếp thắng</div><div class=""game-online""><span class=""online-dot""></span><span id=""online-tictactoe"">0</span> người đang chơi</div></a>
  <a href=""/snake"" class=""game-card"" style=""--cc:#ffaa00;""><span class=""game-icon"">🐍</span><div class=""game-title"">SNAKE BATTLE</div><div class=""game-desc"">2-4 người • Ăn mồi • Last standing</div><div class=""game-online""><span class=""online-dot"" style=""background:var(--ac4);box-shadow:0 0 6px var(--ac4)""></span><span id=""online-snake"">0</span> người đang chơi</div></a>
  <a href=""/pong"" class=""game-card"" style=""--cc:#4488ff;""><span class=""game-icon"">🏓</span><div class=""game-title"">PONG</div><div class=""game-desc"">2 người • Paddle • First to 5</div><div class=""game-online""><span class=""online-dot"" style=""background:var(--ac3);box-shadow:0 0 6px var(--ac3)""></span><span id=""online-pong"">0</span> người đang chơi</div></a>
  <a href=""/chess"" class=""game-card"" style=""--cc:#cc44ff;""><span class=""game-icon"">♟️</span><div class=""game-title"">CHESS</div><div class=""game-desc"">2 players • Chess • Checkmate</div><div class=""game-online""><span class=""online-dot"" style=""background:#cc44ff;box-shadow:0 0 6px #cc44ff""></span><span id=""online-chess"">0</span> playing</div></a>
  <a href=""/mathquiz"" class=""game-card"" style=""--cc:#ff4466;""><span class=""game-icon"">🧮</span><div class=""game-title"">QUICK MATH</div><div class=""game-desc"">2-4 người • Trả lời nhanh • +điểm</div><div class=""game-online""><span class=""online-dot"" style=""background:var(--ac2);box-shadow:0 0 6px var(--ac2)""></span><span id=""online-mathquiz"">0</span> người đang chơi</div></a>
  <a href=""/poker"" class=""game-card"" style=""--cc:#ff9944;""><span class=""game-icon"">🃏</span><div class=""game-title"">POKER</div><div class=""game-desc"">2 người • Texas Hold'em • Chips</div><div class=""game-online""><span class=""online-dot"" style=""background:#ff9944;box-shadow:0 0 6px #ff9944""></span><span id=""online-poker"">0</span> người đang chơi</div></a>
  <a href=""/wordchain"" class=""game-card"" style=""--cc:#44ddff;""><span class=""game-icon"">🔤</span><div class=""game-title"">NỐI TỪ</div><div class=""game-desc"">2–8 người • Tiếng Việt • Nối từ liên tiếp</div><div class=""game-online""><span class=""online-dot"" style=""background:#44ddff;box-shadow:0 0 6px #44ddff""></span><span id=""online-wordchain"">0</span> người đang chơi</div></a>
</main>
<footer style=""text-align:center;padding:26px;color:var(--dim);font-size:.78rem;position:relative;z-index:1;"">Mở nhiều tab để chơi multiplayer • ASP.NET Core SignalR Real-time</footer>
{SignalRScripts}
<script>{SignalRClientJS}</script>
<script>
const saved=initPlayer();
const ni=document.getElementById('nicknameInput'),cp=document.getElementById('colorPicker');
if(saved.nickname){{ni.value=saved.nickname;cp.value=saved.color;}}
ni.addEventListener('keydown',e=>{{if(e.key==='Enter')document.getElementById('joinBtn').click();}});
document.getElementById('joinBtn').addEventListener('click',()=>{{const n=ni.value.trim();if(!n){{showToast('Vui lòng nhập nickname!','error');ni.focus();return;}}registerPlayer(n,cp.value);document.getElementById('nickForm').style.display='none';document.getElementById('playerInfo').style.display='flex';}});
document.getElementById('changeNickBtn').addEventListener('click',()=>{{document.getElementById('playerInfo').style.display='none';document.getElementById('nickForm').style.display='flex';ni.focus();}});
connection.onreconnected(()=>{{const s=initPlayer();if(s.nickname){{registerPlayer(s.nickname,s.color);document.getElementById('nickForm').style.display='none';document.getElementById('playerInfo').style.display='flex';}}}});
socket.on('stats:online',counts=>{{document.getElementById('online-tictactoe').textContent=counts.tictactoe||0;document.getElementById('online-snake').textContent=counts.snake||0;document.getElementById('online-pong').textContent=counts.pong||0;document.getElementById('online-chess').textContent=counts.chess||0;document.getElementById('online-mathquiz').textContent=counts.mathquiz||0;const wc=document.getElementById('online-wordchain');if(wc)wc.textContent=counts.wordchain||0;}});
document.querySelectorAll('.game-card').forEach(c=>c.addEventListener('click',e=>{{if(!localStorage.getItem('playerNickname')){{e.preventDefault();showToast('Vui lòng nhập nickname trước!','error');ni.focus();}}}}));
</script></body></html>";

    public static string TicTacToe => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no""><title>Cờ Caro | GameHub</title><style>{CSS}</style></head><body>
{Header}
{JoinPanel("⬜", "CỜ CARO", "var(--ac)", "tictactoe", "Bảng 15×15 • Đánh X hoặc O • 5 ô liên tiếp là thắng")}
{LobbyPanel("⬜", "CỜ CARO", "var(--ac)", 2)}
<div id=""gameArea"" class=""hidden"">
  <div class=""game-header""><div class=""scoreboard"" id=""scoreboard""></div><div id=""turnIndicator"" class=""turn-indicator""></div></div>
  <div class=""ttt-board"" id=""tttBoard""></div>
</div>
{GameOverOverlay}
{BaseScripts(TttJS, "tictactoe")}
</body></html>";

    public static string Snake => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no""><title>Snake Battle | GameHub</title><style>{CSS}</style></head><body>
{Header}
{JoinPanel("🐍", "SNAKE BATTLE", "var(--ac4)", "snake", "WASD hoặc phím mũi tên • Ăn mồi • Last snake standing")}
{LobbyPanel("🐍", "SNAKE BATTLE", "var(--ac4)", 4)}
<div id=""gameArea"" class=""hidden"">
  <div class=""game-header""><div class=""scoreboard"" id=""scoreboard""></div><div id=""aliveCount"" style=""font-size:.83rem;color:var(--dim);""></div></div>
  <div class=""snake-layout""><div class=""canvas-wrapper""><canvas id=""snakeCanvas""></canvas></div></div>
</div>
{GameOverOverlay}
{BaseScripts(SnakeJS, "snake")}
</body></html>";

    public static string Pong => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no""><title>Pong | GameHub</title><style>{CSS}</style></head><body>
{Header}
{JoinPanel("🏓", "PONG", "var(--ac3)", "pong", "Trái: W/S | Phải: ↑/↓ | First to 5 wins")}
{LobbyPanel("🏓", "PONG", "var(--ac3)", 2)}
<div id=""gameArea"" class=""hidden pong-layout"">
  <div id=""pongHint"" style=""font-size:.82rem;font-family:var(--fd);""></div>
  <canvas id=""pongCanvas"" style=""max-width:100%;""></canvas>
  <div style=""font-size:.78rem;color:var(--dim);"">Bên trái: <kbd>W</kbd>/<kbd>S</kbd> | Bên phải: <kbd>↑</kbd>/<kbd>↓</kbd></div>
</div>
{GameOverOverlay}
{BaseScripts(PongJS, "pong")}
</body></html>";

        public static string Chess => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no""><title>Chess | GameHub</title><style>{CSS}
.chess-info{{display:flex;flex-direction:column;gap:10px;min-width:180px;}}
.chess-timer-box{{background:var(--bg2);border:2px solid var(--br);border-radius:8px;padding:10px 14px;font-family:var(--fd);font-size:1.4rem;text-align:center;transition:all .3s;}}
.chess-timer-box.active{{border-color:var(--ac);color:var(--ac);box-shadow:var(--glow);}}
.chess-move{{font-family:var(--fd);font-size:.78rem;color:var(--dim);}}
.promo-dialog{{display:none;position:fixed;inset:0;background:rgba(0,0,0,.82);z-index:100;align-items:center;justify-content:center;gap:14px;flex-wrap:wrap;}}
.promo-btn{{background:var(--bg2);border:2px solid var(--br);border-radius:12px;padding:12px;cursor:pointer;transition:all .2s;width:88px;height:88px;display:flex;align-items:center;justify-content:center;}}
.promo-btn:hover{{border-color:var(--ac);transform:scale(1.08);box-shadow:var(--glow);}}
.promo-btn img{{width:60px;height:60px;}}
#chessBoardWrap{{border-radius:6px;overflow:hidden;box-shadow:0 8px 32px rgba(0,0,0,.55),0 2px 8px rgba(0,0,0,.4);}}
</style></head><body>
{Header}
{JoinPanel("♟️", "CHESS", "#cc44ff", "chess", "Move pieces • Checkmate to win • 10 min/player")}
{LobbyPanel("♟️", "CHESS", "#cc44ff", 2)}
<div id=""gameArea"" class=""hidden"">
  <div class=""game-header"">
    <div><div id=""myColorLabel"" style=""font-weight:700;color:#cc44ff;""></div><div id=""chessTurn"" class=""turn-indicator""></div><div id=""chessStatusMsg"" style=""color:var(--ac2);font-weight:700;font-size:.9rem;""></div></div>
    <button id=""resignBtn"" class=""btn btn-danger"" style=""padding:8px 14px;font-size:.82rem;"">🏳 Resign</button>
  </div>
  <div style=""display:flex;gap:14px;flex-wrap:wrap;justify-content:center;padding:10px 0;align-items:flex-start;"">
    <div style=""display:flex;flex-direction:column;align-items:center;"">
      <div style=""font-size:.75rem;color:var(--dim);margin-bottom:5px;align-self:flex-start;"" id=""oppTimeLabel"">Opponent</div>
      <div class=""chess-timer-box"" id=""oppTime"" style=""width:min(560px,92vw);text-align:center;"">10:00</div>
      <div id=""chessBoardWrap"" style=""width:min(560px,92vw);margin:6px 0;""></div>
      <div style=""font-size:.75rem;color:var(--dim);margin-bottom:5px;align-self:flex-start;"">You</div>
      <div class=""chess-timer-box active"" id=""myTime"" style=""width:min(560px,92vw);text-align:center;"">10:00</div>
    </div>
    <div class=""chess-info"">
      <div style=""background:var(--bg2);border:1px solid var(--br);border-radius:8px;padding:12px;"">
        <div style=""font-size:.73rem;color:var(--dim);margin-bottom:8px;font-family:var(--fd);"">MOVE HISTORY</div>
        <div id=""moveHistory"" style=""max-height:300px;overflow-y:auto;word-break:break-all;line-height:1.8;""></div>
      </div>
    </div>
  </div>
</div>
<div class=""promo-dialog"" id=""promoDialog"">
  <button class=""promo-btn"" id=""promoQ""></button>
  <button class=""promo-btn"" id=""promoR""></button>
  <button class=""promo-btn"" id=""promoB""></button>
  <button class=""promo-btn"" id=""promoN""></button>
</div>
{GameOverOverlay}
{BaseScripts(ChessJS, "chess")}
</body></html>";

    public static string MathQuiz => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no""><title>Quick Math | GameHub</title><style>{CSS}</style></head><body>
{Header}
{JoinPanel("🧮", "QUICK MATH", "var(--ac2)", "mathquiz", "Trả lời nhanh nhất • Đúng đầu tiên +3 điểm • 10 câu/ván")}
{LobbyPanel("🧮", "QUICK MATH", "var(--ac2)", 4)}
<div id=""gameArea"" class=""hidden"">
  <div class=""game-header"">
    <div class=""scoreboard"" id=""scoreboard""></div>
    <div style=""display:flex;align-items:center;gap:10px;"">
      <span id=""questionProgress"" style=""font-family:var(--fd);font-size:.83rem;color:var(--dim);""></span>
      <div class=""timer-box"" id=""timerDisplay"">8</div>
    </div>
  </div>
  <div class=""math-container"">
    <div class=""progress-bar""><div class=""progress-fill"" id=""timerBar"" style=""width:100%;""></div></div>
    <div id=""mathQuestion"" class=""math-question"">?</div>
    <div class=""math-answer-area"">
      <input type=""number"" id=""mathInput"" class=""math-input"" placeholder=""?"" autocomplete=""off"" inputmode=""numeric"">
      <button id=""submitBtn"" class=""btn btn-primary"" style=""padding:13px 22px;font-size:1.2rem;"">✓</button>
    </div>
    <div id=""answerFeedback"" style=""text-align:center;font-family:var(--fd);font-size:1.1rem;min-height:38px;""></div>
    <div id=""playersStatus"" style=""display:flex;flex-wrap:wrap;gap:9px;justify-content:center;margin-top:14px;""></div>
  </div>
</div>
{GameOverOverlay}
{BaseScripts(MathJS, "mathquiz")}
</body></html>";


// ══════════════════════════════════════════════════════════════
//  POKER PAGE
// ══════════════════════════════════════════════════════════════
    private const string PokerJS = @"
let pokerState=null, myId=null, actionLocked=false;

socket.on('player:joined', ({player}) => { myId = player.id; });

socket.on('game:start', ({gameType}) => {
  if(gameType !== 'poker') return;
  document.getElementById('lobbyArea')?.classList.add('hidden');
  document.getElementById('gameArea')?.classList.remove('hidden');
  document.getElementById('joinArea')?.classList.add('hidden');
});

socket.on('poker:state', gs => {
  pokerState = gs;
  actionLocked = false;
  renderPoker(gs);
});

const SUIT_COLOR = {'♥':'#ff4d6d','♦':'#ff4d6d','♠':'#c8d8ff','♣':'#7fffcf'};
const SUIT_GLOW  = {'♥':'#ff4d6d','♦':'#ff4d6d','♠':'#4488ff','♣':'#00cc88'};
const PHASE_NAMES = {preflop:'PRE-FLOP',flop:'FLOP',turn:'TURN',river:'RIVER',showdown:'SHOWDOWN'};

function makeCard(c) {
  const d = document.createElement('div');
  if (!c || c.hidden) {
    d.className = 'pk-card pk-back';
    d.innerHTML = '<span style=""font-size:1.8rem;opacity:.25;"">🂠</span>';
    return d;
  }
  d.className = 'pk-card';
  const col = SUIT_COLOR[c.suit] || '#eee';
  const glow = SUIT_GLOW[c.suit] || '#aaa';
  d.style.cssText = 'color:'+col+';border-color:'+col+'50;box-shadow:0 0 14px '+glow+'40;background:linear-gradient(145deg,#1c1c2e,#12121f);';
  d.innerHTML = '<span style=""font-size:1rem;font-weight:900;line-height:1;"">'+c.rank+'</span><span style=""font-size:1.3rem;line-height:1;"">'+c.suit+'</span>';
  return d;
}
function makePH() {
  const d = document.createElement('div'); d.className = 'pk-card pk-ph'; return d;
}

function renderPoker(gs) {
  if (!gs) return;

  // HUD
  const potEl = document.getElementById('pkPot'); if(potEl) potEl.textContent = gs.pot.toLocaleString()+' chips';
  const phEl  = document.getElementById('pkPhase');
  if(phEl) { phEl.textContent = PHASE_NAMES[gs.phase]||gs.phase; phEl.className='pk-phase '+gs.phase; }
  const rnEl  = document.getElementById('pkRound'); if(rnEl) rnEl.textContent = 'VÁN '+gs.round;
  const bbEl  = document.getElementById('pkBlinds'); if(bbEl) bbEl.textContent = 'B:'+gs.bigBlind+'/S:'+gs.smallBlind;

  // Community cards
  const cc = document.getElementById('pkCommunity');
  if (cc) {
    cc.innerHTML = '';
    (gs.communityCards||[]).forEach(c => { const card=makeCard(c); card.classList.add('pk-flip'); cc.appendChild(card); });
    for (let i=(gs.communityCards||[]).length; i<5; i++) cc.appendChild(makePH());
  }

  // Players
  const area = document.getElementById('pkPlayers');
  if (area) {
    area.innerHTML = '';
    gs.players.forEach((p, i) => {
      const isMe  = (p.id === myId);
      const isCur = (i === gs.currentPlayerIndex && !gs.gameOver && !p.folded && !p.allIn);
      const seat  = document.createElement('div');
      seat.className = 'pk-seat' + (isMe?' pk-me':'') + (isCur?' pk-active':'') + (p.folded?' pk-folded':'');
      if (isCur) seat.style.cssText = 'border-color:#ffd700;box-shadow:0 0 30px #ffd70055;';
      else if (isMe) seat.style.cssText = 'border-color:#ff9944;box-shadow:0 0 20px #ff994425;';

      if (i === gs.dealerIndex) { const db=document.createElement('span'); db.className='pk-dealer'; db.textContent='D'; seat.appendChild(db); }
      if (isCur) { const tr=document.createElement('div'); tr.className='pk-turn-ring'; seat.appendChild(tr); }

      // Top row: avatar + name + chips
      const top = document.createElement('div');
      top.style.cssText = 'display:flex;align-items:center;gap:8px;margin-bottom:8px;';
      const av = document.createElement('div'); av.className='pk-avatar';
      av.style.cssText = 'background:linear-gradient(135deg,'+p.color+'55,'+p.color+'22);border-color:'+p.color+';';
      av.textContent = p.nickname.charAt(0).toUpperCase();
      const info = document.createElement('div'); info.style.flex='1;min-width:0;';
      const nm = document.createElement('div'); nm.className='pk-nm'+(isMe?' pk-nm-me':'');
      nm.textContent = p.nickname + (isMe?' ★':'');
      nm.style.cssText='overflow:hidden;text-overflow:ellipsis;white-space:nowrap;font-size:.8rem;';
      const ch = document.createElement('div'); ch.className='pk-chips';
      ch.innerHTML = '🪙 <strong>'+p.chips.toLocaleString()+'</strong>';
      info.appendChild(nm); info.appendChild(ch);
      top.appendChild(av); top.appendChild(info); seat.appendChild(top);

      // Cards
      const hand = document.createElement('div'); hand.className='pk-hand';
      if (Array.isArray(p.hand) && p.hand.length > 0) {
        p.hand.forEach(c => hand.appendChild(makeCard(c)));
      } else { hand.appendChild(makePH()); hand.appendChild(makePH()); }
      seat.appendChild(hand);

      // Badges
      const badges = document.createElement('div'); badges.className='pk-badges';
      if (p.currentBet>0 && !p.folded) { const b=document.createElement('span'); b.className='pk-bet'; b.textContent='🪙 '+p.currentBet.toLocaleString(); badges.appendChild(b); }
      if (p.folded) { const b=document.createElement('span'); b.className='pk-tag pk-fold-tag'; b.textContent='FOLD'; badges.appendChild(b); }
      if (p.allIn)  { const b=document.createElement('span'); b.className='pk-tag pk-allin-tag'; b.textContent='ALL-IN'; badges.appendChild(b); }
      if (isCur)    { const b=document.createElement('span'); b.className='pk-tag pk-cur-tag'; b.textContent='▶ LƯỢT'; badges.appendChild(b); }
      seat.appendChild(badges);
      area.appendChild(seat);
    });
  }

  // Action log
  const logEl = document.getElementById('pkLog');
  if (logEl && gs.actionLog) {
    logEl.innerHTML = '';
    [...gs.actionLog].reverse().slice(0,12).forEach(l => {
      const d=document.createElement('div'); d.className='pk-log'; d.textContent=l; logEl.appendChild(d);
    });
  }

  // --- ACTION BUTTONS ---
  // Find my player object
  const me = gs.players.find(p => p.id === myId);
  const curPlayer = gs.players[gs.currentPlayerIndex];
  const isMyTurn = me && !me.folded && !me.allIn && curPlayer && curPlayer.id === myId && !gs.gameOver;

  console.log('[Poker] myId='+myId+' curId='+(curPlayer&&curPlayer.id)+' isMyTurn='+isMyTurn);

  const ab = document.getElementById('pkActions');
  if (ab) {
    ab.style.display = isMyTurn ? 'flex' : 'none';
  }

  if (isMyTurn && me) {
    const canCheck = (gs.currentBet <= (me.currentBet||0));
    const callAmt  = gs.currentBet - (me.currentBet||0);

    const chkBtn = document.getElementById('btnCheck');
    const callBtn = document.getElementById('btnCall');
    if (chkBtn)  chkBtn.style.display  = canCheck ? 'flex' : 'none';
    if (callBtn) {
      callBtn.style.display = canCheck ? 'none' : 'flex';
      const sub = callBtn.querySelector('.pk-sub');
      if (sub) sub.textContent = callAmt + ' chips';
    }

    const minR = gs.minRaise || gs.bigBlind || 20;
    const maxR = me.chips;
    const ri   = document.getElementById('raiseInput');
    const rs   = document.getElementById('raiseSlider');
    const rd   = document.getElementById('raiseDisplay');
    if (ri) { ri.min=minR; ri.max=maxR; ri.value=minR; }
    if (rs) { rs.min=minR; rs.max=maxR; rs.value=minR; }
    if (rd) rd.textContent = minR.toLocaleString();
  }
}

function doAction(a, amt) {
  if (actionLocked) { console.log('[Poker] Locked, ignoring action'); return; }
  actionLocked = true;
  console.log('[Poker] Emit action:', a, amt);
  socket.emit('poker:action', {action: a, amount: parseInt(amt)||0});
  // Visually disable buttons
  document.querySelectorAll('.pk-btn').forEach(b => b.style.opacity='0.5');
  setTimeout(() => {
    actionLocked = false;
    document.querySelectorAll('.pk-btn').forEach(b => b.style.opacity='1');
  }, 2000);
}

document.addEventListener('DOMContentLoaded', () => {
  document.getElementById('btnFold')?.addEventListener('click',  () => doAction('fold', 0));
  document.getElementById('btnCheck')?.addEventListener('click', () => doAction('check', 0));
  document.getElementById('btnCall')?.addEventListener('click',  () => doAction('call', 0));
  document.getElementById('btnRaise')?.addEventListener('click', () => {
    const v = document.getElementById('raiseInput')?.value || 20;
    doAction('raise', parseInt(v)||20);
  });
  document.getElementById('btnAllIn')?.addEventListener('click', () => doAction('allin', 0));

  // Slider sync
  const rs = document.getElementById('raiseSlider');
  const ri = document.getElementById('raiseInput');
  const rd = document.getElementById('raiseDisplay');
  rs?.addEventListener('input', () => { if(ri) ri.value=rs.value; if(rd) rd.textContent=parseInt(rs.value).toLocaleString(); });
  ri?.addEventListener('input', () => { if(rs) rs.value=ri.value; if(rd) rd.textContent=parseInt(ri.value).toLocaleString(); });
});
";
    public static string Poker => $@"<!DOCTYPE html><html lang=""vi""><head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no"">
<title>Texas Hold'em | GameHub</title>
<link href=""https://fonts.googleapis.com/css2?family=Cinzel:wght@700;900&family=Rajdhani:wght@400;500;600;700&display=swap"" rel=""stylesheet"">
<style>
{CSS}
:root{{--felt:#0a1f0e;--gold:#ffd700;}}
body{{background:radial-gradient(ellipse at 50% 30%,#0f2d14 0%,#060e08 60%,#030608 100%);font-family:'Rajdhani',sans-serif;}}
.pk-wrap{{max-width:1100px;margin:0 auto;padding:12px 10px;}}
.pk-hud{{display:flex;align-items:center;justify-content:space-around;padding:10px 20px;background:rgba(0,0,0,.55);border:1px solid #1a3a1a;border-radius:14px;margin-bottom:14px;flex-wrap:wrap;gap:8px;}}
.pk-hud-item{{display:flex;flex-direction:column;align-items:center;gap:1px;}}
.pk-hud-label{{font-size:.58rem;color:#3a6a3a;letter-spacing:.14em;font-family:'Cinzel',serif;}}
.pk-hud-val{{font-size:1rem;font-weight:700;color:var(--gold);font-family:'Cinzel',serif;text-shadow:0 0 10px #ffd70060;}}
.pk-phase{{font-size:.85rem;padding:3px 14px;border-radius:20px;font-family:'Cinzel',serif;font-weight:700;}}
.pk-phase.preflop{{background:#ffd70015;color:#ffd700;border:1px solid #ffd70040;}}
.pk-phase.flop{{background:#44aaff15;color:#44aaff;border:1px solid #44aaff40;}}
.pk-phase.turn{{background:#cc88ff15;color:#cc88ff;border:1px solid #cc88ff40;}}
.pk-phase.river{{background:#ff666615;color:#ff8888;border:1px solid #ff666640;}}
.pk-phase.showdown{{background:#ff994415;color:#ff9944;border:1px solid #ff994440;box-shadow:0 0 20px #ff994420;}}
.pk-felt{{background:radial-gradient(ellipse at 50% 40%,#1a4a20 0%,#0f2d14 50%,#091a0c 100%);border:3px solid #2a5a30;border-radius:24px;padding:22px 18px 16px;box-shadow:0 0 60px rgba(0,150,50,.15),inset 0 0 80px rgba(0,0,0,.4);position:relative;margin-bottom:12px;}}
.pk-community-label{{text-align:center;font-size:.6rem;color:rgba(255,255,255,.22);letter-spacing:.22em;font-family:'Cinzel',serif;margin-bottom:8px;}}
.pk-community{{display:flex;gap:9px;justify-content:center;align-items:center;margin-bottom:20px;flex-wrap:wrap;}}
.pk-players{{display:flex;gap:10px;justify-content:center;flex-wrap:wrap;}}
/* CARD */
.pk-card{{display:inline-flex;flex-direction:column;align-items:center;justify-content:center;width:52px;height:74px;border-radius:8px;border:1.5px solid #444;font-weight:900;background:linear-gradient(145deg,#1c1c2e,#12121f);gap:1px;transition:transform .15s;}}
.pk-card.pk-back{{background:repeating-linear-gradient(45deg,#0d1a2a,#0d1a2a 4px,#0a1520 4px,#0a1520 8px);border-color:#1a2a3a;color:#1e3a5a;}}
.pk-card.pk-ph{{background:transparent;border:2px dashed rgba(255,255,255,.07);box-shadow:none;}}
.pk-flip{{animation:pkFlip .35s ease both;}}
@keyframes pkFlip{{from{{transform:rotateY(-90deg);opacity:0;}}to{{transform:rotateY(0deg);opacity:1;}}}}
/* SEAT */
.pk-seat{{background:linear-gradient(145deg,rgba(10,22,12,.95),rgba(6,12,8,.98));border:2px solid #1e3a20;border-radius:14px;padding:11px 13px;min-width:150px;max-width:185px;flex:1;position:relative;transition:all .25s;}}
.pk-seat.pk-me{{border-color:#ff9944;}}
.pk-seat.pk-active{{animation:seatPulse 1.4s ease infinite;}}
@keyframes seatPulse{{0%,100%{{box-shadow:0 0 22px #ffd70040;}}50%{{box-shadow:0 0 42px #ffd70075;}}}}
.pk-seat.pk-folded{{opacity:.3;filter:grayscale(.9);}}
.pk-dealer{{position:absolute;top:-9px;right:-9px;background:var(--gold);color:#000;font-size:.62rem;font-weight:900;width:20px;height:20px;border-radius:50%;display:flex;align-items:center;justify-content:center;font-family:'Cinzel',serif;box-shadow:0 0 10px #ffd70080;}}
.pk-turn-ring{{position:absolute;inset:-4px;border-radius:17px;border:2px solid #ffd700;animation:turnRing .9s ease infinite;pointer-events:none;}}
@keyframes turnRing{{0%,100%{{opacity:1;}}50%{{opacity:.2;}}}}
.pk-avatar{{width:38px;height:38px;border-radius:50%;border:2px solid #444;display:flex;align-items:center;justify-content:center;font-size:1rem;font-weight:800;font-family:'Cinzel',serif;flex-shrink:0;}}
.pk-nm{{font-size:.78rem;font-weight:700;color:#b8b8c8;}}
.pk-nm.pk-nm-me{{color:#ff9944;}}
.pk-chips{{font-size:.72rem;color:#6a8a6a;margin-top:1px;}}
.pk-chips strong{{color:#88c888;}}
.pk-hand{{display:flex;gap:5px;justify-content:center;margin:8px 0 6px;}}
.pk-badges{{display:flex;gap:4px;flex-wrap:wrap;justify-content:center;min-height:18px;}}
.pk-bet{{font-size:.68rem;color:var(--gold);background:#ffd70015;border:1px solid #ffd70030;border-radius:9px;padding:1px 7px;}}
.pk-tag{{font-size:.62rem;font-weight:800;padding:2px 7px;border-radius:9px;letter-spacing:.06em;}}
.pk-fold-tag{{background:#ff446620;color:#ff4466;border:1px solid #ff446640;}}
.pk-allin-tag{{background:#ff994420;color:#ff9944;border:1px solid #ff994440;animation:blink .8s steps(1) infinite;}}
.pk-cur-tag{{background:#ffd70020;color:#ffd700;border:1px solid #ffd70040;}}
@keyframes blink{{50%{{opacity:.35;}}}}
/* ACTION ZONE */
.pk-action-zone{{background:linear-gradient(180deg,rgba(5,15,8,.92),rgba(3,10,5,.96));border:1px solid #1a3a1a;border-radius:16px;padding:16px 18px;}}
.pk-action-row{{display:flex;gap:8px;justify-content:center;flex-wrap:wrap;align-items:center;}}
.pk-btn{{display:flex;flex-direction:column;align-items:center;justify-content:center;gap:2px;border:none;border-radius:11px;padding:11px 18px;min-width:82px;cursor:pointer;font-family:'Rajdhani',sans-serif;font-weight:700;font-size:.9rem;transition:all .12s;user-select:none;}}
.pk-btn:hover{{transform:translateY(-2px);filter:brightness(1.15);}}
.pk-btn:active{{transform:scale(.95);}}
.pk-sub{{font-size:.67rem;font-weight:500;opacity:.8;}}
.pk-btn-fold{{background:linear-gradient(145deg,#3a0a14,#2a0810);border:1.5px solid #ff446650;color:#ff6680;}}
.pk-btn-check{{background:linear-gradient(145deg,#0a3a1e,#062a14);border:1.5px solid #44dd8850;color:#55ee99;}}
.pk-btn-call{{background:linear-gradient(145deg,#0a2040,#061428);border:1.5px solid #44aaff50;color:#55bbff;}}
.pk-btn-raise{{background:linear-gradient(145deg,#2a1a40,#1a0e2a);border:1.5px solid #cc88ff50;color:#dd99ff;}}
.pk-btn-allin{{background:linear-gradient(145deg,#3a1800,#281000);border:1.5px solid #ff662250;color:#ff9955;}}
.pk-raise-wrap{{display:flex;align-items:center;gap:8px;background:rgba(0,0,0,.3);border:1px solid #1e3a1e;border-radius:11px;padding:9px 13px;flex-wrap:wrap;justify-content:center;width:100%;margin-top:8px;}}
.pk-raise-val{{font-family:'Cinzel',serif;color:var(--gold);font-weight:700;font-size:1rem;min-width:70px;text-align:center;}}
input[type=range].pk-slider{{-webkit-appearance:none;height:5px;border-radius:3px;background:linear-gradient(to right,#cc88ff,#ff6622);outline:none;flex:1;min-width:100px;}}
input[type=range].pk-slider::-webkit-slider-thumb{{-webkit-appearance:none;width:16px;height:16px;border-radius:50%;background:var(--gold);box-shadow:0 0 8px #ffd70080;cursor:pointer;}}
input[type=number].pk-num{{background:#0a100a;border:1px solid #2a3a2a;color:#90c890;border-radius:7px;padding:5px 8px;width:72px;font-family:'Rajdhani',sans-serif;font-size:.88rem;text-align:center;}}
.pk-log-wrap{{margin-top:12px;background:rgba(0,0,0,.4);border:1px solid #0e2010;border-radius:10px;padding:8px 12px;max-height:90px;overflow-y:auto;}}
.pk-log{{font-size:.72rem;color:#4a6a4a;padding:2px 0;border-bottom:1px solid rgba(255,255,255,.03);}}
.pk-log:first-child{{color:#8aaa8a;font-weight:600;}}
</style>
</head><body>
{Header}
{JoinPanel("🃏", "TEXAS HOLD'EM", "#ff9944", "poker", "1 vs 4 Bot • 1000 chips • Raise/Call/Fold")}
{LobbyPanel("🃏", "TEXAS HOLD'EM", "#ff9944", 6)}
<div id=""gameArea"" class=""hidden"">
  <div class=""pk-wrap"">
    <div class=""pk-hud"">
      <div class=""pk-hud-item""><span class=""pk-hud-label"">VÁN</span><span class=""pk-hud-val"" id=""pkRound"">1</span></div>
      <div class=""pk-hud-item""><span class=""pk-hud-label"">PHASE</span><span id=""pkPhase"" class=""pk-phase preflop"">PRE-FLOP</span></div>
      <div class=""pk-hud-item""><span class=""pk-hud-label"">POT</span><span class=""pk-hud-val"" id=""pkPot"">0 chips</span></div>
      <div class=""pk-hud-item""><span class=""pk-hud-label"">BLIND</span><span class=""pk-hud-val"" id=""pkBlinds"">B:20/S:10</span></div>
    </div>
    <div class=""pk-felt"">
      <div class=""pk-community-label"">COMMUNITY CARDS</div>
      <div class=""pk-community"" id=""pkCommunity""></div>
      <div class=""pk-players"" id=""pkPlayers""></div>
    </div>
    <div class=""pk-action-zone"">
      <div id=""pkActions"" style=""display:none;flex-direction:column;gap:8px;"">
        <div class=""pk-action-row"">
          <button id=""btnFold""  class=""pk-btn pk-btn-fold"" >🗑 BỎ BÀI<span class=""pk-sub"">Fold</span></button>
          <button id=""btnCheck"" class=""pk-btn pk-btn-check"">✋ CHECK<span class=""pk-sub"">0 chips</span></button>
          <button id=""btnCall""  class=""pk-btn pk-btn-call"" >📞 GỌI<span class=""pk-sub pk-sub-call"">0 chips</span></button>
          <button id=""btnRaise"" class=""pk-btn pk-btn-raise"">📈 NÂNG<span class=""pk-sub"">chips</span></button>
          <button id=""btnAllIn"" class=""pk-btn pk-btn-allin"">🔥 ALL-IN<span class=""pk-sub"">tất cả</span></button>
        </div>
        <div class=""pk-raise-wrap"">
          <span style=""font-size:.7rem;color:#5a7a5a;"">RAISE:</span>
          <input type=""range""   id=""raiseSlider"" class=""pk-slider"" min=""20"" max=""1000"" value=""20"">
          <span class=""pk-raise-val"" id=""raiseDisplay"">20</span>
          <input type=""number"" id=""raiseInput"" class=""pk-num"" min=""20"" step=""10"" value=""20"">
          <span style=""font-size:.7rem;color:#5a7a5a;"">chips</span>
        </div>
      </div>
      <div class=""pk-log-wrap"" id=""pkLog""></div>
    </div>
  </div>
</div>
{GameOverOverlay}
{BaseScripts(PokerJS, "poker")}
</body></html>";

    // ══════════════════════════════════════════
    //  WORD CHAIN PAGE
    // ══════════════════════════════════════════
    private const string WordChainJS = @"
const WORD_CHAIN_VERSION = 2;
let wcState=null;
function wcInit(gs){ wcState=gs; wcRender(gs); }
function wcRender(gs){
  wcState=gs;
  const pb=document.getElementById('wcPlayers');
  if(pb)pb.innerHTML=gs.players.map(p=>{
    const isMe=p.id===window.currentPlayer?.id;
    const isTurn=p.id===gs.currentTurn;
    return '<div class=""wc-player'+(p.isEliminated?' eliminated':'')+(isTurn?' active-turn':'')+'"" style=""border-color:'+(isTurn?'var(--ac)':'rgba(255,255,255,.1)')+'"">'+
      '<div class=""player-avatar"" style=""background:'+(p.color||'#888')+'"">'+p.nickname.charAt(0).toUpperCase()+'</div>'+
      '<div><div class=""score-name"">'+(isMe?'👤 ':'')+(p.isEliminated?'💀 ':'')+p.nickname+'</div>'+
      '<div class=""score-value"" style=""color:var(--ac)"">'+p.score+' điểm</div></div>'+
      (isTurn&&!p.isEliminated?'<span class=""wc-turn-badge"">⏳ Đang đánh</span>':'')+
      '</div>';
  }).join('');
  const cl=document.getElementById('wcLog');
  if(cl){
    cl.innerHTML=gs.chatLog.map(l=>{
      const cls=l.startsWith('✅')?'log-ok':l.startsWith('❌')?'log-err':l.startsWith('💀')?'log-dead':l.startsWith('🏆')?'log-win':l.startsWith('⚡')?'log-warn':'';
      return '<div class=""wc-log-line '+cls+'"">'+l+'</div>';
    }).join('');
    cl.scrollTop=cl.scrollHeight;
  }
  const ti=document.getElementById('turnIndicator');
  if(ti){
    if(gs.gameOver){ti.innerHTML='🏁 Trò chơi kết thúc!';ti.style.color='var(--ac)';}
    else if(gs.currentTurn===window.currentPlayer?.id){
      ti.innerHTML='✍️ Lượt của bạn! Nhập từ bắt đầu bằng: <strong style=""color:var(--ac);font-size:1.3em"">'+gs.lastSyllable+'</strong>';
      ti.style.color='var(--ac)';
      document.getElementById('wcInput')?.focus();
    } else {
      const cur=gs.players.find(p=>p.id===gs.currentTurn&&!p.isEliminated);
      const who = cur ? cur.nickname : '?';
      const isBot = who.includes('Bot') || who.includes('🤖');
      ti.innerHTML=(isBot?'🤖 ':'⏳ ')+'Lượt của <b>'+who+'</b> — Cần từ bắt đầu: <strong style=""color:var(--ac2)"">'+gs.lastSyllable+'</strong>';
      ti.style.color='var(--dim)';
    }
  }
  const lw=document.getElementById('wcLastWord');
  if(lw&&gs.lastWord)lw.innerHTML='Từ vừa đánh: <span style=""color:var(--ac);font-size:1.3em;font-weight:800"">'+gs.lastWord+'</span>';
  const inp=document.getElementById('wcInput');
  if(inp&&gs.lastSyllable){
    inp.placeholder=gs.lastSyllable+' ...';
    const isMyTurn=gs.currentTurn===window.currentPlayer?.id&&!gs.gameOver;
    inp.disabled=!isMyTurn;
    document.getElementById('wcSendBtn').disabled=!isMyTurn;
  }
}
function wcSubmit(){
  const inp=document.getElementById('wcInput');
  const word=(inp?.value||'').trim();
  if(!word){showToast('Hãy nhập một từ!','error');return;}
  if(wcState&&wcState.currentTurn!==window.currentPlayer?.id){showToast('Chưa đến lượt của bạn!','error');return;}
  socket.emit('wordchain:submit',{word});
  if(inp)inp.value='';
}
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{
  if(gameType!=='wordchain')return;
  document.getElementById('lobbyArea').classList.add('hidden');
  document.getElementById('gameArea').classList.remove('hidden');
  document.getElementById('joinArea').classList.add('hidden');
  wcInit(gameState);
  showToast('Trò chơi bắt đầu! Quy tắc: từ ghép 2+ tiếng, nối tiếng cuối','info');
});
socket.on('wordchain:updated',(data)=>{ wcRender(data.gameState||data); });
socket.on('wordchain:timer',({remaining,currentTurn})=>{
  const tb=document.getElementById('wcTimer');
  if(!tb)return;
  tb.textContent=remaining+'s';
  tb.style.color=remaining<=5?'var(--ac2)':remaining<=10?'#ffaa00':'var(--ac)';
  tb.style.transform=remaining<=5?'scale(1.15)':'scale(1)';
});
socket.on('game:error',(data)=>{ showToast(data.message||'Lỗi','error'); });
socket.on('game:over',({winnerId,winnerNickname,players,gameState})=>{
  if(gameState)wcRender(gameState);
  showGameOver(winnerId,winnerNickname,players);
});
socket.on('game:reset',()=>{
  document.getElementById('gameArea').classList.add('hidden');
  document.getElementById('lobbyArea').classList.remove('hidden');
  const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');
  wcState=null;
});
document.addEventListener('keydown',e=>{if(e.key==='Enter'&&document.activeElement?.id==='wcInput')wcSubmit();});";

    private static string WordChainCSS => @"
.wc-layout{display:grid;grid-template-columns:1fr;gap:12px;max-width:700px;margin:0 auto;padding:10px;}
.wc-players{display:flex;flex-wrap:wrap;gap:8px;justify-content:center;padding:4px 0;}
.wc-player{display:flex;align-items:center;gap:8px;background:var(--bg2);border:2px solid rgba(255,255,255,.1);border-radius:10px;padding:8px 12px;transition:all .3s;position:relative;min-width:140px;}
.wc-player.active-turn{background:rgba(0,255,136,.08);animation:wcGlow .9s ease-in-out infinite alternate;}
.wc-player.eliminated{opacity:.35;filter:grayscale(1);text-decoration:line-through;}
.wc-turn-badge{position:absolute;top:-9px;right:6px;background:var(--ac);color:#000;font-size:.6rem;font-weight:800;padding:2px 7px;border-radius:99px;white-space:nowrap;}
@keyframes wcGlow{from{box-shadow:0 0 4px var(--ac);}to{box-shadow:0 0 20px var(--ac),0 0 4px var(--ac);}}
.wc-center{text-align:center;padding:8px;}
.wc-last-word{font-size:.95rem;color:var(--dim);margin-bottom:4px;min-height:24px;}
#wcTimer{font-size:2.2rem;font-weight:900;display:block;margin:2px 0;transition:transform .2s,color .3s;letter-spacing:1px;}
.wc-input-row{display:flex;gap:8px;max-width:500px;margin:8px auto 0;}
.wc-input{flex:1;background:var(--bg2);border:2px solid var(--br);border-radius:8px;color:var(--tx);padding:10px 14px;font-size:1rem;transition:border-color .2s;}
.wc-input:focus{outline:none;border-color:var(--ac);}
.wc-input:disabled{opacity:.4;cursor:not-allowed;}
.wc-btn{background:var(--ac);color:#000;border:none;border-radius:8px;padding:10px 18px;font-weight:800;cursor:pointer;font-size:1rem;transition:opacity .2s;}
.wc-btn:disabled{opacity:.35;cursor:not-allowed;}
.wc-log{background:var(--bg2);border-radius:10px;padding:10px 12px;max-height:200px;overflow-y:auto;font-size:.84rem;line-height:1.5;}
.wc-log-line{padding:3px 0;border-bottom:1px solid rgba(255,255,255,.04);}
.wc-log-line.log-ok{color:#88ffbb;}
.wc-log-line.log-err{color:#ff6677;}
.wc-log-line.log-dead{color:#ff4444;}
.wc-log-line.log-win{color:#ffd700;font-weight:700;}
.wc-log-line.log-warn{color:#ffaa00;}
.wc-rules{background:var(--bg2);border-radius:10px;padding:12px;font-size:.8rem;color:var(--dim);line-height:1.8;}
.wc-rules b{color:var(--tx);}";

    public static string WordChainPage => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no""><title>Nối Từ | GameHub</title><style>{CSS}{WordChainCSS}</style></head><body>
{Header}
{JoinPanel("🔤", "NỐI TỪ TIẾNG VIỆT", "var(--ac3)", "wordchain", "2–8 người • Tiếng Việt • Nối tiếp liên tục • Hết giờ bị loại")}
{LobbyPanel("🔤", "NỐI TỪ", "var(--ac3)", 8)}
<div id=""gameArea"" class=""hidden"">
  <div class=""game-header""><div class=""scoreboard"" id=""scoreboard"" style=""display:none""></div><div id=""turnIndicator"" class=""turn-indicator""></div></div>
  <div class=""wc-layout"">
    <div class=""wc-players"" id=""wcPlayers""></div>
    <div class=""wc-center"">
      <div class=""wc-last-word"" id=""wcLastWord"">Chờ trò chơi bắt đầu...</div>
      <span id=""wcTimer"" style=""color:var(--ac)"">--</span>
      <div class=""wc-input-row"">
        <input id=""wcInput"" class=""wc-input"" type=""text"" placeholder=""Nhập từ..."" autocomplete=""off"" autocorrect=""off"" spellcheck=""false"">
        <button id=""wcSendBtn"" class=""wc-btn"" onclick=""wcSubmit()"">Gửi ↵</button>
      </div>
    </div>
    <div class=""wc-log"" id=""wcLog""></div>
    <div class=""wc-rules"">
      <b>📋 Luật chơi:</b><br>
      • Từ phải bắt đầu bằng <b>tiếng cuối</b> của từ trước đó<br>
      • Phải là <b>từ ghép hoặc từ láy 2 tiếng</b> trở lên có nghĩa trong tiếng Việt<br>
      • <b>Không lặp lại</b> từ đã dùng trong ván<br>
      • Hết giờ → bị loại | Người cuối còn lại thắng<br>
      • Cứ 10 từ, thời gian giảm 5 giây (tối thiểu 10 giây)
    </div>
  </div>
</div>
{GameOverOverlay}
{BaseScripts(WordChainJS, "wordchain")}
</body></html>";

}