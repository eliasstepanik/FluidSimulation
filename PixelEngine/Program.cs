using System.Drawing;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayMath;
using static Raylib_CsLo.RayGui;
using System.Drawing;
using PixelEngine;
using Raylib_CsLo;


/*var c = new CircleAndClock();
c.Run();*/
var N = 64;
var Scale = 10;
var Iter = 10;
float t = 0;
int whiteholes = 4;
int blackholes = 1;

Fluid fluid;

fluid = new Fluid(0.2f, 0, 0.0000001f, N, Iter, Scale);
InitWindow(N * Scale, N * Scale, "Raylib_CsLo");
SetTargetFPS(60);
Random rd = new Random();

var pMouseX = GetMouseX();
var pMouseY = GetMouseY();

var whiteholesA = new WhiteHole[whiteholes];
var blackholesA = new BlackHole[blackholes];

for (int i = 0; i < whiteholes; i++)
{
    whiteholesA[i] = new WhiteHole(
        new Vector2(rd.Next(3, N - 3), rd.Next(3, N - 3)),
        new Vector2(rd.Next(50, 100), rd.Next(50, 100)),
        rd.Next(0, 100),
        rd.Next(0, 100)
    );
}

for (int i = 0; i < blackholes; i++)
{
    blackholesA[i] = new BlackHole(
        new Vector2(rd.Next(3, N - 3), rd.Next(3, N - 3)),
        rd.Next(0, 3),
        rd.Next(0,200)
    );
}



while (!WindowShouldClose())
{
    BeginDrawing();
    ClearBackground(BLACK);
    DrawFPS(10,10);
    
    /*int cx = (int)0.5*N/Scale;
    int cy = (int)0.5*N/Scale;
    for (int i = -1; i <= 1; i++) {
        for (int j = -1; j <= 1; j++) {
            fluid.addDensity(cx+i, cy+j, rd.Next(50, 150));
        }
    }*/
    /*for (int i = 0; i < 2; i++) {
        float angle = noise(t) * TWO_PI * 2;
        PVector v = PVector.fromAngle(angle);
        v.mult(0.2);
        t += 0.01;
        fluid.addVelocity(cx, cy, v.x, v.y );
    }*/


    /*fluid.step();
    fluid.renderD();*/


    if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
        fluid.addDensity((int)GetMouseX() / Scale, (int)GetMouseY() / Scale, 100);
    else if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
    {
        fluid.reduceDensity((int)GetMouseX() / Scale, (int)GetMouseY() / Scale, 100, 5);
        
        
        //TODO: Suction for Density Reduction
        /*var amtX = GetMouseX() - pMouseX;
        var amtY = GetMouseY() - pMouseY;
        
        fluid.addVelocity((int)GetMouseX() / Scale, (int)GetMouseY() / Scale, amtX, amtY);*/
    }




    foreach (var hole in blackholesA)
    {
        hole.Spawn(fluid);
    }
    
    foreach (var hole in whiteholesA)
    {
        hole.Spawn(fluid);
    }
    
    
    /*fluid.addDensity(N - 3 , 3,  100);
    fluid.addVelocity(N - 3 , 3,  -10f, 0);*/
    
    
        

    /*if (GetMouseX() != pMouseX || GetMouseY() != pMouseY)
    {

        var amtX = GetMouseX() - pMouseX;
        var amtY = GetMouseY() - pMouseY;
        
        fluid.addVelocity((int)GetMouseX() / Scale, (int)GetMouseY() / Scale, amtX, amtY);
    }*/

    pMouseX = GetMouseX();
    pMouseY = GetMouseY();
    
    fluid.step();
    fluid.renderD();
    

    EndDrawing();
}
