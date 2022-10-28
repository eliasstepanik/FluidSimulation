using System.Numerics;
using Raylib_CsLo;
using static Raylib_CsLo.RayMath;

namespace PixelEngine;

public class Fluid
{
    public int size { get; set; }
    public int scale { get; set; }
    public int iter { get; set; }
    
    public float dt { get; set; }
    public float diff { get; set; }
    public float visc { get; set; }
    
    public float[] s { get; set; }
    public float[] density { get; set; }
    
    public float[] Vx { get; set; }
    public float[] Vy { get; set; }
    
    public float[] Vx0 { get; set; }
    public float[] Vy0 { get; set; }


    public Fluid(float dt, float diffusion, float viscosity, int N, int iter, int scale) {

        this.size = N;
        this.iter = iter;
        this.dt = dt;
        this.diff = diffusion;
        this.visc = viscosity;
        this.scale = scale;

        this.s = new float[N*N];
        this.density = new float[N*N];

        this.Vx = new float[N*N];
        this.Vy = new float[N*N];

        this.Vx0 = new float[N*N];
        this.Vy0 = new float[N*N];
    }

    private static dynamic wrapper;
    public void step() {
        int N          = this.size;
        float visc     = this.visc;
        float diff     = this.diff;
        float dt       = this.dt;
        float[] Vx      = this.Vx;
        float[] Vy      = this.Vy;
        float[] Vx0     = this.Vx0;
        float[] Vy0     = this.Vy0;
        float[] s       = this.s;
        float[] density = this.density;

        diffuse(1, Vx0, Vx, visc, dt);
        diffuse(2, Vy0, Vy, visc, dt);

        project(Vx0, Vy0, Vx, Vy);
        advect(this, 1, Vx, Vx0, Vx0, Vy0, dt);
        advect(this,2, Vy, Vy0, Vx0, Vy0, dt);

        project(Vx, Vy, Vx0, Vy0);

        diffuse(0, s, density, diff, dt);
        advect(this,0, density, s, Vx, Vy, dt);
    }


    void fadeD() {
      for (int i = 0; i < this.density.Length; i++) {
        float d = density[i];
        density[i] = Clamp(d-0.02f, 0, 255);
      }
    }
    
