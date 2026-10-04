using UnityEngine;

public class ESMB_Shooting : EnemySMBase
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (Controller == null) return;
    //    if (Controller.Agent.isOnNavMesh) Controller.Agent.isStopped = true;
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Controller == null || Controller.Target == null) return;

        Controller.transform.LookAt(new Vector3(Controller.Target.position.x, Controller.Target.position.y, Controller.Target.position.z));
    }
    public override void OnAnimationEvent(string eventName)
    {
        if (eventName == "AE_OnShootFrame") SpawnProjectile();
        Controller.TriggerAttackCoolDown();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (Controller == null) return;
    //    if (Controller.Agent.isOnNavMesh) Controller.Agent.isStopped = false;        
    //}

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
    private void SpawnProjectile()
    {
        if (Controller == null || Controller.Target == null) return;

        var shootingComponent = Controller.Shooting;
        if (shootingComponent == null || shootingComponent.ProjectilePrefab == null) return;

        Vector3 spawnPos = shootingComponent.FirePoint.position;
        Quaternion rotation = Quaternion.LookRotation(Controller.Target.position - spawnPos);
        GameObject projectile = Controller.PoolManager.Spawn(shootingComponent.ProjectilePrefab, spawnPos, rotation);
        if (projectile != projectile.TryGetComponent<EnemyProjectile>(out var p))
        {
            p.Initialize(Controller.Stats.AttackDamage, Controller.PoolManager, Controller.TargetLayer);
        }
    }
}