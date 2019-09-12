using System;
using System.Collections;
using System.Collections.Generic;
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

        SpriteRenderer[] all_Sprites;

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

        [HideInInspector]
        public bool dead;

        public void Init(Game game)
        {
            //Setting References
            _game = game;
            rigidBody = GetComponent<Rigidbody2D>();
            sword = GetComponentInChildren<DamageCollider>();
            animator = GetComponentInChildren<Animator>();
            col = GetComponent<Collider2D>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
            all_Sprites = GetComponentsInChildren<SpriteRenderer>();

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
            Movement.KnockBack(300 + damage * 10, attacker.transform);
            currentFlashEffectTimer = flashEffectLength;
            StartCoroutine(FlashEffect());
        }

        float currentFlashEffectTimer;
        float flashEffectLength = 0.35f;
        IEnumerator FlashEffect()
        {
            while (currentFlashEffectTimer > 0)
            {         
                currentFlashEffectTimer -= Time.deltaTime * Game.instance.playerTimeScale;
                //Handle Flash Effect
                float a = flashEffectLength - currentFlashEffectTimer;
                float flashStrength = Mathf.Sin(a * Mathf.PI / flashEffectLength) * 0.8f;
                FlashEffect(flashStrength);
                yield return new WaitForEndOfFrame();
            }
            FlashEffect(0);
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

        protected void FlashEffect(float strength)
        {
            foreach (SpriteRenderer rend in all_Sprites)
            {
                rend.material.SetFloat("_flash", strength);
            }
        }

        public void GameOver()
        {
            dead = true;
            animator.Play("Die");
        }
    }
}