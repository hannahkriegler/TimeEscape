﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomieJr_DieBehaviour : StateMachineBehaviour
{
    private GameObject _shroomieJr;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _shroomieJr = animator.GetComponentInParent<Shroomie_JR>().gameObject;
        Debug.Log("JR died");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(_shroomieJr);
    }
}
