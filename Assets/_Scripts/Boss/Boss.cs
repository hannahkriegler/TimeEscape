using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour, IHit
{

    public int hitPoints; // equals life

    public int damage;

    public GameObject spitProjectile;
    public GameObject spitStart;

    private float timeBtwDamage = 0f; // give the player time to recover before taking more damage
    private float timeToRecoverAfterSpit = 4f;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
            anim.SetTrigger("idle");
        }

        CheckRotation();

        if (!isInAttackRange)
        {
            anim.SetTrigger("walk");
        }

        if (isInAttackRange && timeBtwDamage <= 0)
        {
            _rndm = Random.Range(0, 2);
            //if (_rndm == 0)
            //{
                anim.SetTrigger("spit");
                timeBtwDamage = timeToRecoverAfterSpit;
            //}
            //else
            //{
            //    anim.SetTrigger("spawn");
            //    timeBtwDamage = timeToRecoverAfterSpawn;
            //}
            
            
        }

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


    public void OnHit(int damage, GameObject attacker, bool knockBack = true)
    {
        anim.SetTrigger("hit");
        hitPoints--;
        if (hitPoints == 0)
        {
            Die();
        }
    }

    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        Debug.Log("Player is in damage range");
        isInAttackRange = true;
    }
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        Debug.Log("Player is Not in damage range");
        isInAttackRange = false;
    }

    
    private void Die()
    {
        anim.SetTrigger("dead");
        // TODO: wait for die animation
        StartCoroutine(WaitToDie(2));
    }

    IEnumerator WaitToDie(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    } 
}
