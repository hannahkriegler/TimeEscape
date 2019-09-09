using UnityEngine;

namespace TE
{
    public class Session
    {
        private Game _game;

        int dashCollected;
        bool jumpCollected;
        
        public Session(Game game)
        {
            _game = game;
        }

        public void CollectedDashLoot()
        {
            dashCollected++;
            dashCollected = Mathf.Clamp(dashCollected, 0, 2);
        }

        public void CollectedJumpLoot()
        {
            jumpCollected = true;
        }


        public bool IsDashUnlocked()
        {
            return dashCollected > 0 || _game.allMovementSkills;
        }

        public bool IsDoubleJumpUnlocked()
        {
            return jumpCollected || _game.allMovementSkills;
        }
    }
}