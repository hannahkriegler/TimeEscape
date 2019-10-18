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

        /// <summary>
        /// Checks whether the player can attadck and than executes the attack.
        /// </summary>
        /// <returns></returns>
        public bool Attack()
        {
            if (!_player.canAttack)
                return false;

            if (!_player.hasSword)
                return false;
       
            if(_player.Movement.grounded)
              _player.animator.Play("Attack");
            else
                _player.animator.Play("Jump_Attack");
            _player.canAttack = false;
            SoundManager.instance.PlaySlash();
            return true;
        }

        /// <summary>
        /// Reset states to allow player to attack again.
        /// </summary>
        public void ResetAttackState()
        {
            _player.canAttack = true;
            _player.sword.AllowHit(false);
        }
    }
}