using Dead_Rush.scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dead_Rush
{
    public partial class Scene : Form
    {
        public Scene()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            Text = Application.ProductName;
            MaximizeBox = false;
            GameEngine.SetActiveForm(this);
            GameEngine.SetMap();
            GameEngine.AddPlayer(new Vector2(0, 175));
            GameEngine.StartSpawnZombies();
        }


        private void Scene_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Scene_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
