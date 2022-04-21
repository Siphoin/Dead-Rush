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
        private PictureBox _picture;
        private Timer _timer;
        
        public Blood (PictureBox pictureBox)
        {
            int index = Random.Range(1, 6);
            
            _pictureBox.Image = GameEngine.GetSpritePath("bl_texture" + index);
            _picture = pictureBox;
            _timer = new Timer();
            _timer.Tick += Destroy;
            _timer.Interval = 10000;
            _timer.Start();
        }

        public void Dispose()
        {
            _picture.Parent.Controls.Remove(_picture);
            _timer.Stop();
            _timer.Dispose();
            _picture.Dispose();
        }

        private void Destroy(object sender, EventArgs e) => Dispose();
    }
}
