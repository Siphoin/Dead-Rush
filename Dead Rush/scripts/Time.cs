using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dead_Rush.scripts
{
  public static  class Time
    {
        static Timer timer = null;

    static    DateTime time1 = DateTime.Now;
    static    DateTime time2 = DateTime.Now;

        static float delta = 0;


        static Time ()
        {
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += GetDelta;
            timer.Start();
        }

        public static float deltaTime { get => delta; }

        private static void GetDelta(object sender, EventArgs e)
        {
            time2 = DateTime.Now;
            delta = (time2.Ticks - time1.Ticks) / 10000000f;
            Console.WriteLine(time2.Ticks - time1.Ticks); // *int* output {2493331}
            time1 = time2;
        }
    }

   
}
