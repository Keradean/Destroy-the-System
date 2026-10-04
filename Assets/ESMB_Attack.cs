using UnityEngine;

public class ESMB_Attack : EnemySMBase
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Controller == null || Controller.Target == null) return;

        Controller.transform.LookAt(new Vector3(Controller.Target.position.x, Controller.Target.position.y, Controller.Target.position.z));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    public override void OnAnimationEvent(string eventName)
    {
        if (eventName == "AE_OnHitFrame" || eventName == "AE_OnScanSweep")
        {
            if (Controller != null || Controller.Attack != null)
                Controller.Attack.TryHitTarget(Controller.Target, Controller.Stats.MeleeRange, Controller.Stats.AttackDamage);
            Controller.TriggerAttackCoolDown();
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
