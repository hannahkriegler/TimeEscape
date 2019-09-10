using System;
using UnityEngine;

namespace TE
{
    public class Player : MonoBehaviour, IHit
    {
        private Game _game;
        
        public Movement Movement { get; private set; }
        public CombatMelee CombatMelee { get; private set; }
        public TimeSkills TimeSkills { get; private set; }
        public CombatSkill CombatSkill { get; private set; } 
        public Rigidbody2D rigidBody  { get; private set; }
        
        public Collider2D col { get; private set; }

        public TrailRenderer trailRenderer { get; private set; }

        public Animator animator { get; private set; }

        public DamageCollider sword  { get; private set; }
        [Header("References")]
        public LayerMask groundLayerCheck;
        public Transform groundCheck;
        
        [Header("Settings")]
        public float moveSpeed = 300;
        public float jumpVelocity = 10;
        public float fallMultiplier = 2.5f;
        public float gravityMultiplier = 2f;
        public float dashVelocity = 500;
        

        [Header("States")]
        public bool canAttack;
        
        public float delta { get; private set; }
        public float fixedDelta { get; private set; }
        
        // Gem stuff modifiers
        [HideInInspector]
        public int damageModifier = 1;
        [HideInInspector]
        public float takenDamageModifier = 1;
        [HideInInspector]
        public float skillCostModifier = 1;
        [HideInInspector]
        public float enemyKnockBackMultiplier = 1;

        public void Init(Game game)
        {
            //Setting References
            _game = game;
            rigidBody = GetComponent<Rigidbody2D>();
            sword = GetComponentInChildren<DamageCollider>();
            animator = GetComponentInChildren<Animator>();
            col = GetComponent<Collider2D>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
           
            
            //Init Subsystems
            Movement = new Movement(this, _game);
            TimeSkills = new TimeSkills(this, _game);
            CombatMelee = new CombatMelee(this, _game);
            CombatSkill = new CombatSkill(this, _game);
            canAttack = true;
            SetupTrailRenderer();
        }

        private void Update()
        {
            delta = Time.deltaTime * _game.playerTimeScale;
            fixedDelta = Time.fixedDeltaTime * _game.playerTimeScale;
            UpdateAttack();
        }

        void UpdateAttack()
        {
            if (!IsInteracting())
            {
                CombatMelee.AllowAttacking();
            }
        }

  
        public bool IsInteracting()
        {
            return animator.GetBool("isInteracting");
        }


        public void OnHit(int damage, GameObject attacker, bool knockBack)
        {
            Debug.Log("Player hitted!");
            float f = damage * takenDamageModifier;
            _game.DecreaseTime(f);
            animator.CrossFade("Hit", 0.2f);
            Movement.KnockBack(damage * 30, attacker.transform);
        }


        void SetupTrailRenderer()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0.0f, 0.8f);
            curve.AddKey(0.5f, 1.0f);
            curve.AddKey(1.0f, 0.2f);
            trailRenderer.widthCurve = curve;
            trailRenderer.enabled = false;
        }
    }
}