using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
    public class Bullet : IDisposable
    {
       private PictureBox _pictureBox;
       private Timer _timer;
       private Form _form;
       private CircleColider2D _colider;
       
      public CircleColider2D Colider => _colider;
       
      public  Bullet(PictureBox picture, Form form, CircleColider2D c)
        {
            _colider = _form;
            _form = _form;
            _pictureBox = picture;
             _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += Move;
            _timer.Start();
        }


        public void Dispose()
        {
            _form.Controls.Remove(_pictureBox);
            _timer.Stop();
            _timer.Dispose();
            _pictureBox.Dispose();
            _colider.Dispose();
        }

        private void Move(object sender, EventArgs e)
        {
            Vector2 vector = Vector2.zero;
            vector = new Vector2(pictureBox.Location);
            vector.x += 6;
            _pictureBox.Location = vector.ConvertToPoint();

            for (int i = 0; i < GameEngine.Zombies.Count; i++)
            {
                if (!GameEngine.Zombies[i].destroyed)
                {
                bool collision = GameEngine.CheckCollision(Colider, GameEngine.Zombies[i].colider);
                
                    if (collision)
                    {
                        GameEngine.Zombies[i].Destroy();
                        _timer.Stop();
                        Dispose();
                    }


                }
               
            }
            
            
            if (vector.x > 800)
            {
                Debug.WriteLine("bullet dispose");
                Dispose();
            }
           
            
        }


    }
}
