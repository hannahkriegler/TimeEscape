using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpitBehaviour : StateMachineBehaviour
{
    public GameObject boss;
    private Boss_Projectile spit;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Boss entered Spit State");
        boss = GameObject.FindGameObjectWithTag("Boss");

        spit = boss.GetComponent<Boss>().spitProjectile.GetComponent<Boss_Projectile>();
        spit.enabled = true;
        spit.GetComponentInChildren<SpriteRenderer>().enabled = true;
        spit.GetComponent<Collider2D>().enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spit.enabled = false;
    }
}
