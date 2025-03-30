using UnityEngine;

public class AshleyJumpBehaviour : StateMachineBehaviour

    {

        public override void OnStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
    {
        Debug.Log("Jump");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Stop");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Update");
    }




}
