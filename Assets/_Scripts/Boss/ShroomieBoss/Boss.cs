using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Boss Class for Shroomie
/// </summary>
public class Boss : Enemy
{
    public GameObject spitProjectile;
    public GameObject spitStart;
    public GameObject bossSteps;

    protected float timeBtwDamage = 0f; // give the player time to recover before taking more damage
    private float timeToRecoverAfterSpit = 2f;
    private float timeToRecoverAfterSpawn = 4f;
    private int _rndm;
   
    [HideInInspector] 
    public bool isInAttackRange;

    public bool setUp = false;
    
    private bool _activated = false;
    private bool _isDead = false;
    private int _maxHealth;

    /// <summary>
    /// Shroomie is build upon a State machine that is controlled via triggers. They can be found in shroomies animator
    /// </summary>
    protected override void Tick()
    {
        if(_isDead) return;
        
        if (!_activated)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 8 && setUp)
            {
                _activated = true;
                Game.instance.bossHealthBar.Activate(Healthbar.BossType.SHROOMIE);
                _maxHealth = hitPoints;
                SoundManager.instance.PlayBossAmbient();
            }
                
            else return;
        }
        
        CheckRotation();
        CheckDistance();
        
        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
            if (!isInAttackRange)
            {
                animator.SetTrigger(Walk);
            }
            else
            {
                animator.SetTrigger(Idle);
            }
        }
        else
        {
            if (!isInAttackRange)
            {
                animator.SetTrigger(Walk);
            }
            else
            {
                if (timeBtwDamage <= 0)
                { 
                    
                    _rndm = Random.Range(0, 2);
                   if (_rndm == 0)
                   {
                       animator.SetTrigger(Spit);
                       timeBtwDamage = timeToRecoverAfterSpit;
                   }
                   else
                   {
                       animator.SetTrigger(Spawn);
                       timeBtwDamage = timeToRecoverAfterSpawn;
                       Knockback(10 * 500 * player.enemyKnockBackMultiplier);
                   }
                }
            }
        }
    }

    private void CheckDistance()
    {
        var distance = Vector3.Distance(player.transform.position, transform.position);
        isInAttackRange = !(distance > 10f);
    }

    private void CheckRotation()
    {
        var direction = (player.transform.position- transform.position).normalized;
        transform.localRotation = !(direction.x > 0) ? new Quaternion(0, 0, 0, 0) : new Quaternion(0, -180, 0, 0);
    }


    public override void OnHit(int damage, GameObject attacker, bool knockBack)
    {
        if(_isDead) return;
        if(currentKnockbackLength>0) return;
        animator.SetTrigger(Hit);
        currentKnockbackLength = knockbackLength * Game.instance.worldTimeScale;
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        Knockback(damage * 500 * player.enemyKnockBackMultiplier);
        StartCoroutine(KnockbackCountdown());
        Game.instance.IncreaseTime(Game.instance.timeBonusOnHit);
        hitPoints--;
        Game.instance.bossHealthBar.UpdateBar(hitPoints, _maxHealth);
        if (hitPoints == 0)
        {
            animator.SetTrigger(Dead);
            _isDead = true;
            return;
        }

        animator.SetTrigger("spit");

        //Flash
        CurrentFlashEffectTimer = flashEffectLength;
        StartCoroutine(FlashEffect());
    }


    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        IHit hit = other.GetComponent<IHit>();
        hit?.OnHit(damageAmount, gameObject);
    }
    
    
    public override void Die()
    {
        if(hasLootDrop) DropLoot();
        Game.instance.bossHealthBar.DeActivate();
        gameObject.SetActive(false);
        bossSteps.SetActive(true);
        SoundManager.instance.PlayLevel2Ambient();
    }

    protected IEnumerator WaitToDie(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    }


    protected float CurrentFlashEffectTimer;
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Spit = Animator.StringToHash("spit");
    private static readonly int Spawn = Animator.StringToHash("spawn");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int Dead = Animator.StringToHash("dead");

    protected IEnumerator FlashEffect()
    {
        while (CurrentFlashEffectTimer > 0)
        {
            CurrentFlashEffectTimer -= Time.deltaTime * Game.instance.playerTimeScale;
            //Handle Flash Effect
            float a = flashEffectLength - CurrentFlashEffectTimer;
            float flashStrength = Mathf.Sin(a * Mathf.PI / flashEffectLength) * 0.8f;
            FlashEffect(flashStrength);
            yield return new WaitForEndOfFrame();
        }
        FlashEffect(0);
    }
}
