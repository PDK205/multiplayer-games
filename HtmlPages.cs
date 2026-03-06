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
.ttt-board{display:grid;grid-template-columns:repeat(3,1fr);gap:8px;max-width:340px;margin:20px auto;padding:18px}
.ttt-cell{aspect-ratio:1;background:var(--bg3);border:1px solid var(--br);border-radius:10px;cursor:pointer;font-family:var(--fd);font-size:2.8rem;font-weight:700;transition:all .2s;display:flex;align-items:center;justify-content:center;color:var(--tx)}
.ttt-cell:hover:not(.taken){background:var(--bg2);border-color:var(--ac)}
.ttt-cell.taken{cursor:not-allowed}
.ttt-cell.symbol-X{color:var(--ac)}
.ttt-cell.symbol-O{color:var(--ac2)}
.ttt-cell.winning{background:rgba(0,255,136,.1);border-color:var(--ac);animation:winPulse .5s ease 3}
.ttt-cell.winning.symbol-O{background:rgba(255,68,102,.1);border-color:var(--ac2)}
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
@media(max-width:480px){.hub-header{padding:12px 14px}.games-grid{grid-template-columns:1fr 1fr;padding:14px;gap:10px}.game-card{padding:14px 12px}.math-question{font-size:2rem}.room-code{font-size:1.1rem}}";

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
      'chess:resign': 'ChessResign'
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
let tttState=null;
function initBoard(){const b=document.getElementById('tttBoard');b.innerHTML='';for(let i=0;i<9;i++){const c=document.createElement('div');c.className='ttt-cell';c.dataset.index=i;c.addEventListener('click',()=>{if(tttState&&tttState.currentTurn===window.currentPlayer?.id)socket.emit('tictactoe:move',{cellIndex:i});else showToast('Chưa đến lượt bạn!','error');});b.appendChild(c);}}
function renderBoard(gs){tttState=gs;const cells=document.querySelectorAll('.ttt-cell');gs.board.forEach((v,i)=>{const c=cells[i];if(v){c.textContent=v;c.classList.add('taken','symbol-'+v);}else{c.textContent='';c.className='ttt-cell';}});if(gs.winLine)gs.winLine.forEach(i=>cells[i].classList.add('winning'));updateTurnInfo(gs);}
function updateTurnInfo(gs){const e=document.getElementById('turnIndicator');if(!e)return;if(gs.winner||gs.isDraw){e.innerHTML=gs.isDraw?'🤝 Hòa!':'';return;}const cp=gs.players.find(p=>p.id===gs.currentTurn);if(!cp)return;if(gs.currentTurn===window.currentPlayer?.id){e.innerHTML='Lượt của bạn <span>('+gs.players.find(p=>p.id===window.currentPlayer.id)?.symbol+')</span>';e.style.color='var(--ac)';}else{e.innerHTML='Lượt của <span>'+cp.nickname+'</span>';e.style.color='var(--dim)';}}
function updateTTTScore(gs){const e=document.getElementById('scoreboard');if(!e||!gs.players)return;e.innerHTML=gs.players.map(p=>'<div class=""score-item""><div class=""player-avatar"" style=""background:'+(p.color||'#888')+';width:28px;height:28px;font-size:.75rem;"">'+p.nickname.charAt(0).toUpperCase()+'</div><div><div class=""score-name"">'+p.nickname+' ('+p.symbol+')</div><div class=""score-value"" style=""color:'+(p.symbol==='X'?'var(--ac)':'var(--ac2)')+'"">'+(p.score||0)+'</div></div></div>').join('');}
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{if(gameType!=='tictactoe')return;document.getElementById('lobbyArea').classList.add('hidden');document.getElementById('gameArea').classList.remove('hidden');document.getElementById('joinArea').classList.add('hidden');initBoard();renderBoard(gameState);updateTTTScore(gameState);const my=gameState.players.find(p=>p.id===window.currentPlayer?.id);if(my)showToast('Bạn là '+my.symbol,'info');});
socket.on('tictactoe:updated',({gameState})=>{renderBoard(gameState);updateTTTScore(gameState);});
socket.on('game:over',({winnerId,winnerNickname,players,gameState})=>{if(gameState){renderBoard(gameState);updateTTTScore(gameState);}showGameOver(winnerId,winnerNickname,players);});
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');tttState=null;});";

    private const string SnakeJS = @"
