using UnityEngine;

namespace TE
{
    public class CombatMelee
    {
        private Player _player;
        private Game _game;

        public CombatMelee(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        public bool Attack()
        {
            if (!_player.canAttack)
                return false;
       
            if(_player.Movement.grounded)
              _player.animator.Play("Attack");
            else
                _player.animator.Play("Jump_Attack");
            _player.canAttack = false;
            SoundManager.instance.PlaySlash();
            return true;
        }

        public void ResetAttackState()
        {
            _player.canAttack = true;
            _player.sword.AllowHit(false);
        }
    }
}