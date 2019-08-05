using UnityEngine;

namespace TE
{
    public class CombatSkill
    {
        private Player _player;
        private Game _game;

        private Skill _chargeSkill;
        private Skill _activeSkill;
        
        public CombatSkill(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        public void ActivateChargeSkill()
        {
            Debug.Log("Charge Skill activated!");
            _chargeSkill?.Activate();
        }

        public void ActivateActiveSkill()
        {
            Debug.Log("Active Skill activated!");
            _activeSkill?.Activate();
        }
        
    }
}