namespace Dead_Rush.scripts
{
    public  class Player
    {
        private int Health = 100;

        CircleColider2D Colider = null;
        public int health { get => Health; set => Health = value; }
        public CircleColider2D colider { get => Colider; }

        public void IniColider (CircleColider2D c)
        {
            Colider = c;
        }
    }
}