let snakeCanvas,snakeCtx,snakeState=null;const GS=16;
function initSnakeCanvas(gridSize){snakeCanvas=document.getElementById('snakeCanvas');const sz=gridSize*GS;snakeCanvas.width=sz;snakeCanvas.height=sz;snakeCtx=snakeCanvas.getContext('2d');}
function renderSnake(gs){snakeState=gs;const ctx=snakeCtx,s=GS;ctx.fillStyle='#0d0d14';ctx.fillRect(0,0,snakeCanvas.width,snakeCanvas.height);ctx.strokeStyle='rgba(255,255,255,.03)';ctx.lineWidth=.5;for(let i=0;i<=gs.gridSize;i++){ctx.beginPath();ctx.moveTo(i*s,0);ctx.lineTo(i*s,snakeCanvas.height);ctx.stroke();ctx.beginPath();ctx.moveTo(0,i*s);ctx.lineTo(snakeCanvas.width,i*s);ctx.stroke();}const f=gs.food;ctx.fillStyle='#ff4466';ctx.shadowColor='#ff4466';ctx.shadowBlur=10;ctx.beginPath();ctx.arc(f.x*s+s/2,f.y*s+s/2,s/2-1,0,Math.PI*2);ctx.fill();ctx.shadowBlur=0;gs.snakes.forEach(sn=>{if(!sn.alive)ctx.globalAlpha=.2;sn.body.forEach((b,i)=>{const isH=i===0;ctx.fillStyle=sn.color;ctx.shadowColor=isH?sn.color:'transparent';ctx.shadowBlur=isH?8:0;const p=isH?1:2;ctx.fillRect(b.x*s+p,b.y*s+p,s-p*2,s-p*2);});ctx.shadowBlur=0;if(sn.body[0]){ctx.fillStyle=sn.color;ctx.font='bold 9px monospace';ctx.textAlign='center';ctx.fillText(sn.nickname.substring(0,6),sn.body[0].x*s+s/2,sn.body[0].y*s-2);}ctx.globalAlpha=1;});updateSnakeScore(gs);}
function updateSnakeScore(gs){const e=document.getElementById('scoreboard');if(!e)return;e.innerHTML=gs.players.map(p=>'<div class=""score-item""><div style=""width:12px;height:12px;border-radius:2px;background:'+p.color+';box-shadow:0 0 6px '+p.color+';""></div><div><div class=""score-name"" style=""'+(p.alive?'':'text-decoration:line-through;opacity:.5;')+'"">'+(p.nickname||'')+'</div><div class=""score-value"" style=""color:'+p.color+'"">'+(p.score||0)+'</div></div></div>').join('');const ae=document.getElementById('aliveCount');if(ae)ae.textContent=gs.players.filter(p=>p.alive).length+' rắn còn sống';}
const DK={'ArrowUp':'UP','w':'UP','W':'UP','ArrowDown':'DOWN','s':'DOWN','S':'DOWN','ArrowLeft':'LEFT','a':'LEFT','A':'LEFT','ArrowRight':'RIGHT','d':'RIGHT','D':'RIGHT'};
document.addEventListener('keydown',e=>{const d=DK[e.key];if(d&&snakeState&&!snakeState.gameOver){e.preventDefault();socket.emit('snake:direction',{direction:d});}});
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{if(gameType!=='snake')return;document.getElementById('lobbyArea').classList.add('hidden');document.getElementById('gameArea').classList.remove('hidden');document.getElementById('joinArea').classList.add('hidden');initSnakeCanvas(gameState.gridSize);renderSnake(gameState);showToast('🐍 WASD để di chuyển','info',3000);});
socket.on('snake:tick',gs=>renderSnake(gs));
socket.on('game:over',({winnerId,winnerNickname,players})=>showGameOver(winnerId,winnerNickname,players));
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');snakeState=null;});";

    private const string PongJS = @"
