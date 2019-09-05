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

        public Room assignedRoom { get; private set; }


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
