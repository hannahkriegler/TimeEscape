using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public abstract class Enemy : MonoBehaviour, IHit
    {
        public int damageAmount = 10;
        public int hitPoints = 3;

        Vector3 savePos;
        int saveHitPoints;

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
           
        }

        protected virtual void Tick()
        {

        }

        public void Attack()
        { }

        public void FollowPlayer()
        {
            EnemyAI enemyAi = gameObject.GetComponent<EnemyAI>();
            enemyAi.canMove = enemyAi.IsInFollowDistance();
        }

        private void Die()
        {
            Debug.Log("You killed an Enemy!");
            gameObject.SetActive(false);
            assignedRoom.NotifyEnemyDied(this);
        }

        public void HandleTimeStamp()
        {
            savePos = transform.position;
            saveHitPoints = hitPoints;
        }

        public void HandleTimeTravel()
        {
            transform.position = savePos;
            hitPoints = saveHitPoints;
            if(hitPoints <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }


        public float GetDamageAmount()
        {
            return damageAmount;
        }

        public int GetHitPoints()
        {
            return hitPoints;
        }

        public void OnHit(int damage)
        {
            Debug.Log(gameObject.name + " took " + damage + " damage!");
            hitPoints -= damage;
            if (hitPoints <= 0)
                Die();
        }

        public void AssignRoom(Room room)
        {
            assignedRoom = room;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {    
            if(!other.CompareTag("Player")) return;
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            {
                hit.OnHit(damageAmount);
                
            }
        }
    }
}