let pongCanvas,pongCtx,pongState=null;const keysDown=new Set();
function initPongCanvas(w,h){pongCanvas=document.getElementById('pongCanvas');pongCanvas.width=w;pongCanvas.height=h;pongCtx=pongCanvas.getContext('2d');}
function renderPong(gs){pongState=gs;const ctx=pongCtx,W=pongCanvas.width,H=pongCanvas.height;ctx.fillStyle='#080810';ctx.fillRect(0,0,W,H);ctx.setLineDash([8,8]);ctx.strokeStyle='rgba(255,255,255,.12)';ctx.lineWidth=2;ctx.beginPath();ctx.moveTo(W/2,0);ctx.lineTo(W/2,H);ctx.stroke();ctx.setLineDash([]);Object.entries(gs.paddles).forEach(([id,p])=>{const me=id===window.currentPlayer?.id;ctx.fillStyle=me?'#00ff88':'#ff4466';ctx.shadowColor=me?'#00ff88':'#ff4466';ctx.shadowBlur=10;ctx.fillRect(p.x,p.y,p.w,p.h);ctx.shadowBlur=0;ctx.fillStyle=me?'#00ff88':'#ff4466';ctx.font='bold 11px monospace';ctx.textAlign='center';const nx=p.side==='left'?p.x+p.w+30:p.x-30;ctx.fillText(p.nickname.substring(0,8),nx,p.y+p.h/2+4);});const b=gs.ball;ctx.fillStyle='#fff';ctx.shadowColor='#fff';ctx.shadowBlur=12;ctx.fillRect(b.x,b.y,b.size,b.size);ctx.shadowBlur=0;if(gs.players){ctx.font='bold 46px monospace';ctx.fillStyle='rgba(255,255,255,.5)';ctx.textAlign='center';const lp=gs.players.find(p=>p.side==='left'),rp=gs.players.find(p=>p.side==='right');if(lp)ctx.fillText(lp.score,W/4,68);if(rp)ctx.fillText(rp.score,W*3/4,68);}}
document.addEventListener('keydown',e=>{if(!pongState||pongState.gameOver)return;if(['w','W','s','S','ArrowUp','ArrowDown'].includes(e.key))e.preventDefault();if(!keysDown.has(e.key)){keysDown.add(e.key);updatePaddle();}});
document.addEventListener('keyup',e=>{keysDown.delete(e.key);updatePaddle();});
function updatePaddle(){const id=window.currentPlayer?.id;if(!id||!pongState?.paddles[id])return;const side=pongState.paddles[id]?.side;let dir=null;if(side==='left'){if(keysDown.has('w')||keysDown.has('W'))dir='up';else if(keysDown.has('s')||keysDown.has('S'))dir='down';}else{if(keysDown.has('ArrowUp'))dir='up';else if(keysDown.has('ArrowDown'))dir='down';}socket.emit('pong:paddle',{direction:dir});}
(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{if(gameType!=='pong')return;document.getElementById('lobbyArea').classList.add('hidden');document.getElementById('gameArea').classList.remove('hidden');document.getElementById('joinArea').classList.add('hidden');initPongCanvas(gameState.width,gameState.height);const myId=window.currentPlayer?.id;if(myId&&gameState.paddles[myId]){const side=gameState.paddles[myId].side,h=document.getElementById('pongHint');if(h){h.textContent=side==='left'?'🟢 Bạn: bên TRÁI | W/S':'🔴 Bạn: bên PHẢI | ↑/↓';h.style.color=side==='left'?'var(--ac)':'var(--ac2)';}}renderPong(gameState);});
socket.on('pong:tick',gs=>renderPong(gs));
socket.on('game:over',({winnerId,winnerNickname,players})=>showGameOver(winnerId,winnerNickname,players));
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');pongState=null;keysDown.clear();});";

    private const string ChessJS = @"
