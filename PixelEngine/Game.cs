using System.Numerics;
using Newtonsoft.Json;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using Raylib_CsLo;

namespace PixelEngine;

public class Game
{
    public Config? config;

    public void Start()
    {
        InitWindow(config.Size * config.Scale, config.Size * config.Scale, config.WindowTitle);
        SetTargetFPS(config.TargetFps);
    }

    public void HandleInput()
    {
        
        

    }

    public void UpdateSim()
    {
        
    }

    public void UpdateDraw()
    {
        BeginDrawing();
        ClearBackground(BLACK);
        
        
        DrawText("FPS: " + GetFPS(), 10, 10, 10, WHITE);
        
        
    
        EndDrawing();
    }
}