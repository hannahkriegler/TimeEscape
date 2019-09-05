﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateEnterBool : StateMachineBehaviour
{
    public string boolName;
    public bool status;
    public bool resetOnExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (resetOnExit)
            animator.SetBool(boolName, !status);
    }
}