    public void renderD() {
      for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
          int x = i * scale;
          int y = j * scale;
          float d = this.density[IX(i, j)];
          
          Color color = new Color(/*((d + 50) % 255)*/100,255,0,(int)d * 2);
          Raylib.DrawRectangle(x, y, scale, scale, color);
        }
      }
    }


    public int IX(int x, int y)
    {

      var X = Clamp(x, 0, size - 1);;
      var Y = Clamp(y, 0, size - 1);
      return (int) (X + Y * size);
    }

    public void addDensity(int x, int y, float amount) {
      int index = IX(x, y);
      this.density[index] += amount;
    }
    
    public void reduceDensity(int x, int y, float amount, int rSize) {
      
       Parallel.For(-rSize, rSize, i =>
       {
         for (int j = -rSize; j < rSize; j++)
         {
           int index = IX(x + i, y + j);

           if (this.density[index] - amount < 0)
           {
             this.density[index] = 0;
           }
           else
           {
             this.density[index] -= amount;
           }
         }
       });
    }
    

    public void addVelocity(int x, int y, float amountX, float amountY) {
      int index = IX(x, y);
      this.Vx[index] += amountX;
      this.Vy[index] += amountY;
    }


    void diffuse (int b, float[] x, float[] x0, float diff, float dt) {
      float a = dt * diff * (size - 2) * (size - 2);
      lin_solve(b, x, x0, a, 1 + 4 * a);
    }
    void lin_solve(int b, float[] x, float[] x0, float a, float c) {
      float cRecip = 1.0f / c;
      Parallel.For(0, iter, k =>
      {
        for (int j = 1; j < size - 1; j++) {
          for (int i = 1; i < size - 1; i++) {
            x[IX(i, j)] =
              (x0[IX(i, j)]
               + a*(    x[IX(i+1, j)]
                        +x[IX(i-1, j)]
                        +x[IX(i, j+1)]
                        +x[IX(i, j-1)]
               )) * cRecip;
          }
        }

        set_bnd(b, x);
      }); 
    }
    void project(float[] velocX, float[] velocY, float[] p, float[] div)
    {
      Parallel.For(1, size, j =>
      {
        for (int i = 1; i < size - 1; i++)
        {
          div[IX(i, j)] = -0.5f * (
            velocX[IX(i + 1, j)]
            - velocX[IX(i - 1, j)]
            + velocY[IX(i, j + 1)]
            - velocY[IX(i, j - 1)]
          ) / size;
          p[IX(i, j)] = 0;
        }
      });

      set_bnd(0, div); 
      set_bnd(0, p);
      lin_solve(0, p, div, 1, 4);

      Parallel.For(1, size, j =>
      {
        for (int i = 1; i < size - 1; i++)
        {
          velocX[IX(i, j)] -= 0.5f * (p[IX(i + 1, j)]
                                      - p[IX(i - 1, j)]) * size;
          velocY[IX(i, j)] -= 0.5f * (p[IX(i, j + 1)]
                                      - p[IX(i, j - 1)]) * size;
        }
      });
      set_bnd(1, velocX);
      set_bnd(2, velocY);
    }
    
    public void advect(Fluid fluid,int b, float[] d, float[] d0, float[] velocX, float[] velocY, float dt) {
      float i0, i1, j0, j1;

      float dtx = dt * (fluid.size - 2);
      float dty = dt * (fluid.size - 2);

      float s0, s1, t0, t1;
      float tmp1, tmp2, x, y;

      float Nfloat = fluid.size;
      float ifloat, jfloat;
      int i, j;

      for (j = 1, jfloat = 1; j < fluid.size - 1; j++, jfloat++) { 
        for (i = 1, ifloat = 1; i < fluid.size - 1; i++, ifloat++) {
          tmp1 = dtx * velocX[fluid.IX(i, j)];
          tmp2 = dty * velocY[fluid.IX(i, j)];
          x    = ifloat - tmp1; 
          y    = jfloat - tmp2;

          if (x < 0.5f) x = 0.5f; 
          if (x > Nfloat + 0.5f) x = Nfloat + 0.5f; 
          i0 = RayMath.floorf(x); 
          i1 = i0 + 1.0f;
          if (y < 0.5f) y = 0.5f; 
          if (y > Nfloat + 0.5f) y = Nfloat + 0.5f; 
          j0 = RayMath.floorf(y);
          j1 = j0 + 1.0f; 

          s1 = x - i0; 
          s0 = 1.0f - s1; 
          t1 = y - j0; 
          t0 = 1.0f - t1;

          int i0i = (int)i0;
          int i1i = (int)i1;
          int j0i = (int)j0;
          int j1i = (int)j1;

          // DOUBLE CHECK THIS!!!
          d[fluid.IX(i, j)] = 
            s0 * (t0 * d0[fluid.IX(i0i, j0i)] + t1 * d0[fluid.IX(i0i, j1i)]) +
            s1 * (t0 * d0[fluid.IX(i1i, j0i)] + t1 * d0[fluid.IX(i1i, j1i)]);
        }
      }

      fluid.set_bnd(b, d);
    }
    void set_bnd(int b, float[] x)
    {
      Parallel.For(1, size, i =>
      {
        x[IX(i, 0  )] = b == 2 ? -x[IX(i, 1  )] : x[IX(i, 1 )];
        x[IX(i, size-1)] = b == 2 ? -x[IX(i, size-2)] : x[IX(i, size-2)];
      });

      Parallel.For(1, size, j =>
      {
        x[IX(0, j)] = b == 1 ? -x[IX(1, j)] : x[IX(1, j)];
        x[IX(size - 1, j)] = b == 1 ? -x[IX(size - 2, j)] : x[IX(size - 2, j)];
      });

      x[IX(0, 0)] = 0.5f * (x[IX(1, 0)] + x[IX(0, 1)]);
      x[IX(0, size-1)] = 0.5f * (x[IX(1, size-1)] + x[IX(0, size-2)]);
      x[IX(size-1, 0)] = 0.5f * (x[IX(size-2, 0)] + x[IX(size-1, 1)]);
      x[IX(size-1, size-1)] = 0.5f * (x[IX(size-2, size-1)] + x[IX(size-1, size-2)]);
    }

}