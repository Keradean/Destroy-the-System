using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.Dennis.Game;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawnConfiguration
    {
        public string name;
        public EnemyDataSO enemyData;
        [Range(1, 100)] public int baseWeight;
        [Range(0f, 1f)] public float minAlarmPercent;
    }

    [Header("Dependencies")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<EnemySpawner> enemySpawners = new();     
    
    [Header("Configurations")]
    [SerializeField] private List<EnemySpawnConfiguration> enemyPool = new();
    [SerializeField] private EnemyDataSO bossEnemyData;       
    [SerializeField] private Transform bossSpawnPoint;     
    
    [Header("Boss UI")]
    [SerializeField] private BossHealthUI bossHealthUI;
    
    [Header("Wave/Alarm Settings")]
    [SerializeField] private float baseSpawnInterval = 8.0f;
    [SerializeField] private float minSpawnInterval = 2.0f;
    private float spawnTimer;
    private bool isBossSpawned;

    private void Awake()
    {
        if (gameManager == null) gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnEnable()
    {
        if (gameManager != null)
        gameManager.OnAlarmChanged += HandleAlarmChange;
        gameManager.OnCoreReached += HandleCoreBreach;
    }
    private void OnDisable()
    {
        if (gameManager != null)
        gameManager.OnAlarmChanged -= HandleAlarmChange;
        gameManager.OnCoreReached -= HandleCoreBreach;

    }
    private void Update()
    {
        if (gameManager == null || gameManager.State != GameManager.GameState.Playing) return;
        if (isBossSpawned) return;
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            TriggerSpawnWave();
            ResetTimer();
        }
    }
    private void TriggerSpawnWave()
    {
        if (enemySpawners.Count == 0 || enemyPool.Count == 0) return;
        float alarmPercent = gameManager.MaxAlarm > 0 ? gameManager.Alarm / gameManager.MaxAlarm : 0f;
        List<EnemySpawnConfiguration> validConfigs = new();
        Debug.Log($"[SpawnManager] Current Alarm %: {alarmPercent:P0} | Valid Configs: {validConfigs.Count}/{enemyPool.Count}");
        int totalWeight = 0;

        foreach (var config in enemyPool)
        {
            if (config.enemyData == null) continue;
            if (alarmPercent >= config.minAlarmPercent)
            {
                validConfigs.Add(config);
                totalWeight += config.baseWeight;
            }
        }

        if (validConfigs.Count == 0 || totalWeight <= 0) return;
        int roll = Random.Range(0, totalWeight);
        int currentSum = 0;
        EnemySpawnConfiguration selectedConfig = validConfigs[0];
        foreach (var config in validConfigs)
        {
            currentSum += config.baseWeight;
            if (roll < currentSum)
            {
                selectedConfig = config;
                break;
            }
        }
        EnemySpawner selectedSpawner = enemySpawners[Random.Range(0, enemySpawners.Count)];
        float stageMultiplier = 3.0f + (alarmPercent * 1.5f);
        selectedSpawner.SpawnEnemyAtRandomPosition(selectedConfig.enemyData, stageMultiplier);
    }
    private void ResetTimer()
    {
        float alarmPercent = gameManager.MaxAlarm > 0 ? gameManager.Alarm / gameManager.MaxAlarm : 0f;
        spawnTimer = Mathf.Lerp(baseSpawnInterval, minSpawnInterval, alarmPercent);
    }
    private void HandleAlarmChange(float currentAlarm, float maxAlarm)
    {
        ResetTimer();
    }
    private void HandleCoreBreach()
    {
        if (isBossSpawned || bossEnemyData == null) return;
        isBossSpawned = true;
        Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : transform.position;
        GameObject bossGO = enemySpawners[0].SpawnEnemy(bossEnemyData, spawnPos, 2.0f);
        if (bossGO != null && bossGO.TryGetComponent<HealthComponent>(out HealthComponent health))
        {
            health.OnDeath += () => gameManager?.BossDefeated();

            if (bossHealthUI)
            {
                bossHealthUI.SetBoss(health);
            }
        }
    }
}
