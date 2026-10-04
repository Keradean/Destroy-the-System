using UnityEngine;

namespace Project.Scripts.Dennis.Audio
{
    // Ein Sound mit Einstellungen. Mehrere Clips möglich, dann wird zufällig einer gewählt.
    // Basiert auf Chris' SoundData.
    [CreateAssetMenu(fileName = "SoundData", menuName = "Destroy the System/Audio/Sound Data")]
    public class SoundData : ScriptableObject
    {
        [Header("Clips")]
        public AudioClip[] clips;

        [Header("Lautstärke und Tonhöhe")]
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 2f)] public float minPitch = 0.9f;
        [Range(0.1f, 2f)] public float maxPitch = 1.1f;

        [Header("Raumklang")]
        [Tooltip("0 = 2D (UI, Musik), 1 = 3D (Sound in der Welt)")]
        [Range(0f, 1f)] public float spatialBlend = 1f;
        public float minDistance = 1f;
        public float maxDistance = 25f;

        public AudioClip GetRandomClip()
        {
            if (clips == null || clips.Length == 0) return null;
            return clips[Random.Range(0, clips.Length)];
        }
    }
}
