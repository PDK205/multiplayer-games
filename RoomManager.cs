using GameHub.Models;

namespace GameHub;

public class RoomManager
{
    private readonly Dictionary<string, Room> _rooms = new();
    private readonly Dictionary<string, Player> _players = new();
    private readonly Dictionary<string, (int count, long resetAt)> _rateLimits = new();
    private static readonly Random Rng = new();
    private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public static readonly Dictionary<string, GameConfig> GameConfigs = new()
    {
        ["tictactoe"] = new() { MaxPlayers = 2, MinPlayers = 2 },
        ["snake"]     = new() { MaxPlayers = 4, MinPlayers = 2 },
        ["pong"]      = new() { MaxPlayers = 2, MinPlayers = 2 },
        ["chess"]     = new() { MaxPlayers = 2, MinPlayers = 2 },
        ["mathquiz"]  = new() { MaxPlayers = 4, MinPlayers = 2 }
    };

    // ── Room code generation ──────────────────────────────────
    public string GenerateRoomCode()
    {
        string code;
        do { code = new string(Enumerable.Range(0, 6).Select(_ => Chars[Rng.Next(Chars.Length)]).ToArray()); }
        while (_rooms.ContainsKey(code));
        return code;
    }

    // ── Player management ─────────────────────────────────────
    public Player CreatePlayer(string socketId, string nickname, string color)
    {
        var p = new Player { Id = socketId, Nickname = nickname[..Math.Min(nickname.Length, 20)], Color = color };
        _players[socketId] = p;
        return p;
    }

    public Player? GetPlayer(string socketId) => _players.GetValueOrDefault(socketId);

    public Player? RemovePlayer(string socketId)
    {
        _players.TryGetValue(socketId, out var p);
        if (p?.RoomCode != null) LeaveRoom(socketId, p.RoomCode);
        _players.Remove(socketId);
        _rateLimits.Remove(socketId);
        return p;
    }

    // ── Room management ───────────────────────────────────────
    public Room CreateRoom(string gameType, int maxPlayers, bool isPublic = true)
    {
        var code = GenerateRoomCode();
        var room = new Room { Code = code, GameType = gameType, MaxPlayers = maxPlayers, IsPublic = isPublic };
        _rooms[code] = room;
        return room;
    }

    public Room? GetRoom(string code) => _rooms.GetValueOrDefault(code);

    public Room? GetRoomByPlayer(string socketId)
    {
        var p = GetPlayer(socketId);
        return p?.RoomCode != null ? GetRoom(p.RoomCode) : null;
    }

    public (string? error, Room? room) JoinRoom(string socketId, string roomCode)
    {
        var room = _rooms.GetValueOrDefault(roomCode);
        var player = _players.GetValueOrDefault(socketId);
        if (room == null) return ("Room không tồn tại", null);
        if (player == null) return ("Player không hợp lệ", null);
        if (room.State == "PLAYING") return ("Game đang diễn ra", null);
        if (room.Players.Count >= room.MaxPlayers) return ("Room đã đầy", null);
        if (room.Players.Any(p => p.Id == socketId)) return ("Bạn đã trong room này", null);
        if (player.RoomCode != null) LeaveRoom(socketId, player.RoomCode);

        room.Players.Add(new RoomPlayer { Id = socketId, Nickname = player.Nickname, Color = player.Color });
        player.RoomCode = roomCode;
        return (null, room);
    }

    public Room? LeaveRoom(string socketId, string roomCode)
    {
        var room = _rooms.GetValueOrDefault(roomCode);
        if (room == null) return null;
        room.Players.RemoveAll(p => p.Id == socketId);
        room.ReadyPlayers.Remove(socketId);
        var player = _players.GetValueOrDefault(socketId);
        if (player != null) player.RoomCode = null;
        if (room.Players.Count == 0) _rooms.Remove(roomCode);
        return room;
    }

    public Room QuickPlay(string gameType, int maxPlayers)
    {
        var found = _rooms.Values.FirstOrDefault(r =>
            r.GameType == gameType && r.IsPublic && r.State == "WAITING" && r.Players.Count < r.MaxPlayers);
        return found ?? CreateRoom(gameType, maxPlayers, true);
    }

    public Dictionary<string, int> GetOnlineCountByGame()
    {
        var counts = new Dictionary<string, int> { ["tictactoe"]=0,["snake"]=0,["pong"]=0,["chess"]=0,["mathquiz"]=0 };
        foreach (var room in _rooms.Values)
            if ((room.State == "PLAYING" || room.State == "WAITING") && counts.ContainsKey(room.GameType))
                counts[room.GameType] += room.Players.Count;
        return counts;
    }

    // ── Rate limiting ─────────────────────────────────────────
    public bool CheckRateLimit(string socketId)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var limit = _rateLimits.GetValueOrDefault(socketId, (count: 0, resetAt: now + 1000));
        if (now > limit.resetAt) limit = (count: 0, resetAt: now + 1000);
        limit.count++;
        _rateLimits[socketId] = limit;
        return limit.count <= 10;
    }

    public static string SanitizeMessage(string msg) =>
        System.Web.HttpUtility.HtmlEncode(msg.Length > 200 ? msg[..200] : msg);

    // ── Cleanup ────────────────────────────────────────────────
    public void Cleanup()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var toRemove = _rooms.Where(kv =>
            (kv.Value.Players.Count == 0 && now - kv.Value.CreatedAt > 5 * 60 * 1000) ||
            (kv.Value.State == "FINISHED" && now - (kv.Value.FinishedAt ?? 0) > 2 * 60 * 1000)
        ).Select(kv => kv.Key).ToList();
        foreach (var code in toRemove) _rooms.Remove(code);
    }

    public object GetRoomInfo(Room room) => new
    {
        code = room.Code, gameType = room.GameType, state = room.State,
        players = room.Players, maxPlayers = room.MaxPlayers, readyCount = room.ReadyPlayers.Count
    };
}
