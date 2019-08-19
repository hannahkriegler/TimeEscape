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
            if (!_player.canAttack)
                return;

            _player.animator.Play("Attack");
            _player.sword.AllowHit(true);
            _player.canAttack = false;
        }
    }
}