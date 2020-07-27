using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
    class Zombie : IDisposable
    {
        private int Health = 50;

        private int speed = 1;
        private Form active_form = null;

        private PictureBox picture;

        private Timer timerMove = null;
        CircleColider2D Colider = null;

        public int health { get => Health; set => Health = value; }
        public CircleColider2D colider { get => Colider; }
        public bool destroyed { get => Destroyed; }

        private bool Destroyed = false;

        private bool colEn = false;

        private bool colEnBaricades = false;

        const int damage = 2;



        public void IniColider(CircleColider2D c)
        {
            Colider = c;
        }

        

        public Zombie (PictureBox image, Form form)
        {
            if (GameEngine.Level > 2)
            {
            speed = Random.Range(1, 4);
            }

            picture = image;
            active_form = form;
             timerMove = new Timer();
            timerMove.Tick += Move;
            timerMove.Interval = 1;
            timerMove.Start();
        }

        private void Move(object sender, EventArgs e)
        {
            if (!GameEngine.baricades.destroyed)
            {
if (picture.Location.X <= 260)
            {
                Debug.WriteLine("col b");
                if (!colEnBaricades)
                {
                       colEnBaricades = true;
                timerMove.Stop();
                timerMove = new Timer();
                timerMove.Interval = 600;
                timerMove.Tick += AttackBaricades;
                timerMove.Start();
                }

                return;
            }
            }





            
            Vector2 vec = Vector2.zero;
            if (!GameEngine.CheckCollision(Colider, GameEngine.player.colider)) {
            vec = new Vector2(picture.Location);
            vec.x -= speed;
            picture.Location = vec.ConvertToPoint();
            }


           else
            {
                if (!colEn)
                {
                    if (GameEngine.player.health > 0)
                    {
colEn = true;
                timerMove.Stop();
                timerMove = new Timer();
                    timerMove.Interval = 600;
                    timerMove.Tick += Attack;
                    timerMove.Start();
                Debug.WriteLine("col");
                    }
                    
                }

            }

            if (vec.x  < GameEngine.HeightScreen / 46 - 100)
            {
                Debug.WriteLine("zombie dispose");
                Dispose();
            }
        }

        private void Attack(object sender, EventArgs e)
        {
            if (!GameEngine.CheckCollision(Colider, GameEngine.player.colider))
            {
                timerMove.Stop();
                timerMove = new Timer();
                timerMove.Tick += Move;
                timerMove.Interval = 1;
                timerMove.Start();
                return;
            }
                if (GameEngine.player.health > 0)
            {
                GameEngine.player.health -= damage;
            }

            else
            {
                timerMove.Stop();
                timerMove = new Timer();
                timerMove.Tick += Move;
                timerMove.Interval = 1;
                timerMove.Start();
            }
        }

        private void AttackBaricades(object sender, EventArgs e)
        {
            if (GameEngine.baricades.destroyed)
            {
                if (colEnBaricades)
                {
                    colEnBaricades = false;
                    timerMove.Stop();
                    timerMove = new Timer();                   
                    timerMove.Tick += Move;
                    timerMove.Interval = 1;
                    timerMove.Start();
                }
            }
            if (GameEngine.baricades.health > 0)
            {
                GameEngine.baricades.health -= damage;
               
            }

        }

        public void Dispose()
        {
          
            active_form.Controls.Remove(picture);
            timerMove.Stop();
            timerMove.Dispose();
            picture.Dispose();
            Colider.Dispose();
            Destroyed = true;
        }

        public void Destroy ()
        {
            GameEngine.PlaySound("zombie_death");
            Vector2 vector = new Vector2(picture.Location.X, picture.Location.Y);
            GameEngine.CreateBloodTexture(vector);
            GameEngine.score++;
            Dispose();
        }

        
    }
}
