using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class Cup : Enemy
{
    private float explosionTime = 1;
    private float currentTime = 0;
    
    public GameObject explodeAnimation;
    protected override void Tick()
    {
        if (enemyAI.IsInFollowDistance() && enemyAI.canMove)
        {
            animator.SetBool("run", true);
        }
    }

    protected override void Attack(GameObject target)
    {
        if (!target.CompareTag("Player")) return;
        enemyAI.canMove = false;
        animator.SetBool("run", false);
        animator.SetTrigger("explode");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(Explode());
        StartCoroutine(Hide());
    }

    IEnumerator  Explode()
    {
        List<float> flashStrengths = new List<float>();
        for (float i = 0; i <= 1f; i += .2f)
        {
            flashStrengths.Add(i);
        }

        int currentFlashStrength = 0;
        bool reachedEnd = false;
        currentTime = explosionTime + 0.45f;
        while (currentTime > 0)
        {
            yield return new WaitForEndOfFrame();
            currentTime -= Time.deltaTime * Game.instance.worldTimeScale;

            if (reachedEnd)
            {
                currentFlashStrength--;
                if (currentFlashStrength == 0) reachedEnd = false;
            }
            else
            {
                currentFlashStrength++;
                if (currentFlashStrength == flashStrengths.Count-1) reachedEnd = true;
            }
            
            
            FlashEffect(flashStrengths[currentFlashStrength]);
        }
        animator.gameObject.SetActive(false);
        
        
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(explosionTime);
        explodeAnimation.SetActive(true);
        yield return new WaitForSeconds(.2f);
        explodeAnimation.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        
        Die();
        explodeAnimation.gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    public override void OnHit(int damage, GameObject attacker, bool knockBack)
    {
        
    }
}
