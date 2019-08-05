using System;
using UnityEngine;

namespace TE
{
    public class Player : MonoBehaviour, IHit
    {
        private Game _game;
        
        public Movement Movement { get; private set; }
        public CombatMelee CombatMelee { get; private set; }

        [Header("References")]
        public Rigidbody2D rigidBody;

        public SwordHook sword;

        public float delta { get; private set; }
        
        public void Init(Game game)
        {
            this._game = game;
            Movement = new Movement(this, _game);
            CombatMelee = new CombatMelee(this, _game);
        }

        private void Update()
        {
            delta = _game.deltaPlayer;
        }

        public void OnHit()
        {
            Debug.Log("Player hitted!");
        }
        
    }
}