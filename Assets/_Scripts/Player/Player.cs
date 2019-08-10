using System;
using UnityEngine;

namespace TE
{
    public class Player : MonoBehaviour, IHit
    {
        private Game _game;
        
        public Movement Movement { get; private set; }
        public CombatMelee CombatMelee { get; private set; }
        public CombatSkill CombatSkill { get; private set; }
        
        public Rigidbody2D rigidBody  { get; private set; }

        public SwordHook sword  { get; private set; }
        [Header("References")]
        public Transform groundCheck;
        public LayerMask groundLayerCheck;
        
        [Header("Settings")]
        public float moveSpeed = 365f;
        public float maxSpeed = 5;
        public float jumpVelocity = 10;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        
        public float delta { get; private set; }
        public float fixedDelta { get; private set; }
        
        public void Init(Game game)
        {
            this._game = game;
            rigidBody = GetComponent<Rigidbody2D>();
            sword = GetComponentInChildren<SwordHook>();
            Movement = new Movement(this, _game);
            CombatMelee = new CombatMelee(this, _game);
            CombatSkill = new CombatSkill(this, _game);
        }

        private void Update()
        {
            //delta = Time.deltaTime * _game.playerTimeScale;
            //fixedDelta = Time.fixedDeltaTime * _game.playerTimeScale;
        }

        public void OnHit()
        {
            Debug.Log("Player hitted!");
        }
        
    }
}