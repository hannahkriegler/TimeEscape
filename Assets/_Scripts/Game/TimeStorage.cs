using UnityEngine;

namespace TE
{
    public class TimeStorage
    {
        private Game _game;
        
        public float saved_time;
        public Vector3 saved_player_pos;
        bool hasTimeStamp;
        

        public TimeStorage(Game game)
        {
            _game = game;
        }


        /// <summary>
        /// Tells the game that the player placed the timestamp.
        /// </summary>
        /// <param name="player">Player who placed the timestamp</param>
        public void CreateTimeStamp(Player player)
        {
            saved_time = Game.instance.timeLeft;
            saved_player_pos = player.rigidBody.position;
            _game.HandleTimeStampEnemies();
            hasTimeStamp = true;
        }

        /// <summary>
        /// Tells the game that the player timetraveled.
        /// </summary>
        /// <param name="player">Player who timetraveled </param>
        public void LoadTimeStamp(Player player)
        {
            if (!hasTimeStamp)
                return;
            Game.instance.timeLeft = saved_time;
            player.Movement.Teleport(saved_player_pos);
            _game.HandleTimeTravelEnemies();
        }
    }

    public interface ITimeTravel
    {
        /// <summary>
        /// Player has placed a time stamp. Actor should save all current states.
        /// </summary>
        void HandleTimeStamp();

        /// <summary>
        /// Loads all saved states of the actor.
        /// </summary>
        void HandleTimeTravel();
    }
}