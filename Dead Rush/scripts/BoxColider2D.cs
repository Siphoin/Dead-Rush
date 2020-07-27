using System;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
    public class BoxColider2D : PhysicsColider, IDisposable
    {
        int width = 0;
        int height = 0;


        private Vector2 Center;

        PictureBox picture;

        public Vector2 center { get => Center; }
        public int Width { get => width; }
        public int Height { get => height; }

        private Timer timerDraw = null;

       public BoxColider2D (int Width, int Height, Vector2 position, PictureBox pictureBox, bool StaticBody = true)
        {
            staticBody = StaticBody;
            width = Width;
            height = Height;
            if (pictureBox != null)
            {
                picture = pictureBox;
            }
            Center = position;
            if (!staticBody)
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

        public void Dispose()
        {
            picture.Dispose();
            Center.Dispose();
            if (timerDraw != null)
            timerDraw.Dispose();
        }
    }
    }
