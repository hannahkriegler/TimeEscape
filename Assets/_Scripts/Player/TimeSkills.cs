using UnityEngine;

namespace TE
{
    public class TimeSkills
    {
        private Player _player;
        private Game _game;

        private Skill _activeSkill;

        GameObject timeStampSpawn;

        public bool firstTimeTravel;
        public bool firstTimeStamp;
        
        public TimeSkills(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        /// <summary>
        /// Checks whether Player can place time stamps. Than executs the Timestamp operation.
        /// </summary>
        public void PlaceTimestamp()
        {
            if (!_game.session.canPlaceTimeStamp)
                return;

            //Cannot Place Timestamps in Mid-Air
            if (!_player.Movement.grounded)
                return;

            firstTimeStamp = true;

            if (timeStampSpawn != null)
                GameObject.Destroy(timeStampSpawn);

            timeStampSpawn = GameObject.Instantiate(_game.timeStampPrefab);
            timeStampSpawn.transform.position = _player.transform.position;

            _game.timeStorage.CreateTimeStamp(_player);
            SoundManager.instance.PlayPortal();
            Debug.Log("Timestamp placed!");
        }
        
        /// <summary>
        /// Checks whether Player can time travel. Than executs the Time Travel operation.
        /// </summary>
        public void TimeTravel()
        {
            if (!_game.session.canTimeTravel)
                return;

            if (!_game.CanTimeTravel())
                return;

            if (!timeStampSpawn)
                return;

            firstTimeTravel = true;
            _game.ReduceTimeShardCounter(4);
            _game.timeStorage.LoadTimeStamp(_player);
            SoundManager.instance.PlayTimeTravel();
            Debug.Log("Time traveled");
        }


        /// <summary>
        /// Defaul time scales with no time skill active.
        /// </summary>
        public void NormalTime()
        {
            _game.playerTimeScale = 1;
            _game.worldTimeScale = 1;
            _game.countDownScale = 1;
        }

        /// <summary>
        /// Timescales for the slow down time skill.
        /// </summary>
        public void SlowDownTime()
        {
            if (!_game.session.HasTimeSkills())
                return;

            _game.playerTimeScale = 0.7f;
            _game.worldTimeScale = 0.25f;
            _game.countDownScale = 4.0f;
        }

        /// <summary>
        /// Timescales for the speed up time skill.
        /// </summary>
        public void SpeedUpTime()
        {
            if (!_game.session.HasTimeSkills())
                return;

            _game.playerTimeScale = 2.0f;
            _game.worldTimeScale = 2.0f;
            _game.countDownScale = 2.0f;
        }
    }
}