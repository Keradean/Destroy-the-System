using UnityEngine;

[AddComponentMenu("Entities/Components/SpawnerComponent")]
public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] private EnemyDataSO gruntData;
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnGrunts(PoolManager pool)
    {
        if (pool == null || gruntData == null) return;
        foreach (var point in spawnPoints)
        {
            GameObject go = pool.Spawn(gruntData.prefab, point.position, Quaternion.identity);
            if (go.TryGetComponent<EnemyControllerBase>(out var controller))
                controller.InitFromSpawn(gruntData, 1.0f, point.position, pool);
        }
    }
}
