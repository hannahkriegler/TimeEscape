using UnityEngine;

namespace TE
{
    public class TimeStorage
    {
        private Game _game;
        
        public float saved_time;
        public Vector3 saved_player_pos;
        bool hasTimeStamp;
        
        //TODO Room State

        public TimeStorage(Game game)
        {
            _game = game;
        }

        public void CreateTimeStamp(Player player)
        {
            saved_time = Game.TimeLeft;
            saved_player_pos = player.rigidBody.position;
            _game.HandleTimeStampEnemies();
            hasTimeStamp = true;
        }

        public void LoadTimeStamp(Player player)
        {
            if (!hasTimeStamp)
                return;
            Game.TimeLeft = saved_time;
            player.Movement.Teleport(saved_player_pos);
            _game.HandleTimeTravelEnemies();
        }
    }
}