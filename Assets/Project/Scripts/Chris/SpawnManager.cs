using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.Dennis.Game;

public class SpawnManager : MonoBehaviour
{
   [Header("Dependencies")]
   [SerializeField] private GameManager gameManager;
   [SerializeField] private List<EnemySpawner> enemySpawners = new();

   [Header("Configurations")]
   [SerializeField] private EnemyDataSO scannerEnemyData;
   [SerializeField] private EnemyDataSO tracerEnemyData;
   [SerializeField] private EnemyDataSO firewallEnemyData;
   [SerializeField] private EnemyDataSO bossEnemyData;

   [Header("Wave/Alarm Settings")]
   [SerializeField] private float baseSpawnInterval = 8.0f;
   [SerializeField] private float minSpawnInterval = 2.0f;
   [SerializeField] private Transform bossSpawnPoint;

   private float spawnTimer;
   private bool isBossSpawned;

    private void Start()
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
        if (gameManager != null || gameManager.State != GameManager.GameState.Playing) return;
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
        EnemySpawner selectedSpawner = enemySpawners[Random.Range(0, enemySpawners.Count)];
        float alarmPercent = gameManager.MaxAlarm > 0 ? gameManager.Alarm / gameManager.MaxAlarm : 0f;
        float stageMultiplier = 1.0f + (alarmPercent * 1.5f); // Needs adjustment maybe
        EnemyDataSO enemyToSpawn = (alarmPercent > 0.4f && Random.value > 0.5f) ? scannerEnemyData : tracerEnemyData;
        selectedSpawner.SpawnEnemy(enemyToSpawn,transform.position, stageMultiplier);
    }
    private void ResetTimer()
    {
        float alarmPercent = gameManager.MaxAlarm > 0 ? gameManager.Alarm / gameManager.MaxAlarm : 0f;
        float currentInterval = Mathf.Lerp(baseSpawnInterval, minSpawnInterval, alarmPercent);
        spawnTimer = currentInterval;
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
            health.OnDeath += OnBossKilled;
    }
    private void OnBossKilled()
    {
        if (gameManager != null) gameManager.BossDefeated();
    } 
}
