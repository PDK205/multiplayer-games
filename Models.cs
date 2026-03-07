namespace GameHub.Models;

// ══════════════════════════════════════════
//  PLAYER & ROOM MODELS
// ══════════════════════════════════════════
public class Player
{
    public string Id { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Color { get; set; } = "#00ff88";
    public int Wins { get; set; } = 0;
    public string? RoomCode { get; set; }
}

public class RoomPlayer
{
    public string Id { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Color { get; set; } = "#00ff88";
    public int Score { get; set; } = 0;
    public bool IsReady { get; set; } = false;
}

public class Room
{
    public string Code { get; set; } = "";
    public string GameType { get; set; } = "";
    public int MaxPlayers { get; set; }
    public bool IsPublic { get; set; } = true;
    public string State { get; set; } = "WAITING";
    public List<RoomPlayer> Players { get; set; } = new();
    public HashSet<string> ReadyPlayers { get; set; } = new();
    public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public long? FinishedAt { get; set; }
    public object? GameState { get; set; }
    public bool IsVsBot { get; set; } = false;
    public string BotDifficulty { get; set; } = "medium";
}

// ══════════════════════════════════════════
//  TIC-TAC-TOE MODELS
// ══════════════════════════════════════════
public class TttPlayer
{
    public string Id { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Color { get; set; } = "";
    public string Symbol { get; set; } = "";
    public int Score { get; set; } = 0;
}

public class TttGameState
{
    public string?[] Board { get; set; } = new string?[9];
    public string CurrentTurn { get; set; } = "";
    public List<TttPlayer> Players { get; set; } = new();
    public string? Winner { get; set; }
    public bool IsDraw { get; set; } = false;
    public int MoveCount { get; set; } = 0;
    public int[]? WinLine { get; set; }
}

// ══════════════════════════════════════════
//  SNAKE MODELS
// ══════════════════════════════════════════
public class Point { public int X { get; set; } public int Y { get; set; } }

public class SnakePlayer
{
    public string Id { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Color { get; set; } = "";
    public bool Alive { get; set; } = true;
    public int Score { get; set; } = 0;
    public List<Point> Body { get; set; } = new();
    public string Direction { get; set; } = "RIGHT";
    public string NextDirection { get; set; } = "RIGHT";
}

public class SnakeGameState
{
    public List<SnakePlayer> Snakes { get; set; } = new();
    public Point Food { get; set; } = new();
    public int GridSize { get; set; } = 30;
    public bool GameOver { get; set; } = false;
    public string? Winner { get; set; }
    public List<object> Players { get; set; } = new();
    public int Tick { get; set; } = 0;
}

// ══════════════════════════════════════════
//  PONG MODELS
// ══════════════════════════════════════════
public class PongBall
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Vx { get; set; }
    public double Vy { get; set; }
    public int Size { get; set; } = 10;
}

public class PongPaddle
{
    public double X { get; set; }
    public double Y { get; set; }
    public int W { get; set; } = 12;
    public int H { get; set; } = 80;
    public int Score { get; set; } = 0;
    public string Nickname { get; set; } = "";
    public string Side { get; set; } = "";
    public string? Moving { get; set; }
}

public class PongGameState
{
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 500;
    public PongBall Ball { get; set; } = new();
    public Dictionary<string, PongPaddle> Paddles { get; set; } = new();
    public List<object> Players { get; set; } = new();
    public bool GameOver { get; set; } = false;
    public string? Winner { get; set; }
    public int HitCount { get; set; } = 0;
}

// ══════════════════════════════════════════
//  CHESS MODELS
// ══════════════════════════════════════════
public class ChessPlayer
{
    public string Id { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Color { get; set; } = "";
    public string Side { get; set; } = ""; // "white" or "black"
}

public class ChessGameState
{
    // 8x8 board: null = empty, string = piece code e.g. "wK","bP","wR"
    public string?[] Board { get; set; } = new string?[64];
    public string CurrentTurn { get; set; } = "white"; // "white" or "black"
    public List<ChessPlayer> Players { get; set; } = new();
    public bool GameOver { get; set; } = false;
    public string? Winner { get; set; } // playerId
    public string? WinReason { get; set; } // "checkmate","resign","timeout"
    public bool WhiteInCheck { get; set; } = false;
    public bool BlackInCheck { get; set; } = false;
    // Castling rights
    public bool WhiteCanCastleK { get; set; } = true;
    public bool WhiteCanCastleQ { get; set; } = true;
    public bool BlackCanCastleK { get; set; } = true;
    public bool BlackCanCastleQ { get; set; } = true;
    // En passant target square (-1 = none)
    public int EnPassantTarget { get; set; } = -1;
    // Move history for display
    public List<string> MoveHistory { get; set; } = new();
    public int MoveCount { get; set; } = 0;
    // Timer (seconds remaining)
    public int WhiteTime { get; set; } = 600; // 10 minutes
    public int BlackTime { get; set; } = 600;
    public long LastMoveAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}

// ══════════════════════════════════════════
//  MATH QUIZ MODELS
// ══════════════════════════════════════════
public class MathPlayer
{
    public string Id { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Color { get; set; } = "";
    public int Score { get; set; } = 0;
    public bool Answered { get; set; } = false;
}

public class MathGameState
{
    public List<MathPlayer> Players { get; set; } = new();
    public string? CurrentQuestion { get; set; }
    public int QuestionIndex { get; set; } = 0;
    public int TotalQuestions { get; set; } = 10;
    public int TimeLeft { get; set; } = 8;
    public string Phase { get; set; } = "WAITING";
    public int? CorrectAnswer { get; set; }
    public string? FirstCorrect { get; set; }
    public bool GameOver { get; set; } = false;
    public string? Winner { get; set; }
}

public class MathQuestion
{
    public string Question { get; set; } = "";
    public int Answer { get; set; }
}

// ══════════════════════════════════════════
//  GAME CONFIG
// ══════════════════════════════════════════
public class GameConfig
{
    public int MaxPlayers { get; set; }
    public int MinPlayers { get; set; }
}

// ══════════════════════════════════════════
//  POKER MODELS
// ══════════════════════════════════════════
public class PokerCard { public string Suit{get;set;}=""; public string Rank{get;set;}=""; public int Value{get;set;} }

public class PokerPlayer
{
    public string Id{get;set;}=""; public string Nickname{get;set;}=""; public string Color{get;set;}="";
    public int Chips{get;set;}=1000; public List<PokerCard> Hand{get;set;}=new();
    public int CurrentBet{get;set;}=0; public int TotalBet{get;set;}=0; public bool Folded{get;set;}=false; public bool AllIn{get;set;}=false; public int Score{get;set;}=0;
}

public class PokerGameState
{
    public List<PokerPlayer> Players{get;set;}=new();
    public List<PokerCard> CommunityCards{get;set;}=new();
    public List<PokerCard> Deck{get;set;}=new();
    public int Pot{get;set;}=0; public int CurrentBet{get;set;}=0;
    public int SmallBlind{get;set;}=10; public int BigBlind{get;set;}=20;
    public int DealerIndex{get;set;}=0; public int CurrentPlayerIndex{get;set;}=0;
    public HashSet<int> NeedsToAct{get;set;}=new();
    public string Phase{get;set;}="preflop";
    public bool GameOver{get;set;}=false; public string? Winner{get;set;} public string? WinReason{get;set;}
    public int Round{get;set;}=1; public List<string> ActionLog{get;set;}=new(); public int MinRaise{get;set;}=20;
}