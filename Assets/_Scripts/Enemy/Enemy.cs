using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

   public float damageAmount = 10f;
   public int HitPoints = 3;

   protected Enemy(float damageAmount, int Hitpoints)
   {
      this.damageAmount = damageAmount;
      this.HitPoints = Hitpoints;
   }

   public void GotHit()
   {
      HitPoints--;
      if (HitPoints >= 0) 
         Die();
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
}
