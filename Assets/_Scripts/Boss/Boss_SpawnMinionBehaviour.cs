using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;

public class Boss_SpawnMinionBehaviour : StateMachineBehaviour
{
    public GameObject minion;
    private Transform playerPos;

    private int rndm;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Boss entered Spawn Minion State");
        rndm = Random.Range(0,2);
        for (int i = 0; i <= rndm; i++)
        {
            Instantiate(minion, animator.transform.position + Vector3.up * 0.2f,
                Quaternion.identity);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
