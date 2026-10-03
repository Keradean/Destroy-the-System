using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RuntimeEnemyStats))]
[RequireComponent(typeof(HealthComponent))]
public class EnemyControllerBase : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }
    public RuntimeEnemyStats Stats { get; private set; }
    public HealthComponent Health { get; private set; }
    public Transform Target { get; private set; }
    public Transform FirePoint { get; private set; }
    public LayerMask TargetLayer { get; private set; }
    public PoolManager PoolManager { get; private set; }

    #region Internal
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Stats = GetComponent<RuntimeEnemyStats>();
        Health = GetComponent<HealthComponent>();
        FindPlayer();
        FindFirePoint();
        BindBehaviours();
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
    #endregion


    #region Logic
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
        if (Target == null) FindPlayer();   //Absolutely redundant, but jsut in case

        float distance = Vector3.Distance(transform.position, Target.position);
        Anim.SetFloat("DistanceToTarget", distance);
        Anim.SetFloat("Speed", Agent.velocity.magnitude);
        Anim.SetBool("IsDead", Health.IsDead);
        Anim.SetBool("InAttackRange", distance <= Stats.AttackRange);
    }
    #endregion


    #region Helpers
    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) Target = player.transform;
        TargetLayer = player.layer;
    }
    private void FindFirePoint()
    {
        Transform fp = transform.Find("FirePoint");
        FirePoint = fp != null ? fp : transform;
    }
    private void HandleDeath()
    {
        if (PoolManager != null)
            PoolManager.Despawn(gameObject);
        else
            gameObject.SetActive(false);
    }
    private void BindBehaviours()
    {
        if (Anim == null) return;
        var behaviours = Anim.GetBehaviours<EnemySMBase>();
        for (int i = 0; i < behaviours.Length; i++)
            behaviours[i].Initialize(this);
    }
    #endregion
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