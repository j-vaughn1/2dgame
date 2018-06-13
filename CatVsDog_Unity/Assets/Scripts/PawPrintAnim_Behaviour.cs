using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PawPrintAnim_Behaviour : StateMachineBehaviour {
    private TMP_Text getTurnText(Animator animator) {
        return animator.transform.Find("TurnIndicator").GetComponentInChildren<TMP_Text>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if ((animator.GetBool(  "Cat Turn_Start"  ))) {
            getTurnText(animator).SetText("Cat's\nTurn");
        } else if ((animator.GetBool(  "Dog Turn_Start"  ))) {
            getTurnText(animator).SetText("Dog's\nTurn");
        }
        animator.SetBool("Cat Turn_Start", false);
        animator.SetBool("Dog Turn_Start", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
