using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHit
{

   public float damageAmount = 10f;
   public int HitPoints = 3;

   protected Enemy(float damageAmount, int Hitpoints)
   {
      this.damageAmount = damageAmount;
      this.HitPoints = Hitpoints;
   }
   public void Attack()
   {
      
   }

   private void Die()
   {
      Debug.Log("You killed an Enemy!");
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
        HitPoints -= damage;
        if (HitPoints <= 0)
            Die();
    }
}
