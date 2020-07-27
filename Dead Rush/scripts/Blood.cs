using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
 public   class Blood : IDisposable
    {
        private PictureBox picture = null;
        private Timer timer = null;
        public Blood (PictureBox pictureBox)
        {
            int index = Random.Range(1, 6);
            pictureBox.Image = GameEngine.GetSpritePath("bl_texture" + index);
            picture = pictureBox;
            timer = new Timer();
            timer.Tick += Destroy;
            timer.Interval = 10000;
            timer.Start();
        }

        public void Dispose()
        {
            picture.Parent.Controls.Remove(picture);
            timer.Stop();
            timer.Dispose();
            picture.Dispose();
        }

        private void Destroy(object sender, EventArgs e)
        {
            Dispose();

        }
    }
}
