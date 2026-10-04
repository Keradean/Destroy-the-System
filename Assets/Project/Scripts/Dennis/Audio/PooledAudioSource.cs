using UnityEngine;

namespace Project.Scripts.Dennis.Audio
{
    // Sitzt auf dem SFX-Prefab. Spielt einen Sound ab und geht danach zurück in den Pool.
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudioSource : MonoBehaviour
    {
        private AudioSource _source;
        private PoolManager _poolManager;
        private float _remainingTime;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.playOnAwake = false;
        }

        public void Play(SoundData data, PoolManager poolManager)
        {
            AudioClip clip = data.GetRandomClip();
            if (clip == null)
            {
                poolManager.Despawn(gameObject);
                return;
            }

            _poolManager = poolManager;
            _source.clip = clip;
            _source.volume = data.volume;
            _source.pitch = Random.Range(data.minPitch, data.maxPitch);
            _source.spatialBlend = data.spatialBlend;
            _source.minDistance = data.minDistance;
            _source.maxDistance = data.maxDistance;
            _source.Play();

            // Höhere Tonhöhe spielt schneller ab, deshalb durch pitch teilen
            _remainingTime = clip.length / _source.pitch;
        }

        private void Update()
        {
            if (_poolManager == null) return;

            // unscaled, damit Sounds auch bei pausiertem Spiel (Sieg, Game Over) zurückkommen
            _remainingTime -= Time.unscaledDeltaTime;
            if (_remainingTime > 0f) return;

            _source.Stop();
            PoolManager poolManager = _poolManager;
            _poolManager = null;
            poolManager.Despawn(gameObject);
        }
    }
}
