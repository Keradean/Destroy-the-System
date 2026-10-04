using System.Collections;
using Project.Scripts.Dennis.Game;
using UnityEngine;
using UnityEngine.Audio;

namespace Project.Scripts.Dennis.Audio
{
    // Spielt Sounds über den PoolManager ab und wechselt die Musik passend zum Spielverlauf.
    // Basiert auf Chris' AudioManager, angepasst an unseren GameManager und PoolManager.
    public class AudioManager : MonoBehaviour
    {
        [Header("Abhängigkeiten")]
        [SerializeField] private AudioEventChannel _audioChannel;
        [SerializeField] private PoolManager _poolManager;      // leer lassen, wird dann gesucht
        [SerializeField] private GameManager _gameManager;      // leer lassen, wird dann gesucht
        [SerializeField] private AudioMixer _mainMixer;         // optional

        [Header("SFX")]
        [SerializeField] private GameObject _sfxSourcePrefab;   // Prefab mit AudioSource und PooledAudioSource

        [Header("Musik")]
        [SerializeField] private AudioSource _musicSourceA;
        [SerializeField] private AudioSource _musicSourceB;
        [SerializeField] private float _musicFadeDuration = 1.5f;

        [Header("Musik pro Spielabschnitt")]
        [SerializeField] private SoundData _gameMusic;          // läuft ab Spielstart
        [SerializeField] private SoundData _bossMusic;          // ab Betreten des Core
        [SerializeField] private SoundData _winMusic;
        [SerializeField] private SoundData _loseMusic;

        private AudioSource _activeMusicSource;
        private Coroutine _musicFadeRoutine;

        private void Awake()
        {
            if (_poolManager == null) _poolManager = FindAnyObjectByType<PoolManager>();
            if (_gameManager == null) _gameManager = FindAnyObjectByType<GameManager>();

            if (_musicSourceA != null) _musicSourceA.playOnAwake = false;
            if (_musicSourceB != null) _musicSourceB.playOnAwake = false;
        }

        private void OnEnable()
        {
            if (_audioChannel != null)
            {
                _audioChannel.OnSFXRequested += PlaySFX;
                _audioChannel.OnMusicRequested += PlayMusic;
            }

            if (_gameManager != null)
            {
                _gameManager.OnCoreReached += HandleCoreReached;
                _gameManager.OnWin += HandleWin;
                _gameManager.OnLose += HandleLose;
            }
        }

        private void OnDisable()
        {
            if (_audioChannel != null)
            {
                _audioChannel.OnSFXRequested -= PlaySFX;
                _audioChannel.OnMusicRequested -= PlayMusic;
            }

            if (_gameManager != null)
            {
                _gameManager.OnCoreReached -= HandleCoreReached;
                _gameManager.OnWin -= HandleWin;
                _gameManager.OnLose -= HandleLose;
            }
        }

        private void Start()
        {
            PlayMusic(_gameMusic, _musicFadeDuration);
        }

        private void HandleCoreReached() => PlayMusic(_bossMusic, _musicFadeDuration);
        private void HandleWin() => PlayMusic(_winMusic, _musicFadeDuration);
        private void HandleLose() => PlayMusic(_loseMusic, _musicFadeDuration);

        // Lautstärke 0 bis 1, z.B. von einem Slider im Optionsmenü.
        // Im AudioMixer müssen die Parameter MasterVolume, MusicVolume und SFXVolume freigegeben sein.
        public void SetMasterVolume(float linearVolume) => SetMixerVolume("MasterVolume", linearVolume);
        public void SetMusicVolume(float linearVolume) => SetMixerVolume("MusicVolume", linearVolume);
        public void SetSFXVolume(float linearVolume) => SetMixerVolume("SFXVolume", linearVolume);

        private void SetMixerVolume(string parameterName, float linearVolume)
        {
            if (_mainMixer == null) return;

            // Mixer rechnet in Dezibel, 0.0001 statt 0, weil Log10(0) nicht geht
            float decibel = Mathf.Log10(Mathf.Clamp(linearVolume, 0.0001f, 1f)) * 20f;
            _mainMixer.SetFloat(parameterName, decibel);
        }

        private void PlaySFX(SoundData data, Vector3 position)
        {
            if (data == null || _poolManager == null || _sfxSourcePrefab == null) return;

            GameObject sourceObject = _poolManager.Spawn(_sfxSourcePrefab, position, Quaternion.identity);
            if (sourceObject == null) return;
            if (!sourceObject.TryGetComponent(out PooledAudioSource pooledSource)) return;

            pooledSource.Play(data, _poolManager);
        }

        private void PlayMusic(SoundData data, float fadeDuration)
        {
            if (data == null) return;
            if (_musicSourceA == null || _musicSourceB == null) return;

            AudioClip clip = data.GetRandomClip();
            if (clip == null) return;

            // Läuft schon, nichts tun
            if (_activeMusicSource != null && _activeMusicSource.clip == clip && _activeMusicSource.isPlaying) return;

            if (_musicFadeRoutine != null) StopCoroutine(_musicFadeRoutine);

            // Zwei Quellen im Wechsel, damit die alte aus- und die neue einblenden kann
            AudioSource nextSource = _activeMusicSource == _musicSourceA ? _musicSourceB : _musicSourceA;
            nextSource.clip = clip;
            nextSource.loop = true;
            nextSource.spatialBlend = 0f;
            nextSource.volume = 0f;
            nextSource.Play();

            _musicFadeRoutine = StartCoroutine(CrossFadeMusic(nextSource, data.volume, fadeDuration));
        }

        private IEnumerator CrossFadeMusic(AudioSource newSource, float targetVolume, float duration)
        {
            AudioSource oldSource = _activeMusicSource;
            _activeMusicSource = newSource;
            float startOldVolume = oldSource != null ? oldSource.volume : 0f;
            float timer = 0f;

            while (timer < duration)
            {
                // unscaled, weil bei Sieg und Game Over die Zeit auf 0 steht
                timer += Time.unscaledDeltaTime;
                float t = timer / duration;

                if (oldSource != null) oldSource.volume = Mathf.Lerp(startOldVolume, 0f, t);
                newSource.volume = Mathf.Lerp(0f, targetVolume, t);
                yield return null;
            }

            newSource.volume = targetVolume;
            if (oldSource != null) oldSource.Stop();
            _musicFadeRoutine = null;
        }
    }
}
