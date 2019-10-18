using UnityEngine;

namespace TE
{
    public class CombatSkill
    {
        private Player _player;
        private Game _game;

        private Skill _activeSkill;
        
        public CombatSkill(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        /// <summary>
        /// Assigns the FireballSkill as active skill
        /// </summary>
        public void GetFireBall()
        {
            _activeSkill = new FireBallSkill();
        }

        /// <summary>
        /// Executes the current active skills. 
        /// </summary>
        public void ActivateActiveSkill()
        {
            if(_activeSkill == null && _game.session.HasFireball())
            {
                GetFireBall();
            }

            _activeSkill?.Activate(_player);
        }

        /// <summary>
        /// Executes the actual effect for the current active skill. 
        /// </summary>
        public void TriggerSkill()
        {
            _activeSkill?.Trigger(_player);
        }
        
    }
}