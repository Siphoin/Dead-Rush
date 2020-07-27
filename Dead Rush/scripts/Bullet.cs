using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
    public class Bullet : IDisposable
    {
        PictureBox pictureBox;
        Timer timer;
        Form f;
        CircleColider2D Colider;
      public  Bullet(PictureBox picture, Form form, CircleColider2D c)
        {
            Colider = c;
            f = form;
            pictureBox = picture;
             timer = new Timer();
            timer.Interval = 1;
            timer.Tick += Move;
            timer.Start();
        }

        public CircleColider2D colider { get => Colider; }

        public void Dispose()
        {
            f.Controls.Remove(pictureBox);
            timer.Stop();
            timer.Dispose();
            pictureBox.Dispose();
            Colider.Dispose();
        }

        private void Move(object sender, EventArgs e)
        {
            Vector2 vec = Vector2.zero;
            vec = new Vector2(pictureBox.Location);
            vec.x += 6;
            pictureBox.Location = vec.ConvertToPoint();

            for (int i = 0; i < GameEngine.Zombies.Count; i++)
            {
                if (!GameEngine.Zombies[i].destroyed)
                {
                bool col = GameEngine.CheckCollision(Colider, GameEngine.Zombies[i].colider);
                    if (col)
                    {
                        GameEngine.Zombies[i].Destroy();
                        timer.Stop();
                        Dispose();
                    }


                }
               
            }
        //    vec.Log();
            if (vec.x > 800)
            {
                Debug.WriteLine("bullet dispose");
                Dispose();
            }
           
            
        }


    }
}