// ── Chess Canvas Renderer ─────────────────────────────────────
let chessState=null,myColor=null,selectedSq=null,legalFromServer={},highlightedSqs=new Set(),lastMove=null;
let chessCanvas=null,chessCtx=null,CELL=0;

// SVG-based piece drawing using Path2D on canvas
// Pieces: wK wQ wR wB wN wP  bK bQ bR bB bN bP
const PIECE_UNICODE={'wK':'♔','wQ':'♕','wR':'♖','wB':'♗','wN':'♘','wP':'♙','bK':'♚','bQ':'♛','bR':'♜','bB':'♝','bN':'♞','bP':'♟'};

function initChessCanvas(){
  const wrap=document.getElementById('chessBoardWrap');
  if(!wrap)return;
  wrap.innerHTML='';
  chessCanvas=document.createElement('canvas');
  chessCanvas.style.cssText='display:block;cursor:pointer;border-radius:4px;box-shadow:0 8px 32px rgba(0,0,0,.6);max-width:100%;';
  wrap.appendChild(chessCanvas);
  chessCtx=chessCanvas.getContext('2d');
  resizeChessCanvas();
  chessCanvas.addEventListener('click',onCanvasClick);
  window.addEventListener('resize',()=>{resizeChessCanvas();if(chessState)drawBoard(chessState);});
}

function resizeChessCanvas(){
  const wrap=document.getElementById('chessBoardWrap');
  if(!wrap||!chessCanvas)return;
  const size=Math.min(wrap.clientWidth||560,560,window.innerWidth*0.9);
  CELL=Math.floor(size/8);
  const total=CELL*8;
  chessCanvas.width=total;
  chessCanvas.height=total;
  chessCanvas.style.width=total+'px';
  chessCanvas.style.height=total+'px';
}

function onCanvasClick(e){
  if(!chessState||chessState.gameOver)return;
  const rect=chessCanvas.getBoundingClientRect();
  const scaleX=chessCanvas.width/rect.width;
  const scaleY=chessCanvas.height/rect.height;
  const x=Math.floor((e.clientX-rect.left)*scaleX/CELL);
  const y=Math.floor((e.clientY-rect.top)*scaleY/CELL);
  if(x<0||x>7||y<0||y>7)return;
  const sq=y*8+x;
  onSquareClick(sq);
}

