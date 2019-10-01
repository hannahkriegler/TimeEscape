using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TE
{
    public abstract class Enemy : MonoBehaviour, IHit, ITimeTravel
    {

        public int damageAmount = 10;
        public int hitPoints = 3;
        public bool hasLootDrop = false;
        public Loot.LootTypes lootTypes;

        [HideInInspector]
        public Player player;

        SpriteRenderer[] all_Sprites;

        // Knockbacks
        protected float currentKnockbackLength = 0f;
        public float knockbackLength = 1;
        protected float attackKnockback = 3f;

        private Vector3 savePos;
        private int _saveHitPoints;

        public Room assignedRoom { get; private set; }

        public Rigidbody2D rb { get; set; }
        public Animator animator { get; protected set; }

        public EnemyAI enemyAI { get; protected set; }

        public Collider2D[] colliders { get; protected set; }

        bool died;
        private void Awake()
        {
            assignedRoom = GetComponentInParent<Room>();
            if (colliders == null)
                colliders = GetComponents<Collider2D>();
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();
            if(all_Sprites == null)
                all_Sprites = GetComponentsInChildren<SpriteRenderer>();
            if (assignedRoom != null)
                assignedRoom.AddEnemyToRoom(this);
        }

        private void Start()
        {

            animator = GetComponent<Animator>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            player = Game.instance.player;
            enemyAI = GetComponent<EnemyAI>();
            Setup();
        }

        private void Update()
        {
            Tick();
        }

        protected virtual void Setup()
        {


        }

        protected void DropLoot()
        {
            Debug.Log("Dropped Loot");
            if (hasLootDrop)
            {
                switch (lootTypes)
                {
                    case Loot.LootTypes.Time:
                        Instantiate(Resources.Load("BonusTime") as GameObject, transform.position, Quaternion.identity);
                        break;
                    case Loot.LootTypes.Zeitsplitter:
                        Instantiate(Resources.Load("Zeitsplitter") as GameObject, transform.position, Quaternion.identity);
                        break;
                }
            }
        }


        protected virtual void Tick()
        {

        }

        protected virtual void Attack(GameObject target)
        {
            if (!target.CompareTag("Player")) return;

            //AttackAnim();
            IHit hit = target.GetComponent<IHit>();
            if (hit != null)
            {
                hit.OnHit(damageAmount, gameObject);
                AttackKnockback();
                Vector2 knockbackDirection = (transform.position - target.transform.position).normalized * attackKnockback;
                gameObject.GetComponent<Rigidbody2D>().velocity = knockbackDirection;

            }
        }

        protected virtual void AttackAnim(bool b)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                Animator anim = gameObject.GetComponent<Animator>();
                anim.SetBool("agro", b);

            }
        }

        public void FollowPlayer()
        {
            EnemyAI enemyAi = gameObject.GetComponent<EnemyAI>();
            enemyAi.canMove = enemyAi.IsInFollowDistance();
        }

        public virtual void Die()
        {
            if (died)
                return;

            Debug.Log("You killed an Enemy!");
            if (enemyAI != null)
                enemyAI.canMove = false;
            died = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            SetColliderStatus(false);
            StartCoroutine(DieRoutine());
        }

        public void HandleTimeStamp()
        {
            savePos = transform.position;
            _saveHitPoints = hitPoints;
        }

        public void HandleTimeTravel()
        {
            transform.position = savePos;
            hitPoints = _saveHitPoints;
            if (hitPoints <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                foreach (SpriteRenderer rend in all_Sprites)
                {
                    rend.material.SetFloat("_disolve", 0);
                }
                died = false;
            }
            currentKnockbackLength = 0;
            SetColliderStatus(true);
            if (enemyAI != null)
                enemyAI.canMove = true;
        }

        public virtual void OnHit(int damage, GameObject attacker, bool knockBack)
        {
            if (died) return;
            if (currentKnockbackLength > 0) return;
            if (animator != null)
            {
                animator.CrossFade("hit", 0.2f);
            }
            currentKnockbackLength = knockbackLength * Game.instance.worldTimeScale;
            Debug.Log(gameObject.name + " took " + damage + " damage!");
            Knockback(damage * 500 * player.enemyKnockBackMultiplier);
            StartCoroutine(KnockbackCountdown());
            hitPoints -= damage;
            Game.instance.IncreaseTime(Game.instance.timeBonusOnHit);
            SoundManager.instance.PlayHit();
            if (hitPoints <= 0)
                Die();
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            Attack(other.gameObject);
        }

        protected virtual void Knockback(float strength)
        {
            rb.velocity = Vector2.zero;
            Vector3 dir = transform.position - player.transform.position;
            rb.AddForce(strength * dir.normalized, ForceMode2D.Force);
        }

        protected virtual void AttackKnockback()
        {
            bool right = player.transform.position.x < transform.position.x;
            rb.AddForce(200 * (right ? transform.right : -transform.right), ForceMode2D.Force);
            rb.AddForce(80 * transform.up, ForceMode2D.Force);
        }

        public float GetDamageAmount()
        {
            return damageAmount;
        }

        public int GetHitPoints()
        {
            return hitPoints;
        }

        protected float flashEffectLength = 0.35f;

        protected IEnumerator KnockbackCountdown()
        {
            while (currentKnockbackLength > 0)
            {
                yield return new WaitForEndOfFrame();
                currentKnockbackLength -= Time.deltaTime * Game.instance.worldTimeScale;

                //Handle Flash Effect
                float a = 1 - currentKnockbackLength / knockbackLength;
                float flashStrength = 0;
                if (a <= flashEffectLength)
                    flashStrength = Mathf.Sin(a * Mathf.PI / flashEffectLength) * 0.8f;
                FlashEffect(flashStrength);
            }

        }

        public virtual bool IsDead()
        {
            return hitPoints <= 0;
        }

        protected void FlashEffect(float strength)
        {
            foreach (SpriteRenderer rend in all_Sprites)
            {
                rend.material.SetFloat("_flash", strength);
            }
        }


        float dieTimer;
        IEnumerator DieRoutine()
        {
            dieTimer = 0;
            SoundManager.instance.PlayDie();
            while (dieTimer < 1.2f)
            {
                foreach (SpriteRenderer rend in all_Sprites)
                {
                    rend.material.SetFloat("_disolve", dieTimer / 1.2f);
                }
                dieTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            gameObject.SetActive(false);
            assignedRoom.NotifyEnemyDied(this);
            if (hasLootDrop) DropLoot();
        }

        void SetColliderStatus(bool newStatus)
        {
            if (colliders == null)
                return;
            foreach (Collider2D col in colliders)
            {
                col.enabled = newStatus;
            }
        }
    }


}
