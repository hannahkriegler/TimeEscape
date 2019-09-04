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

        public void Attack()
        {
            if (!_player.canAttack && !IsAttacking())
                return;

            if(_player.Movement.grounded)
              _player.animator.CrossFade("Attack", 0.2f);
            else
                _player.animator.CrossFade("Jump_Attack", 0.2f);
            _player.sword.AllowHit(true);
            _player.canAttack = false;
        }

        public bool IsAttacking()
        {
            //TODO Improve Attack Check
            return _player.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
                _player.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump_Attack");
        }
    }
}