function drawBoard(gs){
  chessState=gs;
  if(!chessCtx||!chessCanvas)return;
  const ctx=chessCtx,C=CELL;
  // Draw squares
  for(let r=0;r<8;r++){
    for(let c=0;c<8;c++){
      const idx=r*8+c;
      const light=(r+c)%2===0;
      // Base color
      if(highlightedSqs.has(idx)&&idx===selectedSq) ctx.fillStyle='#7fc97f';
      else if(highlightedSqs.has(idx)) ctx.fillStyle='#7fc97f88';
      else if(lastMove&&(idx===lastMove.from||idx===lastMove.to)) ctx.fillStyle='#cdd16f';
      else ctx.fillStyle=light?'#f0d9b5':'#b58863';
      ctx.fillRect(c*C,r*C,C,C);
      // Check highlight
      if(gs.whiteInCheck&&gs.board[idx]==='wK') ctx.fillStyle='rgba(255,50,50,0.6)',ctx.fillRect(c*C,r*C,C,C);
      if(gs.blackInCheck&&gs.board[idx]==='bK') ctx.fillStyle='rgba(255,50,50,0.6)',ctx.fillRect(c*C,r*C,C,C);
      // Legal move dot
      if(highlightedSqs.has(idx)&&idx!==selectedSq){
        ctx.fillStyle='rgba(0,0,0,0.22)';
        ctx.beginPath();
        ctx.arc(c*C+C/2,r*C+C/2,C*0.17,0,Math.PI*2);
        ctx.fill();
      }
    }
  }
  // Selected square border
  if(selectedSq!==null){
    const sr=Math.floor(selectedSq/8),sc=selectedSq%8;
    ctx.strokeStyle='#3a9a3a';ctx.lineWidth=4;
    ctx.strokeRect(sc*C+2,sr*C+2,C-4,C-4);
  }
  // Draw pieces
  ctx.font='bold '+Math.floor(C*0.82)+'px serif';
  ctx.textAlign='center';
  ctx.textBaseline='middle';
  for(let r=0;r<8;r++){
    for(let c=0;c<8;c++){
      const idx=r*8+c;
      const piece=gs.board[idx];
      if(!piece)continue;
      const isWhite=piece[0]==='w';
      const cx=c*C+C/2, cy=r*C+C/2;
      const sym=PIECE_UNICODE[piece]||'?';
      // Shadow / outline first
      ctx.save();
      if(isWhite){
        // White piece: draw dark outline then white fill
        ctx.fillStyle='#333';
        ctx.shadowColor='rgba(0,0,0,0.8)';
        ctx.shadowBlur=3;
        // Draw outline by drawing multiple offsets
        const off=Math.max(2,Math.floor(C*0.04));
        for(let dx=-off;dx<=off;dx+=off){
          for(let dy=-off;dy<=off;dy+=off){
            if(dx===0&&dy===0)continue;
            ctx.fillText(sym,cx+dx,cy+dy);
          }
        }
        ctx.shadowBlur=0;
        ctx.fillStyle='#ffffff';
        ctx.fillText(sym,cx,cy);
      } else {
        // Black piece: draw light outline then dark fill
        ctx.fillStyle='#ddd';
        ctx.shadowColor='rgba(255,255,255,0.3)';
        ctx.shadowBlur=2;
        const off=Math.max(2,Math.floor(C*0.04));
        for(let dx=-off;dx<=off;dx+=off){
          for(let dy=-off;dy<=off;dy+=off){
            if(dx===0&&dy===0)continue;
            ctx.fillText(sym,cx+dx,cy+dy);
          }
        }
        ctx.shadowBlur=0;
        ctx.fillStyle='#111111';
        ctx.fillText(sym,cx,cy);
      }
      ctx.restore();
    }
  }
  updateChessStatus(gs);
}

// renderBoard is alias for drawBoard (called from event handlers)
function renderBoard(gs){drawBoard(gs);}
function initChessBoard(){initChessCanvas();}

function updateChessStatus(gs){
  const myId=window.currentPlayer?.id;
  const myPlayer=gs.players.find(p=>p.id===myId);
  const oppPlayer=gs.players.find(p=>p.id!==myId);
  const myTimeEl=document.getElementById('myTime');
  const oppTimeEl=document.getElementById('oppTime');
  const turnEl=document.getElementById('chessTurn');
  if(myTimeEl&&myPlayer)myTimeEl.textContent=fmtTime(myPlayer.side==='white'?gs.whiteTime:gs.blackTime);
  if(oppTimeEl&&oppPlayer)oppTimeEl.textContent=fmtTime(oppPlayer.side==='white'?gs.whiteTime:gs.blackTime);
  const isMyTurn=myPlayer&&gs.currentTurn===myPlayer.side;
  if(turnEl)turnEl.textContent=gs.gameOver?'Game over':(isMyTurn?'Your turn':'Waiting...');
  if(turnEl)turnEl.style.color=isMyTurn?'var(--ac)':'var(--dim)';
  const hist=document.getElementById('moveHistory');
  if(hist&&gs.moveHistory){hist.innerHTML='';gs.moveHistory.forEach((m,i)=>{const d=document.createElement('span');d.className='chess-move';d.textContent=(i%2===0?(Math.floor(i/2)+1)+'. ':' ')+m+' ';hist.appendChild(d);});hist.scrollTop=hist.scrollHeight;}
}

