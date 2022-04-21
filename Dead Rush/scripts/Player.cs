namespace Dead_Rush.scripts
{
    public  class Player
    {

      private  CircleColider2D _colider = null;
        
        public int Health { get; set; } = 100;
        
        public CircleColider2D Colider => _colider;

        public void IniColider (CircleColider2D colider) => _colider = colider;
    }
}
