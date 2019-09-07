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
        public float knockbackLength;
        protected float attackKnockback = 3f;

        private Vector3 savePos;
        private int _saveHitPoints;

        public Room assignedRoom { get; private set; }

        private void Start()
        {
            Setup();   
        }

        private void Update()
        {
            Tick();
        }

        protected virtual void Setup()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
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
            
            IHit hit = target.GetComponent<IHit>();
            if (hit != null)
            {
                hit.OnHit(damageAmount);
                Vector2 knockbackDirection = (transform.position  - target.transform.position ).normalized *attackKnockback; 
                gameObject.GetComponent<Rigidbody2D>().velocity = knockbackDirection;

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

        public virtual void OnHit(int damage)
        {
            if(currentKnockbackLength>0) return;
            currentKnockbackLength = knockbackLength * Game.instance.worldTimeScale;
            Debug.Log(gameObject.name + " took " + damage + " damage!");
            Knockback();
            StartCoroutine(KnockbackCountdown());
            hitPoints -= damage;
            Game.instance.IncreaseTime(Game.instance.timeBonusOnHit);
            if (hitPoints <= 0)
                Die();
        }

        public void AssignRoom(Room room)
        {
            assignedRoom = room;
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {    
            Attack(other.gameObject);
        }

        protected virtual void Knockback()
        {
            var sword = player.GetComponent<Player>().sword;
            Vector2 knockbackDirection = (transform.position  - player.transform.position ).normalized *sword.knockback; 
            gameObject.GetComponent<Rigidbody2D>().velocity = knockbackDirection;
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
            yield return new WaitForSeconds(currentKnockbackLength * Game.instance.worldTimeScale);
            currentKnockbackLength = 0;
        }
    }
    
    
}
