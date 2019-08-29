using UnityEngine;

namespace TE
{
    public class TimeSkills
    {
        private Player _player;
        private Game _game;

        private Skill _activeSkill;

        GameObject timeStampSpawn;
        
        public TimeSkills(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        public void PlaceTimestamp()
        {
            //Cannot Place Timestamps in Mid-Air
            if (!_player.Movement.grounded)
                return;

            if (timeStampSpawn != null)
                GameObject.Destroy(timeStampSpawn);

            timeStampSpawn = GameObject.Instantiate(_game.timeStampPrefab);
            timeStampSpawn.transform.position = _player.transform.position;

            _game.timeStorage.CreateTimeStamp(_player);
            Debug.Log("Timestamp placed!");
        }
        
        public void TimeTravel()
        {
            _game.timeStorage.LoadTimeStamp(_player);
            Debug.Log("Time traveled");
        }
    }
}