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
            //TODO-Hannah Implement basic melee Attack
            Debug.Log(("Melee Attack!"));
        }
    }
}