using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
    public class CircleColider2D : PhysicsColider, IDisposable
    {
        private double rad = 0;

        private Vector2 Center;

        PictureBox picture;

        public Vector2 center { get => Center; }
        public double radius { get => rad; }

        private Timer timerDraw = null;

        public void Ini(Vector2 position, double r, PictureBox target_image, bool static_body = false)
        {
            staticBody = static_body;
            rad = r;
            if (target_image != null)
            {
            picture = target_image;
            }

            Center = position;
            if (!static_body)
            {
             timerDraw = new Timer();
            timerDraw.Tick += RedrawColider;
            timerDraw.Interval = 1;
            timerDraw.Start();
            }

        }

        private void RedrawColider(object sender, EventArgs e)
        {
            if (picture.Location.Y != Center.y || picture.Location.X != Center.x)
            {
Center = new Vector2(picture.Location);
           //     Center.Log();


            }
            
        }

        public void DestroyColider ()
        {
           
        }

        public void Dispose()
        {
            picture.Dispose();
            Center.Dispose();
            timerDraw.Dispose();
            rad = 0;

        }
    }
}