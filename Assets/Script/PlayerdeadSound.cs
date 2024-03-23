using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerdeadSound : StateMachineBehaviour
{
    public override void OnStateEnter(
    Animator animator, AnimatorStateInfo stateInfo,
    int layerIndex)
    {
        PlayerSound.instance.playDead();
    }
}
