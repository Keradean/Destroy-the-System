using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RuntimeEnemyStats))]
[RequireComponent(typeof(HealthComponent))]
public class EnemyControllerBase : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");


    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }
    public RuntimeEnemyStats Stats { get; private set; }
    public HealthComponent Health { get; private set; }
    public Transform Target { get; private set; }
    public PoolManager PoolManager { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Stats = GetComponent<RuntimeEnemyStats>();
        Health = GetComponent<HealthComponent>();
        
    }
    //TODO Enable and Disable after Components are written and wiring can begin
    private void OnEnable()
    {
        Health.OnDeath += HandleDeath;
        if (Agent != null && Agent.isOnNavMesh)
        {
            Agent.isStopped = false;
        }

    }
    private void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
        if (Agent != null && Agent.isActiveAndEnabled && Agent.isOnNavMesh)
        {
            Agent.isStopped = true;
            Agent.ResetPath();
        }
    }
    public void AE_OnScanSweep()
    {
        // Place scanner logic here, or pass it to a ScanningComponent
        Debug.Log($"[{gameObject.name}] AE_OnScanSweep triggered!");
    }
    public void InitFromSpawn(EnemyDataSO data, float stageMultiplier, Vector3 validPosition, PoolManager poolManager)
    {
        PoolManager = poolManager;
        Stats.SetupStats(data, stageMultiplier);
        Health.Initialize(Stats.MaxHealth);
        if (Anim != null)
        {
            Anim.Rebind();
            Anim.Update(0f);
        }
        if (Target == null) FindPlayer();
        if (Agent != null)
        {
            Agent.Warp(validPosition);
            Agent.isStopped = false;
        }
    }
    private void Update()
    {
        if (Target == null || Health.IsDead) return;
        UpdateNavigation();
    }
    private void UpdateNavigation()
    {
        if (Agent == null || !Agent.isOnNavMesh) {
        Debug.LogError($"[{gameObject.name}] NavMeshAgent component reference is NULL.");
            return;
        }
        Agent.SetDestination(Target.position);
        if (Target == null) FindPlayer();
Debug.Log($"[{gameObject.name}] NavUpdate -> TargetPos: {Target.position} | PathStatus: {Agent.pathStatus} | IsStopped: {Agent.isStopped} | Speed: {Agent.speed}");
        float distance = Vector3.Distance(transform.position, Target.position);
        Anim.SetFloat("DistanceToTarget", distance);
        Anim.SetFloat(SpeedHash, Agent.velocity.magnitude);
        Anim.SetBool("IsDead", Health.IsDead);
    }
    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) Target = player.transform;
        Debug.Log($"[{gameObject.name}] Target acquired: {player.name} at {Target.position}");
 
    }
    private void HandleDeath()
    {
        if (PoolManager != null)
            PoolManager.Despawn(gameObject);
        else
            gameObject.SetActive(false);
    }
}

public static class NavMeshUtility
{
    public static bool TryGetValidSpawnPosition(Vector3 targetPosition, out Vector3 validPosition, float sampleRadius)
    {
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
        {
            validPosition = hit.position;
            return true;
        }

        validPosition = targetPosition;
        return false;
    }
    
}