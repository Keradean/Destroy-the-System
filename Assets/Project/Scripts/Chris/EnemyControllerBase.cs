using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RuntimeEnemyStats))]
[RequireComponent(typeof(HealthComponent))]
public class EnemyControllerBase : MonoBehaviour
{
    private static readonly int CanSpecialHash = Animator.StringToHash("CanSpecial");
    private static readonly int InSpecialRangeHash = Animator.StringToHash("InSpecialRange");
    private static readonly int CanAttackHash = Animator.StringToHash("CanAttack");
    private static readonly int InRangedRangeHash = Animator.StringToHash("InRangedRange");
    private static readonly int InMeleeRangeHash = Animator.StringToHash("InMeleeRange");
    private static readonly int DistanceToTargetHash = Animator.StringToHash("DistanceToTarget");

    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }
    public RuntimeEnemyStats Stats { get; private set; }
    public HealthComponent Health { get; private set; }
    public Transform Target { get; private set; }
    public LayerMask TargetLayer { get; private set; }
    public PoolManager PoolManager { get; private set; }
    public FXBridge FXBridge { get; private set; }

    //Optional Components
    public ShootingComponent Shooting { get; private set; }
    public AttackComponent Attack { get; private set; }
    public SpawnerComponent Spawner { get; private set; }


    #region Internal
    private float nextAttackTime;
    private float nextSpecialTime;
    
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Stats = GetComponent<RuntimeEnemyStats>();
        Health = GetComponent<HealthComponent>();
        Shooting = GetComponent<ShootingComponent>();
        Attack = GetComponent<AttackComponent>();
        Spawner = GetComponent<SpawnerComponent>();
        FXBridge = GetComponent<FXBridge>();

        FindPlayer();
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
    #endregion


    #region Logic
    public void InitFromSpawn(EnemyDataSO data, float stageMultiplier, Vector3 validPosition, PoolManager poolManager)
    {
        PoolManager = poolManager;
        if (Target == null) FindPlayer();
        Stats.SetupStats(data, stageMultiplier);
        Health.Initialize(Stats.MaxHealth);
        nextAttackTime = 0f;
        if (Agent != null)
        {
            Agent.enabled = true;
            Agent.Warp(validPosition);
            Agent.isStopped = false;
        }
        if (Anim != null)
        {
            Anim.Rebind();
            BindBehaviours();
            Anim.Update(0f);
        }
        TriggerSpecialCooldown();
    }
    private void Update()
    {
        if (Target == null || Health.IsDead || Stats == null) return;
        if (Target == null) FindPlayer(); // just in case
        float distance = Vector3.Distance(transform.position, Target.position);
        Anim.SetFloat(DistanceToTargetHash, distance);
        Anim.SetBool(InMeleeRangeHash, distance <= Stats.MeleeRange);
        Anim.SetBool(InRangedRangeHash, distance <= Stats.RangedRange);
        bool canAttack = Time.time >= nextAttackTime;
        if (canAttack) Anim.SetBool(CanAttackHash, canAttack);
        if (Stats.IsSpecial)
        {
            bool canSpecial = Time.time >= nextSpecialTime;
            Anim.SetBool(InSpecialRangeHash, distance <= Stats.SpecialRange);
            Anim.SetBool(CanSpecialHash, canSpecial);
        }
    }
    #endregion


    #region Helpers
    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) Target = player.transform;
        TargetLayer = player.layer;
    }
    private void HandleDeath()
    {
        // Reward Player with XP
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
    public void TriggerAttackCoolDown()
    {
        nextAttackTime = Time.time + Stats.AttackCooldown;
    }
    public void TriggerSpecialCooldown()
    {
        nextSpecialTime = Time.time + Stats.SpecialCooldown;
    }
    #endregion


    #region Animation Events        This entire region was written by AI. I think this is an appropriate use case for that shit
    /// <summary>
    /// ANIMATION EVENT RELAYS (Unity Engine Calls -> SMB Pipeline)
    /// All Unity Animation Events defined in the FBX files call these methods directly.
    /// They route keyframes straight down to all active StateMachineBehaviours (EnemySMBase).
    /// </summary>

    // --- SPAWN & DEATH EVENTS ---
    public void AE_OnSpawnStartFrame() => RelayAnimationEvent("AE_OnSpawnStartFrame");
    public void AE_OnSpawnEndFrame() => RelayAnimationEvent("AE_OnSpawnEndFrame");
    public void AE_OnDeathStartFrame() => RelayAnimationEvent("AE_OnDeathStartFrame");
    public void AE_OnDeathEndFrame() => RelayAnimationEvent("AE_OnDeathEndFrame");

    // --- COMBAT & ATTACK EVENTS ---
    public void AE_OnShootFrame() => RelayAnimationEvent("AE_OnShootFrame");
    public void AE_OnHitFrame() => RelayAnimationEvent("AE_OnHitFrame");
    public void AE_OnAttackEndFrame() => RelayAnimationEvent("AE_OnAttackEndFrame");
    public void AE_OnChargeStartFrame() => RelayAnimationEvent("AE_OnChargeStartFrame");

    // --- FIREWALL BLOCK EVENTS ---
    public void AE_OnBlockStartFrame() => RelayAnimationEvent("AE_OnBlockStartFrame");
    public void AE_OnBlockEndFrame() => RelayAnimationEvent("AE_OnBlockEndFrame");

    // --- DAMAGE & REACTION EVENTS ---
    public void AE_OnHitFlashFrame() => RelayAnimationEvent("AE_OnHitFlashFrame");
    public void AE_OnDamageEndFrame() => RelayAnimationEvent("AE_OnDamageEndFrame");

    // --- SCANNER & MOVEMENT EVENTS ---
    public void AE_OnScanSweep() => RelayAnimationEvent("AE_OnScanSweep");
    public void AE_OnTurn() => RelayAnimationEvent("AE_OnTurn");
    public void AE_OnStepFrame() => RelayAnimationEvent("AE_OnStepFrame");

    // --- BOSS SUMMON EVENTS ---
    public void AE_OnSummonStartFrame() => RelayAnimationEvent("AE_OnSummonStartFrame");
    public void AE_OnSummonFrame() => RelayAnimationEvent("AE_OnSummonFrame");
    public void AE_OnSummonEndFrame() => RelayAnimationEvent("AE_OnSummonEndFrame");

    // --- PROJECTILE & IMPACT EVENTS ---
    public void AE_OnImpactStartFrame() => RelayAnimationEvent("AE_OnImpactStartFrame");
    public void AE_OnImpactEndFrame() => RelayAnimationEvent("AE_OnImpactEndFrame");

    /// <summary>
    /// Central Relay mechanism: Distributes event triggers to all SMBs on this Animator.
    /// </summary>
    private void RelayAnimationEvent(string eventName)
    {
        if (Anim == null) return;

        var behaviours = Anim.GetBehaviours<EnemySMBase>();
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].OnAnimationEvent(eventName);
        }
        if (FXBridge != null) FXBridge.OnAnimationEvent(eventName);
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