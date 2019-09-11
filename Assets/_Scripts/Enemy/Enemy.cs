using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TE
{
    public abstract class Enemy : MonoBehaviour, IHit
    {
        
        public int damageAmount = 10;
        public int hitPoints = 3;
        public bool hasLootDrop = false;
        public Loot.LootTypes lootTypes;
        
        [HideInInspector]
        public Transform player;

        // Knockbacks
        private float currentKnockbackLength = 0f;
        public float knockbackLength = 1;
        protected float attackKnockback = 3f;

        private Vector3 savePos;
        private int _saveHitPoints;

        public Room assignedRoom { get; private set; }

        public Rigidbody2D rb { get; private set; }
        public Animator animator { get; private set; }

        private void Awake()
        {
            assignedRoom = GetComponentInParent<Room>();
            if (assignedRoom != null)
             assignedRoom.AddEnemyToRoom(this);
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            Setup();   
        }

        private void Update()
        {
            Tick();
        }

        protected virtual void Setup()
        {
            player = Game.instance.player.transform;
            
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
                Vector2 knockbackDirection = (transform.position  - target.transform.position ).normalized *attackKnockback; 
                gameObject.GetComponent<Rigidbody2D>().velocity = knockbackDirection;

            }
        }

        protected void AttackAnim(bool b)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                Debug.Log("agro");
                Animator anim = gameObject.GetComponent<Animator>();
                anim.SetBool("agro", b);
                //anim.CrossFade("agro", 0.2f);
            }
        }

        public void FollowPlayer()
        {
            EnemyAI enemyAi = gameObject.GetComponent<EnemyAI>();
            enemyAi.canMove = enemyAi.IsInFollowDistance();
        }

        protected virtual void Die()
        {
            Debug.Log("You killed an Enemy!");
            gameObject.SetActive(false);
            assignedRoom.NotifyEnemyDied(this);
            if(hasLootDrop) DropLoot();
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
            if(hitPoints <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void OnHit(int damage, GameObject attacker, bool knockBack)
        {
            if(currentKnockbackLength>0) return;
            if (animator != null)
            {
                Animator anim = gameObject.GetComponent<Animator>();
                anim.CrossFade("hit", 0.2f);
            }
            currentKnockbackLength = knockbackLength * Game.instance.worldTimeScale;
            Debug.Log(gameObject.name + " took " + damage + " damage!");
            Knockback(damage);
            StartCoroutine(KnockbackCountdown());
            hitPoints -= damage;
            Game.instance.IncreaseTime(Game.instance.timeBonusOnHit);
            if (hitPoints <= 0)
                Die();
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {    
            Attack(other.gameObject);
        }

        protected virtual void Knockback(int damage)
        {
            Player playerRef = player.GetComponent<Player>();
            rb.velocity = Vector2.zero;
            Vector3 dir = transform.position - player.transform.position;
            rb.AddForce(500 * damage * playerRef.enemyKnockBackMultiplier * dir.normalized, ForceMode2D.Force);
        }
        
        public float GetDamageAmount()
        {
            return damageAmount;
        }

        public int GetHitPoints()
        {
            return hitPoints;
        }

        IEnumerator KnockbackCountdown()
        {
            while (currentKnockbackLength > 0)
            {
                yield return new WaitForEndOfFrame();
                currentKnockbackLength -= Time.deltaTime * Game.instance.worldTimeScale;
            }
        }
    }
    
    
}
