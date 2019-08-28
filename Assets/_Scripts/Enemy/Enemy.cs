using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public abstract class Enemy : MonoBehaviour, IHit
    {
        public float damageAmount = 10f;
        public int HitPoints = 3;

        public Room assignedRoom { get; private set; }

        public void Attack()
        {

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
            return HitPoints;
        }

        public void OnHit(int damage)
        {
            Debug.Log(gameObject.name + " took " + damage + " damage!");
            HitPoints -= damage;
            if (HitPoints <= 0)
                Die();
        }

        public void AssignRoom(Room room)
        {
            assignedRoom = room;
        }
    }
}
