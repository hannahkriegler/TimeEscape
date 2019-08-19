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
        
        public Collider2D col { get; private set; }

        public Animator animator { get; private set; }

        public SwordHook sword  { get; private set; }
        [Header("References")]
        public LayerMask groundLayerCheck;
        
        [Header("Settings")]
        public float moveSpeed = 50;
        public float jumpVelocity = 10;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        public float gravityMultiplier = 2f;


        [Header("States")]
        public bool canAttack;
        
        public float delta { get; private set; }
        public float fixedDelta { get; private set; }
        
        public void Init(Game game)
        {
            //Setting References
            _game = game;
            rigidBody = GetComponent<Rigidbody2D>();
            sword = GetComponentInChildren<SwordHook>();
            animator = GetComponentInChildren<Animator>();
            col = GetComponent<Collider2D>();
            
            //Init Components
            Movement = new Movement(this, _game);
            CombatMelee = new CombatMelee(this, _game);
            CombatSkill = new CombatSkill(this, _game);
            canAttack = true;
        }

        private void Update()
        {
            delta = Time.deltaTime * _game.playerTimeScale;
            fixedDelta = Time.fixedDeltaTime * _game.playerTimeScale;
            UpdateAttack();
        }

        void UpdateAttack()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                sword.AllowHit(false);
                canAttack = true;
            }
        }

        public void OnHit(int damage)
        {
            Debug.Log("Player hitted!");
        }
    }
}