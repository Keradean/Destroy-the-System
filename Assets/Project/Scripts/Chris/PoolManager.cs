using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;


public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public struct PrewarmConfiguration
    {
        public string label;
        public GameObject prefab;
        public int prewarmCount;
        public int maxSize;
    }

    [SerializeField] private List<PrewarmConfiguration> prewarmList = new();

    private readonly Dictionary<GameObject, IObjectPool<GameObject>> poolDictionary = new();

    private void Awake()
    {
        PrewarmPools();       
    }
    private void PrewarmPools()
    {
        foreach (var config in prewarmList)
        {
            if (config.prefab != null)
            {
                GetOrCreatePool(config.prefab, config.prewarmCount, config.maxSize);
            }
        }
    }
    private IObjectPool<GameObject> GetOrCreatePool(GameObject prefab, int defaultCapacity = 10, int maxSize = 1000)
    {
        if (poolDictionary.TryGetValue(prefab, out var existingPool))
            return existingPool;
        
        var pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject instance = Instantiate(prefab);
                var tracker = instance.AddComponent<PooledObjectTracker>();
                tracker.SourcePrefab = prefab;

                return instance;
            },
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
        poolDictionary.Add(prefab, pool);
        return pool;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;
        var pool = GetOrCreatePool(prefab);
        GameObject instance = pool.Get();
        instance.transform.SetPositionAndRotation(position, rotation);
        return instance;
    }
    public void Despawn(GameObject instance)
    {
        if (instance == null) return;
        if (instance.TryGetComponent<PooledObjectTracker>(out var tracker) && tracker.SourcePrefab != null)
        {
            if (poolDictionary.TryGetValue(tracker.SourcePrefab, out var pool))
            {
                pool.Release(instance);
                return;
            }
        }
        Destroy(instance);
    }
}

public class PooledObjectTracker : MonoBehaviour
{
    public GameObject SourcePrefab { get; set; }
}