namespace TE
{
    public class Session
    {
        private Game _game;
        
        public Session(Game game)
        {
            _game = game;
        }


        public bool IsDashUnlocked()
        {
           //TODO Implement check
            return true;
        }

        public bool IsDoubleJumpUnlocked()
        {
            //Implement Check
            return true;
        }
    }
}