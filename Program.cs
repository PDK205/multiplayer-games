using GameHub;
using GameHub.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<RoomManager>();
builder.Services.AddSignalR();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();
app.UseCors();

// ── Cleanup timer ─────────────────────────────────────────────
var rm = app.Services.GetRequiredService<RoomManager>();
var timer = new Timer(_ => rm.Cleanup(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

// ── SignalR hub ───────────────────────────────────────────────
app.MapHub<GameHubSignalR>("/gamehub");

// ══════════════════════════════════════════════════════════════
//  STATIC HTML PAGES (giống hệt Node.js - nhúng thẳng vào)
// ══════════════════════════════════════════════════════════════
app.MapGet("/", () => Results.Content(HtmlPages.Index, "text/html"));
app.MapGet("/tictactoe", () => Results.Content(HtmlPages.TicTacToe, "text/html"));
app.MapGet("/snake", () => Results.Content(HtmlPages.Snake, "text/html"));
app.MapGet("/pong", () => Results.Content(HtmlPages.Pong, "text/html"));
app.MapGet("/chess", () => Results.Content(HtmlPages.Chess, "text/html"));
app.MapGet("/mathquiz", () => Results.Content(HtmlPages.MathQuiz, "text/html"));

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
app.Urls.Add($"http://0.0.0.0:{port}");

Console.WriteLine("");
Console.WriteLine("╔══════════════════════════════════════════════╗");
Console.WriteLine("║   🎮 MULTIPLAYER GAMES HUB (C# .NET 10)     ║");
Console.WriteLine("║                                              ║");
Console.WriteLine($"║   👉 Mở:  http://localhost:{port}             ║");
Console.WriteLine("║                                              ║");
Console.WriteLine("║   📱 Mở nhiều tabs để test multiplayer       ║");
Console.WriteLine("╚══════════════════════════════════════════════╝");
Console.WriteLine("");

app.Run();
