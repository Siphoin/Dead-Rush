using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using FontStyle = System.Drawing.FontStyle;

namespace Dead_Rush.scripts
{
  public static  class GameEngine
    {
        static Player pl = null;

        static Timer timer_spawn_zombies;

        static Timer timer_dispense_zombies;

        static Timer timer_wave_zombies;

        static Timer timerUpdateTextScore;

        static Timer timer_UpdateMove;

        static int level = 1;

        static Baricades Baricades;


        static Label textScore;

        static Label textScoreRecord;

        static Label textHealth;

        static Label textLevel;

        static Button button_new_game;

        static Label defeat_text;

        static PictureBox fon_defeat;

        static Label textBaricasesHealth;
        static    PictureBox Fon = null;
        private static Form active_form = null;
        public static Player player { get => pl; }
        public static PictureBox player_sprite { get => pl_sl; set => pl_sl = value; }


        public static int HeightScreen { get => active_form.Height / 2; }

        public static int WidthScreen { get => active_form.Width / 2; }
        internal static List<Zombie> Zombies { get => zombies; }
        public static int score { get => Score; set => Score = value; }
        public static Baricades baricades { get => Baricades; }
        public static int Level { get => level; }
        public static int score_record { get => ScoreRecord; }

        private static List<Zombie> zombies = new List<Zombie>();

        private static PictureBox pl_sl = null;

        private static int Score = 0;

        private static int ScoreRecord = 0;
        private static int speedPlayer = 2;


        private static string pathImages = "/img/";
        private static string pathAudio = "/audio/";


        public static void AddPlayer (Vector2 position)
        {
            textScore = AddText(new Vector2(700, 5), Color.White, 12, FontStyle.Bold, "Score: 0");
            textScoreRecord = AddText(new Vector2(690, 25), Color.White, 11, FontStyle.Bold, "Your record: 0");
            textLevel = AddText(new Vector2(400, 5), Color.White, 12, FontStyle.Bold, "Score: 0");
            textHealth = AddText(new Vector2(-2, 5), Color.White, 12, FontStyle.Bold, "Health: 100");
            textBaricasesHealth = AddText(new Vector2(-2, 25), Color.White, 8, FontStyle.Bold, "Health baricades: 100");
            PictureBox player = new PictureBox();
            player.Image = GetSpritePath("hitman1_gun");
            player.Location = position.ConvertToPoint();
            player.Size = new System.Drawing.Size(49, 43);

            active_form.Controls.Add(player);
            player.BringToFront();
            player.BackColor = Color.Transparent;
            player.Parent = Fon;
            pl = new Player();
            pl_sl = player;

            timer_UpdateMove = new Timer();
            timer_UpdateMove.Interval = 1;
            timer_UpdateMove.Tick += PlayerMove;
            timer_UpdateMove.Start();
            CircleColider2D colider2D = new CircleColider2D();
            colider2D.Ini(position, 25, pl_sl);
            pl.IniColider(colider2D);
            Fon.Controls.SetChildIndex(player, 0);

            Fon.Click += Fire;

            timerUpdateTextScore = new Timer();
            timerUpdateTextScore.Tick += UpdateTextScore;
            timerUpdateTextScore.Interval = 60;
            timerUpdateTextScore.Start();
            CreateBaricades();

            if (File.Exists("save.txt"))
            {
                try
                {
                    string fileContent = File.ReadAllText("save.txt");
                    ScoreRecord = int.Parse(fileContent);
                }

                catch
                {
                    Application.Exit();
                }
            }

        }

        private static void CreateBaricades()
        {
            PictureBox baricades = new PictureBox();
            baricades.Image = GetSpritePath("baricades");
            Vector2 pos_baricades = new Vector2(200, 0);
            baricades.Location = pos_baricades.ConvertToPoint();
            baricades.Size = new System.Drawing.Size(64, 440);

            active_form.Controls.Add(baricades);

            baricades.BackColor = Color.Transparent;
            baricades.Parent = Fon;

            BoxColider2D col_b = new BoxColider2D(64, 440, pos_baricades, baricades);
            Baricades b = new Baricades(col_b, baricades);
            Baricades = b;
        }

