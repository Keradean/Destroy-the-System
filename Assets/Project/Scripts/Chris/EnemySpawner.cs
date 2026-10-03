using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] private PoolManager poolManager;

    [Header("Spawn Setting")]
    [SerializeField] private float sampleRadius = 3.0f;

    private void Start()
    {
        if (poolManager == null) poolManager = FindAnyObjectByType<PoolManager>();
        spawnTimer = autoSpawnInterval;
    }
    public GameObject SpawnEnemy(EnemyDataSO enemyData, Vector3 rawSpawnPosition, float currentStage)
    {
        if (poolManager == null) return null;
        if (!NavMeshUtility.TryGetValidSpawnPosition(rawSpawnPosition, out Vector3 validPosition, sampleRadius))
            return null;
        GameObject enemyObj = poolManager.Spawn(enemyData.prefab, validPosition, Quaternion.identity);
        if (enemyObj.TryGetComponent<EnemyControllerBase>(out var controller))
            controller.InitFromSpawn(enemyData, currentStage, validPosition, poolManager);
        return enemyObj;
    }
    #region Testing Setup

    [Header("Testing Configuration")]
    [SerializeField] private EnemyDataSO testEnemyData;
    [SerializeField] private Transform testSpawnPoint;
    [SerializeField] private float testStageMultiplier = 1.0f;
    [SerializeField] private bool enableAutoTestSpawning = true;
    [SerializeField] private float autoSpawnInterval = 3.0f;
    [SerializeField] private int maxTestSpawns = 5;

    private float spawnTimer;
    private int currentSpawnCount;


    private void Update()
    {
        if (!enableAutoTestSpawning || currentSpawnCount >= maxTestSpawns) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            spawnTimer = autoSpawnInterval;
            TriggerTestSpawn();
            currentSpawnCount++;
        }
    }

    /// <summary>
    /// Right-click the component header in the Inspector during Play Mode to manually trigger a test spawn.
    /// </summary>
    [ContextMenu("Spawn Test Enemy")]
    public void TriggerTestSpawn()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning($"[{gameObject.name}] Manual test spawn can only be executed during Play Mode.");
            return;
        }

        if (testEnemyData == null)
        {
            Debug.LogError($"[{gameObject.name}] Test spawn failed: 'Test Enemy Data' is missing in Inspector.");
            return;
        }

        Vector3 spawnPos = testSpawnPoint != null ? testSpawnPoint.position : transform.position;
        GameObject enemy = SpawnEnemy(testEnemyData, spawnPos, testStageMultiplier);

        if (enemy != null)
        {
            Debug.Log($"[{gameObject.name}] Test spawn successful! Spawned '{enemy.name}' at {spawnPos}");
        }
    }

    #endregion
}
