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
            if (!_player.canAttack && !_player.IsInteracting())
                return false;

            if(_player.Movement.grounded)
              _player.animator.CrossFade("Attack", 0.2f);
            else
                _player.animator.CrossFade("Jump_Attack", 0.2f);
            _player.sword.AllowHit(true);
            _player.canAttack = false;
            SoundManager.instance.PlaySlash();
            return true;
        }

        public void AllowAttacking()
        {
            _player.canAttack = true;
            _player.sword.AllowHit(false);
        }
    }
}