using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class ShroomieJr_WalkBehaviour : StateMachineBehaviour
{
    private Transform playerPos;
    public float maxSpeed;
    public float minSpeed;
    private float speed;

    private GameObject shroomieJR;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        speed = Random.Range(maxSpeed, minSpeed);
        shroomieJR = animator.GetComponentInParent<Shroomie_JR>().gameObject;
        playerPos = Game.instance.player.transform;
        Debug.Log("shroomie: " + shroomieJR);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("JR walks");
        var direction = (playerPos.transform.position- shroomieJR.transform.position).normalized;
        
        direction.y = 0;
        shroomieJR.transform.position += speed * Time.deltaTime * Game.instance.worldTimeScale * direction;

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
