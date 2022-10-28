using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using Raylib_CsLo;

namespace PixelEngine;

public class Game
{
    private const int Size = 256;
    private const int Scale = 2;
    private const int Iter = 10;
    private const float Dt = 0.2f;
    private const float Diffusion = 0f;
    private const float Viscosity = 0.0000001f;
    
    private const string WindowTitle = "Fluid Simulation";
    private const int TargetFps = 60;

    private Fluid _fluid = null!;

    public Vector2 PMouse;
    
    public void Start()
    {
        _fluid = new Fluid(Dt, Diffusion, Viscosity, Size, Iter, Scale);

        InitWindow(Size * Scale, Size * Scale, WindowTitle);
        SetTargetFPS(TargetFps);
    }

    public void HandleInput()
    {
        
        
        // Add Density
        if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
        {
            _fluid.addDensity(GetMouseX() / Scale, GetMouseY() / Scale, 100);
        }
        
        
        // Reduce Density
        else if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
        {
            _fluid.reduceDensity(GetMouseX() / Scale, GetMouseY() / Scale, 100,5);
        
        
            //TODO: Suction for Density Reduction

        }

        //Mouse Velocity
        if (Math.Abs(GetMouseX() - PMouse.X) > 0.1 || Math.Abs(GetMouseY() - PMouse.Y) > 0.1)
        {
            var amtX = GetMouseX() - PMouse.X;
            var amtY = GetMouseY() - PMouse.Y;
                
            _fluid.addVelocity(GetMouseX() / Scale, GetMouseY() / Scale, amtX, amtY);

            PMouse = GetMousePosition();
        }
        

        

    }

    public void UpdateSim()
    {
        _fluid.step();
    }

    public void UpdateDraw()
    {
        BeginDrawing();
        ClearBackground(BLACK);
    
        //Debug
        DrawText("FPS: " + GetFPS(), 10, 10, 10, WHITE);
        
        
        //Create a GUI Slider for Viscosity 
        var rect = new Rectangle(10, 20, 100, 20);
        GuiSlider(rect, "", "", _fluid.visc, 0.0000001f, 0.01f);
        
        _fluid.renderD();
    
        EndDrawing();
    }
}