        private static void UpdateTextScore(object sender, EventArgs e)
        {
            textScore.Text = "Score: " + Score;
            textHealth.Text = "Health: " + pl.health;
            textBaricasesHealth.Text = "Health baricades: " + Baricades.health;
            textLevel.Text = "Level: " + level;
            if (ScoreRecord > 0)
            {
            textScoreRecord.Text = "Your record: " + ScoreRecord;
            }

            else
            {
                textScoreRecord.Text = "";
            }


            if (Baricades != null)
            {
                if (Baricades.health <= 0)
                {
                    if (!Baricades.destroyed) {
                    Baricades.Dispose();

                    }

                }
            }

            if (pl.health <= 0)
            {
                timer_spawn_zombies.Stop();
                timer_dispense_zombies.Stop();
                timer_wave_zombies.Stop();
                timerUpdateTextScore.Stop();
                timer_UpdateMove.Stop();
                CreateDefeatWindow();
            }
        }

        private static void Fire(object sender, EventArgs e)
        {

            PictureBox bullet = new PictureBox();
            bullet.Image = GetSpritePath("bullet");
            Vector2 position = new Vector2(pl_sl.Location.X + 28, pl_sl.Location.Y + 28);
            bullet.Location = position.ConvertToPoint();

            bullet.Size = new System.Drawing.Size(30, 8);
            active_form.Controls.Add(bullet);
            bullet.BringToFront();
            bullet.Parent = Fon;
           bullet.BackColor = Color.Transparent;
            CircleColider2D colider2D = new CircleColider2D();
            colider2D.Ini(position, 20, bullet);
            Bullet bullet_obj = new Bullet(bullet, active_form, colider2D);
         Fon.Controls.SetChildIndex(bullet, 1);
          PlaySound("gun");
        }

        private static void PlayerMove(object sender, EventArgs e)
        {
          
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S))
            {

                Vector2 vec = new Vector2(pl_sl.Location);
                vec.y += speedPlayer;
                vec.y = Clamp(vec.y, -389, 389);
                //    System.Diagnostics.Debug.WriteLine(vec.y);
                pl_sl.Location = vec.ConvertToPoint();

            }

