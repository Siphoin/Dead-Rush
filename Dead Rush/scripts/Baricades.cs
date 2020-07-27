using System;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
    public  class Baricades : IDisposable
    {
        BoxColider2D Colider = null;
        private int Health = 1000;
        private PictureBox picture;

        bool Destroyed = false;

   public     Baricades (BoxColider2D c, PictureBox pictureBox)
        {
            picture = pictureBox;
            Colider = c;
        }

        public int health { get => Health; set => Health = value; }
        public BoxColider2D colider { get => Colider; }
        public bool destroyed { get => Destroyed; }

        public void Dispose()
        {
            Destroyed = true;
            picture.Parent.Controls.Remove(picture);
            picture.Dispose();
            Colider.Dispose();
        }
    }
}
