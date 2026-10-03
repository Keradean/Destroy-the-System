using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] private EnemyDataSO gruntData;
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnGrunts(PoolManager pool)
    {
        if (pool == null || gruntData == null) return;
        foreach (var point in spawnPoints)
            pool.Spawn(gruntData.prefab, point.position, Quaternion.identity);
    }
}
