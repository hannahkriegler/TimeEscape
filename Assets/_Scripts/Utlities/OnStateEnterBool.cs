using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateEnterBool : StateMachineBehaviour
{
    public string boolName;
    public bool status;
    public bool resetOnExit;

    /// <summary>
    /// When the animator enters the state change paramter boolname to status
    /// </summary>
    /// <param name="animator">current animator</param>
    /// <param name="stateInfo">specified state</param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
    }

    /// <summary>
    /// When the animator leaves the state change paramter boolname to not status. (only if resetOnExit holds true)
    /// </summary>
    /// <param name="animator">current animator</param>
    /// <param name="animatorStateInfo">specified state</param>
    /// <param name="layerIndex"></param>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (resetOnExit)
            animator.SetBool(boolName, !status);
    }
}