function fmtTime(s){const m=Math.floor(s/60);const sc=s%60;return m+':'+(sc<10?'0':'')+sc;}

function onSquareClick(sq){
  if(!chessState||chessState.gameOver)return;
  const myId=window.currentPlayer?.id;
  const myPlayer=chessState.players.find(p=>p.id===myId);
  if(!myPlayer||chessState.currentTurn!==myPlayer.side)return;
  if(selectedSq!==null&&highlightedSqs.has(sq)&&sq!==selectedSq){
    const movingPiece=chessState.board[selectedSq];
    const isPromo=movingPiece&&movingPiece[1]==='P'&&(sq<8||sq>=56);
    if(isPromo){showPromoDialog(selectedSq,sq);}
    else{socket.emit('chess:move',{from:selectedSq,to:sq,promotion:null});clearSelection();}
    return;
  }
  const piece=chessState.board[sq];
  if(piece&&piece.startsWith(myPlayer.side[0])){
    selectedSq=sq;
    highlightedSqs=new Set([sq]);
    const moves=legalFromServer[sq];
    if(moves){moves.forEach(m=>highlightedSqs.add(m));}
    else{socket.emit('chess:getLegalMoves',{square:sq});}
    drawBoard(chessState);
  } else {
    clearSelection();
    drawBoard(chessState);
  }
}

function clearSelection(){selectedSq=null;highlightedSqs=new Set();}

function showPromoDialog(from,to){
  const d=document.getElementById('promoDialog');
  if(!d)return;
  d.style.display='flex';
  const side=chessState.board[from][0];
  const PROMO_SYMS={'Q':side==='w'?'♕':'♛','R':side==='w'?'♖':'♜','B':side==='w'?'♗':'♝','N':side==='w'?'♘':'♞'};
  ['Q','R','B','N'].forEach(p=>{
    const btn=document.getElementById('promo'+p);
    if(btn){btn.textContent=PROMO_SYMS[p];btn.onclick=()=>{d.style.display='none';socket.emit('chess:move',{from,to,promotion:p});clearSelection();};}
  });
}

socket.on('chess:legalMoves',({moves})=>{
  legalFromServer={};
  if(moves)Object.keys(moves).forEach(k=>{legalFromServer[parseInt(k)]=moves[k];});
  if(selectedSq!==null&&legalFromServer[selectedSq]){
    highlightedSqs=new Set([selectedSq]);
    legalFromServer[selectedSq].forEach(m=>highlightedSqs.add(m));
    if(chessState)drawBoard(chessState);
  }
});

socket.on('chess:squareMoves',({square,moves})=>{
  legalFromServer[square]=moves;
  if(selectedSq===square){
    highlightedSqs=new Set([square]);
    moves.forEach(m=>highlightedSqs.add(m));
    if(chessState)drawBoard(chessState);
  }
});

socket.on('chess:updated',({gameState,lastMove:lm,notation,isCheck,isCheckmate})=>{
  lastMove=lm;legalFromServer={};clearSelection();
  chessState=gameState;
  drawBoard(gameState);
  const statusMsg=document.getElementById('chessStatusMsg');
  if(statusMsg){
    if(isCheckmate)statusMsg.textContent='Checkmate!';
    else if(isCheck)statusMsg.textContent='Check!';
    else if(notation==='resign')statusMsg.textContent='Opponent resigned!';
    else if(notation==='timeout')statusMsg.textContent='Time\'s up!';
    else statusMsg.textContent='';
  }
});

socket.on('chess:timer',({whiteTime,blackTime,currentTurn})=>{
  if(!chessState)return;
  chessState.whiteTime=whiteTime;chessState.blackTime=blackTime;chessState.currentTurn=currentTurn;
  updateChessStatus(chessState);
});

