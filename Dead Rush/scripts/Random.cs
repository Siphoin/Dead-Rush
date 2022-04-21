using System;

namespace Dead_Rush.scripts
{
    public static class Random
    {
        private static  System.Random _random = new System.Random();
        
        public static int Range (int min, int max)
        {
          return _random.Next(min, max);
        }
    }
}
