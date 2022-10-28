using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayMath;
using static Raylib_CsLo.RayGui;
using System.Drawing;
using Newtonsoft.Json;
using PixelEngine;
using Raylib_CsLo;

Game game = new Game();

game.config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
game.Start();



while (!WindowShouldClose())
{
    game.config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
    game.HandleInput();
    game.UpdateSim();
    game.UpdateDraw();
}