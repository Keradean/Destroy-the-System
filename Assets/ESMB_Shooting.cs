using System;
using UnityEngine;

public class EnemySMB_Shooting : EnemySMBase
{
    [SerializeField] private GameObject projectilePrefab;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Controller == null) return;
        if (Controller.Agent.isOnNavMesh) Controller.Agent.isStopped = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Controller == null || Controller.Target == null) return;

        Controller.transform.LookAt(new Vector3(Controller.Target.position.x, Controller.Target.position.y, Controller.Target.position.z));
    }
    public override void OnAnimationEvent(string eventName)
    {
        if (eventName == "AE_OnShoot") SpawnProjectile();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Controller == null) return;
        if (Controller.Agent.isOnNavMesh) Controller.Agent.isStopped = false;        
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
    private void SpawnProjectile()
    {
        if (Controller == null || Controller.Target == null) return;
        Vector3 spawnPos = Controller.FirePoint.position;
        Quaternion rotation = Quaternion.LookRotation(Controller.Target.position - spawnPos);
        GameObject projectile = Controller.PoolManager.Spawn(projectilePrefab, spawnPos, rotation);
        if (projectile != projectile.TryGetComponent<EnemyProjectile>(out var p))
        {
            p.Initialize(Controller.Stats.AttackDamage, Controller.PoolManager, Controller.TargetLayer);
        }
    }
}


[RequireComponent(typeof(Collider))]
public class EnemyProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxLifeTime = 5f;
    [SerializeField] private LayerMask targetLayer;

    public float damage { get; private set; }
    private float currentLifetime;
    private PoolManager poolManager;
    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }
    private void OnEnable()
    {
        currentLifetime = 0f;
    }
    public void Initialize(float damage, PoolManager pool, LayerMask mask)
    {
        this.damage = damage;
        poolManager = pool;
        targetLayer = mask;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= maxLifeTime)
        {
            Despawn();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if (other.TryGetComponent<HealthComponent>(out var health))
                health.TakeDamage(damage);
            Despawn();
        }
    }
    private void Despawn()
    {
        if (poolManager != null) poolManager.Despawn(gameObject);
        else gameObject.SetActive(false);
    }
}