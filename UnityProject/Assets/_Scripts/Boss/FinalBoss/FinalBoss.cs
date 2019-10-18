using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class FinalBoss : MonoBehaviour, IHit, ITimeTravel
    {
        private Game _game;

        public Rigidbody2D rigidBody { get; private set; }

        public Collider2D col { get; private set; }

        public TrailRenderer trailRenderer { get; private set; }

        public FinalBoss_AnimHook anim_hook { get; private set; }
        public Animator animator { get; private set; }

        public DamageCollider sword { get; private set; }

        SpriteRenderer[] all_Sprites;

        [Header("References")]
        public LayerMask groundLayerCheck;
        public Transform groundCheck;
        public GameObject fireBall;

        [Header("Settings")]
        public float moveSpeed = 300;
        public float jumpVelocity = 10;
        public float fallMultiplier = 2.5f;
        public float gravityMultiplier = 2f;
        public float dashSpeed = 500;

        [Header("Stats")]
        public int maxHealth = 30;
        public int curHealth;
        private int savedHealth;
        public float attackDistance = 2.8f;
        public int attackDamage = 10;
        public int fireBallDamage = 10;

        [Header("States")]
        public bool canAttack;
        public bool grounded;
        public bool dashActive;
        public bool knockBack;

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
        bool dashEffect;
        Player player;


        //Dash
        Vector3 targetPosition;
        float moveTimer;
        float knockBackTimer;
        float moveImpulseCD;
        float fireBallCD;

        //Prevents the boss from being stunlocked by the player. 
        //After a specified damage amount the boss escapes from the hit animation and executes a counter attack
        float revengeValue;

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

            moveImpulseCD = 1.5f;
            fireBallCD = 2.0f;
        }

        /// <summary>
        /// Displays healthbar of the boss. Makes the boss having aggro on the player
        /// </summary>
        public void ActivateBoss()
        {
            activated = true;
            _game.bossHealthBar.Activate(Healthbar.BossType.PROKRASTINATION);
            SoundManager.instance.PlayBossAmbient();
        }

        /// <summary>
        /// Hides healthbar and stops the boss having aggro on the player
        /// </summary>
        public void DeactivateBoss()
        {
            activated = false;
            _game.bossHealthBar.DeActivate();
        }

        private void Update()
        {
            if (!activated)
                return;

            if (dead)
                return;

            delta = Time.deltaTime * _game.worldTimeScale;
            fixedDelta = Time.fixedDeltaTime * _game.worldTimeScale;

            if (curHealth <= 0)
            {
                dead = true;
                Die();
            }

            UpdateGrounded();

            if (knockBack)
            {
                if (knockBackTimer < 0.5f)
                {
                    knockBackTimer += delta;
                    return;
                }
                else
                    knockBack = false;
            }

            MoveUpdate();
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

        /// <summary>
        /// Updates Attacks of the boss. Handles Sword Attack and Fireball. Prevents spamming of skills. 
        /// Triggers a Movement Action after an attack (with delay)
        /// </summary>
        void MeleeUpdate()
        {
            float dist = Vector2.Distance(player.transform.position, transform.position);
            if (dist < attackDistance)
            {
                if (responseAttackTimer > 0)
                    responseAttackTimer -= delta;
                else
                    Attack();
            }
            else if (dist < attackDistance * 3 && fireBallCD <= 0)
            {
                CastFireBall();
            }
            else
            {
                if (moveImpulseCD > 0.0f)
                    moveImpulseCD -= Time.deltaTime;
                else
                    MoveImpulse();
                attackCooldown = 0;
                if (!grounded)
                    responseAttackTimer = 0.4f;
                else
                    responseAttackTimer = 1.2f;
                fireBallCD -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Executes an sowrd attack
        /// </summary>
        /// <returns></returns>
        bool Attack()
        {
            if (!canAttack)
                return false;

            if (!grounded)
                animator.Play("Attack");
            else
                animator.Play("Jump_Attack");

            canAttack = false;
            revengeValue = 0;
            SoundManager.instance.PlaySlash();
            dashActive = false;
            moveImpulseCD = 0.5f;
            return true;
        }

        /// <summary>
        /// Plays the fireball animation and prepares boss for fireball
        /// </summary>
        void CastFireBall()
        {
            if (!canAttack)
                return;

            animator.Play("Cast");
       
            fireBallCD = 8.0f;
            dashActive = false;
            moveImpulseCD = 0.5f;
            canAttack = false;
            revengeValue = 0;
        }

        /// <summary>
        /// Actually spawns the fireball
        /// </summary>
        public void TriggerFireBall()
        {
            GameObject fb = Instantiate(fireBall);
            Vector2 dir = facingRight ? Vector2.right : Vector2.left;
            fb.transform.position = transform.position + (Vector3)dir * 1.2f + transform.up * 0.5f;
            float dist = Vector2.Distance(player.transform.position, transform.position);
            fb.GetComponent<FireBall>().Shoot(fireBallDamage, dir, false, dist * 0.8f + 3);
        }

        public bool IsInteracting()
        {
            return animator.GetBool("isInteracting");
        }

        #region Movement

        void MoveUpdate()
        {
            if (!IsInteracting())
            {
                float dot = Vector2.Angle((player.transform.position - transform.position).normalized, Vector2.right);
                FlipCharacter(dot < 90);
            }

            Vector2 rigidbodyCurVelocity = rigidBody.velocity;

            Vector2 moveVel = Vector2.zero;

            if (dashActive)
            {
                sword.AllowHit(false);
                Vector2 dir = (targetPosition - transform.position).normalized;

                moveTimer += Time.deltaTime* 2.0f;

                if (moveTimer < 0.4f)
                   targetPosition = player.transform.position;

                float curDashSpeed = Mathf.Lerp(dashSpeed, 0, moveTimer);

                moveVel = dir * curDashSpeed;

                moveVel.y = Mathf.Clamp(moveVel.y, -40.0f, 40.0f);

                if (Vector2.Distance(targetPosition, transform.position) < 0.2f || moveTimer > 1.0f)
                {
                    dashActive = false;
                    moveImpulseCD = 0.4f;
                    if (!grounded)
                        responseAttackTimer = 0.5f;
                    else
                        responseAttackTimer = 1.2f;
                }

                if (!trailRenderer.enabled && dashEffect)
                {
                    trailRenderer.enabled = true;
                    trailRenderer.Clear();
                }

            }
            else
            {
                trailRenderer.enabled = false;
            }

            rigidBody.velocity = moveVel * delta + Vector2.up * rigidbodyCurVelocity.y;

            float walkSpeed = Mathf.Abs(rigidBody.velocity.x) * 0.1f;
            walkSpeed = Mathf.Clamp01(walkSpeed);
            animator.SetFloat("MoveSpeed", walkSpeed);

        }


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

        void MoveImpulse()
        {
            if (dashActive)
                return;

            targetPosition = player.transform.position;
            SoundManager.instance.PlayDash();
            dashActive = true;
            float dist = Vector2.Distance(player.transform.position, transform.position);
            dashSpeed = 350 + dist * 150;
            Vector2 dashDir = player.transform.position - transform.position;
            dashEffect = false;
            if (dashDir.y > 1.0f)
                animator.Play("Jump");
            else
            if (dist > 2.0f)
            {
                animator.Play("Dash");
                dashEffect = true;
            }

            moveTimer = 0;
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
            if (dead)
                return;
            
            if(!activated)
                ActivateBoss();
            
            SoundManager.instance.PlayHit();
            Debug.Log("Final Boss hitted!");
            TakeDamage(damage);

            //TODO Damage Threshold System
            if (!IsInteracting() && revengeValue < 1)
            {
                revengeValue += damage;
                animator.Play("Hit");
                KnockBack(150 + damage * 5, attacker.transform);
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

        public void KnockBack(float strength, Transform attacker)
        {
            knockBack = true;
            knockBackTimer = 0;
            rigidBody.velocity = Vector2.zero;
            Vector3 dir = transform.position - attacker.transform.position;
            dir.Normalize();
            if (dir.x < 0)
                dir -= Vector3.right * 0.2f;
            else
                dir += Vector3.right * 0.2f;
            rigidBody.AddForce(strength * dir, ForceMode2D.Force);
        }

        void Die()
        {
            animator.Play("Die");
            StartCoroutine(DieRoutine());
        }

        IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
            _game.bossHealthBar.DeActivate();
            Game.instance.Won();
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

        public void HandleTimeStamp()
        {
            savedHealth = curHealth;
        }

        public void HandleTimeTravel()
        {
            DeactivateBoss();
            curHealth = savedHealth;
        }
    }
}