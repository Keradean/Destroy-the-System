using UnityEngine;

namespace Project.Scripts.Dennis.VFX
{
    // Ein Effekt mit Einstellungen. Mehrere Prefabs möglich, dann wird zufällig eins gewählt.
    // Basiert auf Chris' VFXData.
    [CreateAssetMenu(fileName = "VFXData", menuName = "Destroy the System/VFX/VFX Data")]
    public class VFXData : ScriptableObject
    {
        public GameObject[] prefabs;
        public float lifetime = 2f;         // nach dieser Zeit zurück in den Pool
        public Vector3 spawnOffset;         // Versatz zur angefragten Position

        public GameObject GetRandomPrefab()
        {
            if (prefabs == null || prefabs.Length == 0) return null;
            return prefabs[Random.Range(0, prefabs.Length)];
        }
    }
}
