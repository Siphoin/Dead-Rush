using System;
using System.Diagnostics;
using System.Drawing;

namespace Dead_Rush.scripts
{
    public  struct Vector2 : IDisposable
    {
        public int x;
        public int y;

        public static Vector2 zero { get => new Vector2(0, 0); }
        public static Vector2 one { get => new Vector2(1, 1); }

        public Vector2 (int X, int Y)
        {
            x = X;
            y = Y;
        }

        public Vector2(Point point)
        {
            x = point.X;
            y = point.Y;
        }


        public Point ConvertToPoint ()
        {
            return new Point(x, y);
        }

        public static double Distance (Vector2 a, Vector2 b)
        {
            double c = (Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
            
            c = Math.Sqrt(c);
            return c;


        }


        public void Dispose()
        {
            this.x = 0;
            this.y = 0;
        }
        
        public void Log () => Debug.WriteLine("X: " + x + " Y: " + y);
    }
    }

