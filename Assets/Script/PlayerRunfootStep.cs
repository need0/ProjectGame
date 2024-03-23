using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunfootStep : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!PlayerVFXManager.instance.footStep.isPlaying)
        {
            PlayerVFXManager.instance.FootStep();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerVFXManager.instance.footStep.Stop();
    }
}
