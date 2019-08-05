using UnityEngine;

namespace TE
{
    public class TimeStorage
    {
        private Game _game;
        
        public float current_Time;
        public float savePoint_Time;
        
        //TODO Room State

        public TimeStorage(Game game)
        {
            _game = game;
        }
    }
}