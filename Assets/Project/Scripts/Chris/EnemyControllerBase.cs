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

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Stats = GetComponent<RuntimeEnemyStats>();
        Health = GetComponent<HealthComponent>();
        
        if (Target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) Target = player.transform;
        }
    }
    //TODO Enable and Disable after Components are written and wiring can begin
    private void OnEnable()
    {
        Health.OnDeath += HandleDeath;
    }
    private void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
        if (Agent != null)
        {
            Agent.isStopped = true;
            Agent.ResetPath();
        }
    }
    public void InitFromSpawn(EnemyDataSO data, float stageMultiplier)
    {
        Stats.SetupStats(data, stageMultiplier);
        Health.Initialize(Stats.MaxHealth);
        if (Anim != null)
        {
            Anim.Rebind();
            Anim.Update(0f);
        }
        if (Agent != null) Agent.isStopped = false;
    }
    private void Update()
    {
        if (Target == null || Health.IsDead) return;
        UpdateNavigation();
    }
    private void UpdateNavigation()
    {
        if (Agent == null || !Agent.isOnNavMesh) return;
        float distance = Vector3.Distance(transform.position, Target.position);
        Anim.SetFloat("DistanceToTarget", distance);
        Anim.SetFloat("Speed", Agent.velocity.magnitude);
        Anim.SetBool("IsDead", Health.IsDead);
    }
    private void HandleDeath()
    {
        //TODO Wait until Objectpooling is done then update this shit
            gameObject.SetActive(false);
    }
}
