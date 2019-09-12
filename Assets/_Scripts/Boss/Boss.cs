using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : Enemy
{

    public GameObject spitProjectile;
    public GameObject spitStart;

    private float timeBtwDamage = 0f; // give the player time to recover before taking more damage
    private float timeToRecoverAfterSpit = 2f;
    private float timeToRecoverAfterSpawn = 5f;
    private int _rndm;
    
    [HideInInspector] 
    public Animator anim;
    [HideInInspector] 
    public bool isInAttackRange;

    [HideInInspector] public Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = Game.instance.player.transform;
        anim = GetComponent<Animator>();
        base.rb = GetComponent<Rigidbody2D>();
        player = Game.instance.player;
    }

    // Update is called once per frame
    protected override void Tick()
    {
        CheckRotation();
        CheckDistance();
        
        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
            if (!isInAttackRange)
            {
                anim.SetTrigger("walk");
            }
            else
            {
                anim.SetTrigger("idle");
            }
        }
        else
        {
            if (!isInAttackRange)
            {
                anim.SetTrigger("walk");
            }
            else
            {
                if (timeBtwDamage <= 0)
                {
                    anim.SetTrigger("spit");
                    timeBtwDamage = timeToRecoverAfterSpit;
                    
                    /*
                   _rndm = Random.Range(0, 2);
                   if (_rndm == 0)
                   {
                       anim.SetTrigger("spit");
                       timeBtwDamage = timeToRecoverAfterSpit;
                   }
                   else
                   {
                       anim.SetTrigger("spawn");
                       timeBtwDamage = timeToRecoverAfterSpawn;
                   }*/
                }
            }
        }


    }

    private void CheckDistance()
    {
        var distance = Vector3.Distance(playerPos.position, transform.position);
        isInAttackRange = !(distance > 10f);
    }

    private void CheckRotation()
    {
        var direction = (playerPos.transform.position- transform.position).normalized;
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
        anim.SetTrigger("hit");
        base.currentKnockbackLength = knockbackLength * Game.instance.worldTimeScale;
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        Knockback(damage * 500 * player.enemyKnockBackMultiplier);
        StartCoroutine(KnockbackCountdown());
        Game.instance.IncreaseTime(Game.instance.timeBonusOnHit);
        hitPoints--;
        if (hitPoints == 0)
        {
            Die();
        }

        anim.SetTrigger("spit");
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
        anim.SetTrigger("dead");
        // TODO: wait for die animation
        StartCoroutine(WaitToDie(2));
        if(hasLootDrop) DropLoot();
    }

    IEnumerator WaitToDie(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    }

    protected override void FlashEffect(float strength)
    {
        Debug.Log("Philip, Fix this pls");
    }
}
