using System.Collections;
using UnityEngine;

namespace Project.Scripts.Dennis.VFX
{
    // Holt Effekte aus dem PoolManager und gibt sie nach ihrer Lebenszeit zurück.
    // Basiert auf Chris' VFXManager, angepasst an unseren PoolManager.
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private VFXEventChannel _vfxEventChannel;
        [SerializeField] private PoolManager _poolManager;   // leer lassen, wird dann gesucht

        private void Awake()
        {
            if (_poolManager == null) _poolManager = FindAnyObjectByType<PoolManager>();
        }

        private void OnEnable()
        {
            if (_vfxEventChannel != null) _vfxEventChannel.OnVFXRequested += HandleVFXRequest;
        }

        private void OnDisable()
        {
            if (_vfxEventChannel != null) _vfxEventChannel.OnVFXRequested -= HandleVFXRequest;
        }

        private void HandleVFXRequest(VFXData data, Vector3 position, Quaternion rotation)
        {
            if (data == null || _poolManager == null) return;

            GameObject prefab = data.GetRandomPrefab();
            if (prefab == null) return;

            GameObject instance = _poolManager.Spawn(prefab, position + data.spawnOffset, rotation);
            if (instance == null) return;

            StartCoroutine(ReturnAfter(instance, data.lifetime));
        }

        private IEnumerator ReturnAfter(GameObject instance, float delay)
        {
            yield return new WaitForSeconds(delay);

            // Könnte inzwischen schon zurückgegeben oder zerstört sein
            if (instance == null || !instance.activeSelf) yield break;
            _poolManager.Despawn(instance);
        }
    }
}
