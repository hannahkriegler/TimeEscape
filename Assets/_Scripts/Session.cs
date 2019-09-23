using UnityEngine;

namespace TE
{
    public class Session
    {
        private Game _game;

        int dashCollected;
        bool jumpCollected;
        bool timeSkills;

        public bool canTimeTravel { get; private set; }
        public bool canPlaceTimeStamp { get; private set; }

        public Session(Game game)
        {
            _game = game;
        }

        public void UnlockTimeTravel()
        {
            canTimeTravel = true;
        }

        public void UnlockTimestamp()
        {
            canPlaceTimeStamp = true;
        }

        public void CollectedDashLoot()
        {
            dashCollected++;
            dashCollected = Mathf.Clamp(dashCollected, 0, 2);
            _game.dashIcon.SetActive(true);
        }

        public void CollectedJumpLoot()
        {
            jumpCollected = true;
            _game.jumpIcon.SetActive(true);
        }

        public void UnlockTimeSkills()
        {
            //TODO Permanent unlock
            timeSkills = true;
            _game.ShowTextBox("Du kannst jetzt die Zeit verlangsamen mit LB !");
        }


        public bool IsDashUnlocked()
        {
            return dashCollected > 0 || _game.allMovementSkills;
        }

        public bool IsDoubleJumpUnlocked()
        {
            return jumpCollected || _game.allMovementSkills;
        }

        public bool HasTimeSkills()
        {
            return timeSkills || _game.allTimeSkills;
        }
    }
}