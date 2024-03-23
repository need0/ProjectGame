using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump_Sound : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerSound.instance.playJumping();
    }

}
