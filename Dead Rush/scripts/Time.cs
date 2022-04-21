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
      private  static Timer _timer = null;

      private  static float delta = 0;
    
      public static float DeltaTime => delta;
    
    private static DateTime[] times = {
    DateTime.Now,
    DateTime.Now
    
    };


       private static Time ()
        {
            _timer = new Timer();
         
            _timer.Interval = 1;
            _timer.Tick += GetDelta;
            _timer.Start();
        }

        

        private static void GetDelta(object sender, EventArgs e)
        {
            times[1] = DateTime.Now;
            delta = (times[1].Ticks - times[0].Ticks) / 10000000f;
            Console.WriteLine(times[1].Ticks - times[0].Ticks);
            times[0] = times[1];
        }
    }

   
}