(window._gameStartHandlers=window._gameStartHandlers||[]).push(({gameType,gameState})=>{
  if(gameType!=='chess')return;
  chessState=gameState;
  const myId=window.currentPlayer?.id;
  const myPlayer=gameState.players.find(p=>p.id===myId);
  myColor=myPlayer?myPlayer.side:null;
  document.getElementById('lobbyArea').classList.add('hidden');
  document.getElementById('gameArea').classList.remove('hidden');
  document.getElementById('joinArea').classList.add('hidden');
  const myColorEl=document.getElementById('myColorLabel');
  if(myColorEl)myColorEl.textContent=myColor==='white'?'You: White (moves first)':'You: Black';
  initChessCanvas();
  drawBoard(gameState);
});

socket.on('game:over',({winnerId,winnerNickname,players})=>showGameOver(winnerId,winnerNickname,players));
socket.on('game:reset',()=>{document.getElementById('gameArea').classList.add('hidden');document.getElementById('lobbyArea').classList.remove('hidden');const o=document.getElementById('gameOverOverlay');if(o)o.classList.add('hidden');chessState=null;myColor=null;selectedSq=null;legalFromServer={};highlightedSqs=new Set();lastMove=null;chessCanvas=null;chessCtx=null;});
document.getElementById('resignBtn')?.addEventListener('click',()=>{if(confirm('Are you sure you want to resign?'))socket.emit('chess:resign',{});});
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
    public static string Index => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1"">
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
  <a href=""/tictactoe"" class=""game-card"" style=""--cc:#00ff88;""><span class=""game-icon"">⭕</span><div class=""game-title"">TIC-TAC-TOE</div><div class=""game-desc"">2 người • X vs O • Cổ điển</div><div class=""game-online""><span class=""online-dot""></span><span id=""online-tictactoe"">0</span> người đang chơi</div></a>
  <a href=""/snake"" class=""game-card"" style=""--cc:#ffaa00;""><span class=""game-icon"">🐍</span><div class=""game-title"">SNAKE BATTLE</div><div class=""game-desc"">2-4 người • Ăn mồi • Last standing</div><div class=""game-online""><span class=""online-dot"" style=""background:var(--ac4);box-shadow:0 0 6px var(--ac4)""></span><span id=""online-snake"">0</span> người đang chơi</div></a>
  <a href=""/pong"" class=""game-card"" style=""--cc:#4488ff;""><span class=""game-icon"">🏓</span><div class=""game-title"">PONG</div><div class=""game-desc"">2 người • Paddle • First to 5</div><div class=""game-online""><span class=""online-dot"" style=""background:var(--ac3);box-shadow:0 0 6px var(--ac3)""></span><span id=""online-pong"">0</span> người đang chơi</div></a>
  <a href=""/chess"" class=""game-card"" style=""--cc:#cc44ff;""><span class=""game-icon"">♟️</span><div class=""game-title"">CHESS</div><div class=""game-desc"">2 players • Chess • Checkmate</div><div class=""game-online""><span class=""online-dot"" style=""background:#cc44ff;box-shadow:0 0 6px #cc44ff""></span><span id=""online-chess"">0</span> playing</div></a>
  <a href=""/mathquiz"" class=""game-card"" style=""--cc:#ff4466;""><span class=""game-icon"">🧮</span><div class=""game-title"">QUICK MATH</div><div class=""game-desc"">2-4 người • Trả lời nhanh • +điểm</div><div class=""game-online""><span class=""online-dot"" style=""background:var(--ac2);box-shadow:0 0 6px var(--ac2)""></span><span id=""online-mathquiz"">0</span> người đang chơi</div></a>
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
socket.on('stats:online',counts=>{{document.getElementById('online-tictactoe').textContent=counts.tictactoe||0;document.getElementById('online-snake').textContent=counts.snake||0;document.getElementById('online-pong').textContent=counts.pong||0;document.getElementById('online-chess').textContent=counts.chess||0;document.getElementById('online-mathquiz').textContent=counts.mathquiz||0;}});
document.querySelectorAll('.game-card').forEach(c=>c.addEventListener('click',e=>{{if(!localStorage.getItem('playerNickname')){{e.preventDefault();showToast('Vui lòng nhập nickname trước!','error');ni.focus();}}}}));
</script></body></html>";

    public static string TicTacToe => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><title>Tic-Tac-Toe | GameHub</title><style>{CSS}</style></head><body>
{Header}
{JoinPanel("⭕", "TIC-TAC-TOE", "var(--ac)", "tictactoe", "Click ô trống để đánh • 3 liên tiếp thắng")}
{LobbyPanel("⭕", "TIC-TAC-TOE", "var(--ac)", 2)}
<div id=""gameArea"" class=""hidden"">
  <div class=""game-header""><div class=""scoreboard"" id=""scoreboard""></div><div id=""turnIndicator"" class=""turn-indicator""></div></div>
  <div class=""ttt-board"" id=""tttBoard""></div>
