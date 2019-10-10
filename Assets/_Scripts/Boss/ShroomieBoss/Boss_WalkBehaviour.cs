using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class Boss_WalkBehaviour : StateMachineBehaviour
{
    private Transform playerPos;
    public float speed;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = Game.instance.player.transform;
        Debug.Log("Boss entered Walk State");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var direction = (playerPos.transform.position- animator.transform.position).normalized;
        
        direction.y = 0;
        animator.transform.parent.position += speed * Time.deltaTime * Game.instance.worldTimeScale * direction;

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
