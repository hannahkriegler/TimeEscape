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

        public void GetFireBall()
        {
            _activeSkill = new FireBallSkill();
        }

        public void ActivateActiveSkill()
        {
            if(_activeSkill == null && _game.session.HasFireball())
            {
                GetFireBall();
            }

            _activeSkill?.Activate(_player);
        }
        
    }
}