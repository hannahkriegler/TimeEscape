using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class Shroomie_JR : Boss
{
    protected override void Setup()
    {
        animator = GetComponentInChildren<Animator>();
        timeBtwDamage = 0;
    }

    protected override void Tick()
    {
        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime * Game.instance.worldTimeScale;
        }
        else
        {
            
        }
        //CheckRotation();
        
    }

    protected override void AttackAnim(bool b)
    {
        return;
    }

    public override void OnHit(int damage, GameObject attacker, bool knockBack = true)
    {
        if(currentKnockbackLength>0) return;
        animator.SetTrigger("GettingHit");
        currentKnockbackLength = knockbackLength * Game.instance.worldTimeScale;
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        Knockback(damage * 250 * player.enemyKnockBackMultiplier);
        StartCoroutine(KnockbackCountdown());
        Game.instance.IncreaseTime(Game.instance.timeBonusOnHit);
        hitPoints--;
        if (hitPoints == 0)
        {
            Die();
        }

        
        //Flash
        CurrentFlashEffectTimer = flashEffectLength;
        StartCoroutine(FlashEffect());
    }

    public override void Die()
    {
        animator.SetTrigger("dead");
        if(hasLootDrop) DropLoot();    }
}
