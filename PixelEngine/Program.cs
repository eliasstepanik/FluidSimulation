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

Fluid fluid;

fluid = new Fluid(0.2f, 0, 0.0000001f, N, Iter, Scale);
InitWindow(N * Scale, N * Scale, "Raylib_CsLo");
SetTargetFPS(60);
Random rd = new Random();


var pMouseX = GetMouseX();
var pMouseY = GetMouseY();


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
        fluid.reduceDensity((int)GetMouseX() / Scale, (int)GetMouseY() / Scale, 100);
        
        
        //TODO: Suction for Density Reduction
        /*var amtX = GetMouseX() - pMouseX;
        var amtY = GetMouseY() - pMouseY;
        
        fluid.addVelocity((int)GetMouseX() / Scale, (int)GetMouseY() / Scale, amtX, amtY);*/
    }
    
    
    
    fluid.addDensity(N - 3, 3,  200);
    fluid.addVelocity(N - 3 , 3,  -10f, -10f);
        

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
