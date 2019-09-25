using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boss : Enemy
{

    public GameObject spitProjectile;
    public GameObject spitStart;

    protected float timeBtwDamage = 0f; // give the player time to recover before taking more damage
    private float timeToRecoverAfterSpit = 2f;
    private float timeToRecoverAfterSpawn = 5f;
    private int _rndm;
   
    [HideInInspector] 
    public bool isInAttackRange;

    bool activated = false;

    public GameObject bossInfo;
    public GameObject healthbar;
    private int maxHealth;

    protected override void Setup()
    {
        base.Setup();
    }

    // Update is called once per frame
    protected override void Tick()
    {
        
        if (!activated)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 8)
            {
                activated = true;
                bossInfo.SetActive(true);
                maxHealth = hitPoints;
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
                animator.SetTrigger("walk");
            }
            else
            {
                animator.SetTrigger("idle");
            }
        }
        else
        {
            if (!isInAttackRange)
            {
                animator.SetTrigger("walk");
            }
            else
            {
                if (timeBtwDamage <= 0)
                { 
                    
                    _rndm = Random.Range(0, 2);
                   if (_rndm == 0)
                   {
                       animator.SetTrigger("spit");
                       timeBtwDamage = timeToRecoverAfterSpit;
                   }
                   else
                   {
                       animator.SetTrigger("spawn");
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

    protected void CheckRotation()
    {
        var direction = (player.transform.position- transform.position).normalized;
        if (direction.x > 0) // go right
        {
            transform.localRotation = new Quaternion(0, -180,0,0);
        }
        else
        {
            transform.localRotation = new Quaternion(0, 0,0,0);
        }
    }


    public override void OnHit(int damage, GameObject attacker, bool knockBack = true)
    {
        if(currentKnockbackLength>0) return;
        animator.SetTrigger("hit");
        currentKnockbackLength = knockbackLength * Game.instance.worldTimeScale;
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        Knockback(damage * 500 * player.enemyKnockBackMultiplier);
        StartCoroutine(KnockbackCountdown());
        Game.instance.IncreaseTime(Game.instance.timeBonusOnHit);
        hitPoints--;
        healthbar.GetComponent<Image>().fillAmount = (float) hitPoints / maxHealth;
        if (hitPoints == 0)
        {
            Die();
        }

        animator.SetTrigger("spit");

        //Flash
        currentFlashEffectTimer = flashEffectLength;
        StartCoroutine(FlashEffect());
    }


    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        IHit hit = other.GetComponent<IHit>();
        if (hit != null)
        {
            hit.OnHit(damageAmount, gameObject);
        }
    }
    
    
    /*public virtual void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        Debug.Log("Player is Not in damage range");
        isInAttackRange = false;
    }*/

    
    protected override void Die()
    {
        animator.SetTrigger("dead");
        // TODO: wait for die animation
        StartCoroutine(WaitToDie(0.5f));
        if(hasLootDrop) DropLoot();
        bossInfo.SetActive(false);
        //Unlocks time skills
        Game.instance.session.UnlockTimeSkills();
    }

    protected IEnumerator WaitToDie(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    }


    protected float currentFlashEffectTimer;
    protected float flashEffectLength = 0.35f;
    protected IEnumerator FlashEffect()
    {
        while (currentFlashEffectTimer > 0)
        {
            currentFlashEffectTimer -= Time.deltaTime * Game.instance.playerTimeScale;
            //Handle Flash Effect
            float a = flashEffectLength - currentFlashEffectTimer;
            float flashStrength = Mathf.Sin(a * Mathf.PI / flashEffectLength) * 0.8f;
            FlashEffect(flashStrength);
            yield return new WaitForEndOfFrame();
        }
        FlashEffect(0);
    }
}
