using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSound : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerSound.instance.playAttack();
    }
}