</div>
{GameOverOverlay}
{BaseScripts(TttJS, "tictactoe")}
</body></html>";

    public static string Snake => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><title>Snake Battle | GameHub</title><style>{CSS}</style></head><body>
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

    public static string Pong => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><title>Pong | GameHub</title><style>{CSS}</style></head><body>
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

        public static string Chess => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><title>Chess | GameHub</title><style>{CSS}
.chess-info{{display:flex;flex-direction:column;gap:10px;min-width:180px;}}
.chess-timer-box{{background:var(--bg2);border:1px solid var(--br);border-radius:8px;padding:10px 14px;font-family:var(--fd);font-size:1.4rem;text-align:center;}}
.chess-timer-box.active{{border-color:var(--ac);color:var(--ac);box-shadow:var(--glow);}}
.chess-move{{font-family:var(--fd);font-size:.78rem;color:var(--dim);}}
.promo-dialog{{display:none;position:fixed;inset:0;background:rgba(0,0,0,.7);z-index:100;align-items:center;justify-content:center;gap:14px;}}
.promo-btn{{background:var(--bg2);border:2px solid var(--br);border-radius:10px;padding:16px;font-size:2.5rem;cursor:pointer;}}
.promo-btn:hover{{border-color:var(--ac);}}
</style></head><body>
{Header}
{JoinPanel("♟️", "CHESS", "#cc44ff", "chess", "Move pieces • Checkmate to win • 10 min/player")}
{LobbyPanel("♟️", "CHESS", "#cc44ff", 2)}
<div id=""gameArea"" class=""hidden"">
  <div class=""game-header"">
    <div><div id=""myColorLabel"" style=""font-weight:700;color:#cc44ff;""></div><div id=""chessTurn"" class=""turn-indicator""></div><div id=""chessStatusMsg"" style=""color:var(--ac2);font-weight:700;font-size:.9rem;""></div></div>
    <button id=""resignBtn"" class=""btn btn-danger"" style=""padding:6px 14px;font-size:.82rem;"">🏳 Bo cuoc</button>
  </div>
  <div style=""display:flex;gap:18px;flex-wrap:wrap;justify-content:center;padding:10px 0;"">
    <div>
      <div style=""font-size:.75rem;color:var(--dim);margin-bottom:5px;"" id=""oppTimeLabel"">Doi thu</div>
      <div class=""chess-timer-box"" id=""oppTime"">10:00</div>
      <div id=""chessBoardWrap"" style=""width:min(560px,90vw);margin:8px 0;""></div>
      <div style=""font-size:.75rem;color:var(--dim);margin-bottom:5px;"">Ban</div>
      <div class=""chess-timer-box active"" id=""myTime"">10:00</div>
    </div>
    <div class=""chess-info"">
      <div style=""background:var(--bg2);border:1px solid var(--br);border-radius:8px;padding:12px;"">
        <div style=""font-size:.73rem;color:var(--dim);margin-bottom:8px;font-family:var(--fd);"">LICH SU NUOC DI</div>
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

    public static string MathQuiz => $@"<!DOCTYPE html><html lang=""vi""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><title>Quick Math | GameHub</title><style>{CSS}</style></head><body>
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
}