            if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
            {
                Vector2 vec = new Vector2(pl_sl.Location);
                vec.y -= speedPlayer;
                vec.y = Clamp(vec.y, 0, 389);
                //  System.Diagnostics.Debug.WriteLine(vec.y);
                pl_sl.Location = vec.ConvertToPoint();
            }

            

        }

        public static Image GetSpritePath (string name)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + pathImages + name + ".png";
            try
            {
                
                return Image.FromFile(path);
            }

            catch 
            {
                throw new Exception("File: " + name + ".png Not Found. Check parameter Copy in project this image! Path: " + path);
            }
        }

        public static void SetActiveForm (Form form)
        {
            active_form = form;
        }

        public static bool CheckCollision (CircleColider2D a, CircleColider2D b)
        {
            double dist = Vector2.Distance(a.center, b.center);
            double sumRadius = a.radius + b.radius;
            if (dist < sumRadius)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public static bool CheckCollision(BoxColider2D a, CircleColider2D b)
        {
            float a_width = a.Width;
            float b_height = a.Height;
            double D = (a_width * a_width) + (b_height * b_height);
            D = Math.Sqrt(D);
            double R = D / 2;
            double R_Result = b.radius + R;
            double dist = Vector2.Distance(a.center, b.center);
            if (dist < R_Result)
            {
                CircleColider2D circleColider2D = new CircleColider2D();
                circleColider2D.Ini(a.center, 40, null, true);

                return CheckCollision(circleColider2D, b);
            }

            else
            {
                return false;
            }

            


        }

        private   static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }


        

        public static void StartSpawnZombies ()
        {

           timer_spawn_zombies = new Timer();
            timer_spawn_zombies.Interval = 2800;
            timer_spawn_zombies.Tick += SpawnZombie;
            timer_spawn_zombies.Start();

            timer_dispense_zombies = new Timer();
            timer_dispense_zombies.Interval = 1200;
            timer_dispense_zombies.Tick += DisposeListZombies;
            timer_dispense_zombies.Start();

            timer_wave_zombies = new Timer();
            timer_wave_zombies.Interval = 60000;
            timer_wave_zombies.Tick += NewWaveZombies;
            timer_wave_zombies.Start();
        }

        private static void NewWaveZombies(object sender, EventArgs e)
        {
            level++;
            if (timer_spawn_zombies.Interval > 900)
            {
                timer_spawn_zombies.Interval -= 300;
            }
        }

        private static void DisposeListZombies(object sender, EventArgs e)
        {
            
           for (int i = 0; i < zombies.Count; i++)
            {
                Zombie zombie = zombies[i];
                if (zombie.destroyed)
                {
                    zombies.Remove(zombie);
                }
            }
        }

        private static void SpawnZombie(object sender, EventArgs e)
        {
            PictureBox zombie = new PictureBox();
            zombie.Image = GetSpritePath("zombie");
            Vector2 position = new Vector2(1000, Random.Range(0, HeightScreen));
            zombie.Location = position.ConvertToPoint();
            
            zombie.Size = new System.Drawing.Size(35, 43);
           
            active_form.Controls.Add(zombie);
            active_form.Invalidate();
            zombie.BringToFront();
            zombie.Parent = Fon;
            zombie.BackColor = Color.Transparent;

            Zombie zombie1 = new Zombie(zombie, active_form);
            CircleColider2D colider2D = new CircleColider2D();


            Vector2 colider_center = new Vector2(zombie.Location);
            colider2D.Ini(colider_center, 25, zombie);
            zombie1.IniColider(colider2D);
            zombies.Add(zombie1);
            Fon.Controls.SetChildIndex(zombie, 2);
        }

        public static void SetMap ()
        {
            int index = Random.Range(1, 3);
            PictureBox fon = new PictureBox();
           fon.Image = GetSpritePath("background" + index);
            fon.Location = new Vector2(0, 0).ConvertToPoint();
            fon.BackColor = Color.Transparent;
            fon.Size = new System.Drawing.Size(active_form.Width, active_form.Height);
            active_form.Controls.Add(fon);
            Fon = fon;


        }

       public static Label AddText (Vector2 position, Color color, int sizeFont, FontStyle style = FontStyle.Regular, string text = "", GraphicsUnit graphicsUnit = GraphicsUnit.Point )
        {
            Label namelabel = new Label();
            namelabel.Location = position.ConvertToPoint();
            namelabel.Text = text;   
            active_form.Controls.Add(namelabel);
            namelabel.BringToFront();
            namelabel.Parent = Fon;
            namelabel.Font = new Font("Arial", sizeFont, style, graphicsUnit);
            namelabel.BackColor = Color.Transparent;
            namelabel.BorderStyle = BorderStyle.None;
            namelabel.ForeColor = color;
            Fon.Controls.SetChildIndex(namelabel, 0);
            namelabel.Size = new System.Drawing.Size(namelabel.Size.Width + namelabel.Text.Length + 5, namelabel.Size.Height + 5);
            return namelabel;
        }

        private static string GetPathAudio (string name)
        {
            return AppDomain.CurrentDomain.BaseDirectory + pathAudio + name + ".wav";
        }


        public static void PlaySound(string audioName)
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = GetPathAudio(audioName);
            player.Play();


        }

        public static void CreateBloodTexture (Vector2 pos)
        {
            PictureBox texture = new PictureBox();
            texture.Location = pos.ConvertToPoint();
            texture.Size = new System.Drawing.Size(49, 43);
            active_form.Controls.Add(texture);
            texture.BringToFront();
            texture.Parent = Fon;
            texture.BackColor = Color.Transparent;
            Fon.Controls.SetChildIndex(texture, 4);
            Blood blood = new Blood(texture);
        }

        private static void CreateDefeatWindow ()
        {
           fon_defeat = new PictureBox();
            fon_defeat.Image = GetSpritePath("background_defeat");
            fon_defeat.Location = new Vector2(0, 0).ConvertToPoint();
            fon_defeat.Size = new System.Drawing.Size(active_form.Width, active_form.Height);
            fon_defeat.Parent = Fon;
            active_form.Controls.Add(fon_defeat);
            fon_defeat.BringToFront();
            fon_defeat.BackColor = Color.Transparent;


            defeat_text = AddText(new Vector2(350, 100), Color.White, 10, FontStyle.Bold, "You died!");
            defeat_text.Parent = fon_defeat;
            active_form.Invalidate();

   button_new_game = new Button();
            button_new_game.Text = "New game";
            Vector2 vector_Button = new Vector2(350, 150);
            button_new_game.Location = vector_Button.ConvertToPoint();
            button_new_game.Size = new System.Drawing.Size(176, 52);
            button_new_game.Parent = fon_defeat;
            fon_defeat.Controls.Add(button_new_game);
            button_new_game.BringToFront();
            button_new_game.BackColor = Color.Transparent;
            button_new_game.Click += NewGameStart;
            foreach (Zombie zombie in zombies)
            {
                zombie.Dispose();
            }

            zombies.Clear();
        }

        private static void NewGameStart(object sender, EventArgs e)
        {
            NewGame();
        }

        private static void NewGame ()
        {
            if (Score > ScoreRecord)
            {
                ScoreRecord = score;
                File.WriteAllText("save.txt", Score.ToString());
            }
            Application.Restart();
        }
    }
}
