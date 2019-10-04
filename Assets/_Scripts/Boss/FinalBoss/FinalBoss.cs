using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class FinalBoss : MonoBehaviour, IHit
    {
        private Game _game;
        
        public Rigidbody2D rigidBody  { get; private set; }
        
        public Collider2D col { get; private set; }

        public TrailRenderer trailRenderer { get; private set; }

        public FinalBoss_AnimHook anim_hook { get; private set; }
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

        [Header("Stats")]
        public int maxHealth = 30;
        public int curHealth;
        public float attackDistance = 2.8f;
        public int attackDamage = 10;

        [Header("States")]
        public bool canAttack;
        public bool grounded;

        [Header("Misc")]
        public bool facingRight;
        public float responseAttackTimer;
        public float attackCooldown;
        public float damageThreshold;

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

        bool activated;

        Player player;

      
        public void Start()
        {
            //Setting References
            _game = Game.instance;
            rigidBody = GetComponent<Rigidbody2D>();
            sword = GetComponentInChildren<DamageCollider>();
            anim_hook = GetComponentInChildren<FinalBoss_AnimHook>();
            animator = GetComponentInChildren<Animator>();
            col = GetComponent<Collider2D>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
            all_Sprites = GetComponentsInChildren<SpriteRenderer>();

            canAttack = true;
            anim_hook.Init(this);
            SetupTrailRenderer();

            curHealth = maxHealth;

            player = _game.player;

            ActivateBoss();
        }

        public void ActivateBoss()
        {
            activated = true;
            _game.bossHealthBar.Activate("Prokrastination");
        }

        private void Update()
        {
            if (!activated)
                return;

            delta = Time.deltaTime * _game.worldTimeScale;
            fixedDelta = Time.fixedDeltaTime * _game.worldTimeScale;

            UpdateGrounded();
            MeleeUpdate();
        }

        private void LateUpdate()
        {
            if (!IsInteracting())
            {
                if (attackCooldown > 0)
                {
                    sword.AllowHit(false);
                    attackCooldown -= delta;
                }
                else
                    canAttack = true;
            }
            else
            {
                attackCooldown = 0.4f;
            }
        }

        void MeleeUpdate()
        {
            if(Vector2.Distance(player.transform.position, transform.position) < attackDistance)
            {
                if (responseAttackTimer > 0)
                    responseAttackTimer -= delta;
                else
                    Attack();
            }
            else
            {
                responseAttackTimer = 0.2f;
            }
        }

        bool Attack()
        {
            if (!canAttack)
                return false;

            float dot = Vector2.Angle((player.transform.position - transform.position).normalized, Vector2.right);
            FlipCharacter(dot < 90);

            if (!grounded)
                animator.Play("Attack");
            else
                animator.Play("Jump_Attack");

            canAttack = false;
            SoundManager.instance.PlaySlash();
            return true;
        }

        
        public bool IsInteracting()
        {
            return animator.GetBool("isInteracting");
        }

        #region Movement
        void UpdateGrounded()
        {
            grounded = false;

            for (int i = 0; i < 10; i++)
            {
                Vector2 rayOrigin = groundCheck.position;
                rayOrigin += Vector2.right * (i - 5) * 0.025f;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.1f, groundLayerCheck);

                if (hit)
                {
                    Debug.DrawRay(rayOrigin, Vector2.down * 0.1f, Color.red);
                    grounded = true;
                    break;
                }
            }
        }

        void FlipCharacter(bool right = true)
        {
            if (facingRight && right)
                return;

            if (!facingRight && !right)
                return;

            facingRight = !facingRight;
            Vector3 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
        }
        #endregion

        #region TakeDamage
        void TakeDamage(int damage)
        {
            curHealth -= damage;
            curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
            _game.bossHealthBar.UpdateBar(curHealth, maxHealth);
        }


        public void OnHit(int damage, GameObject attacker, bool knockBack)
        {
            SoundManager.instance.PlayHit();
            Debug.Log("Final Boss hitted!");
            TakeDamage(damage);

            //TODO Damage Threshold System
            if (!IsInteracting())
            {
                animator.CrossFade("Hit", 0.2f);
            }
         
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

        protected void FlashEffect(float strength)
        {
            foreach (SpriteRenderer rend in all_Sprites)
            {
                rend.material.SetFloat("_flash", strength);
            }
        }

        #endregion

        #region Effects
        void SetupTrailRenderer()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0.0f, 0.8f);
            curve.AddKey(0.5f, 1.0f);
            curve.AddKey(1.0f, 0.2f);
            trailRenderer.widthCurve = curve;
            trailRenderer.enabled = false;
        }
        #endregion
    }
}