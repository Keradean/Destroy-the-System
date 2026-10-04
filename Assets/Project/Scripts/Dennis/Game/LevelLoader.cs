using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Dennis.Game
{
    // Lädt nach dem Sieg das nächste Level, nach Game Over das gleiche Level neu.
    // Sitzt in jeder Level-Szene, z.B. am GameManager-Objekt.
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;   // leer lassen, wird dann gesucht

        [Header("Nächstes Level")]
        [SerializeField] private string _nextSceneName;       // genau wie die Szene heißt, leer = letztes Level
        [SerializeField] private string _lastLevelSceneName;  // nach dem letzten Level, z.B. Hauptmenü, leer = Level neu starten

        [Header("Automatik")]
        [SerializeField] private bool _autoLoad = true;       // aus, sobald die UI Buttons für Weiter/Neustart hat
        [SerializeField] private float _winDelay = 3f;        // Sekunden nach dem Sieg
        [SerializeField] private float _loseDelay = 3f;       // Sekunden nach Game Over

        private void Awake()
        {
            if (_gameManager == null) _gameManager = FindAnyObjectByType<GameManager>();
        }

        private void OnEnable()
        {
            if (_gameManager == null) return;
            _gameManager.OnWin += HandleWin;
            _gameManager.OnLose += HandleLose;
        }

        private void OnDisable()
        {
            if (_gameManager == null) return;
            _gameManager.OnWin -= HandleWin;
            _gameManager.OnLose -= HandleLose;
        }

        private void HandleWin()
        {
            if (!_autoLoad) return;
            StartCoroutine(LoadAfter(_winDelay, LoadNextLevel));
        }

        private void HandleLose()
        {
            if (!_autoLoad) return;
            StartCoroutine(LoadAfter(_loseDelay, RestartLevel));
        }

        // Für einen "Weiter"-Button auf dem Sieg-Bildschirm
        public void LoadNextLevel()
        {
            if (!string.IsNullOrEmpty(_nextSceneName))
            {
                LoadScene(_nextSceneName);
                return;
            }

            // Letztes Level geschafft
            if (!string.IsNullOrEmpty(_lastLevelSceneName))
            {
                LoadScene(_lastLevelSceneName);
                return;
            }
            RestartLevel();
        }

        // Für einen "Nochmal"-Button auf dem Game-Over-Bildschirm
        public void RestartLevel()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        private void LoadScene(string sceneName)
        {
            // Sieg und Game Over pausieren das Spiel, sonst bleibt die neue Szene stehen
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }

        private IEnumerator LoadAfter(float delay, System.Action load)
        {
            // Realtime, weil bei Sieg und Game Over die Zeit auf 0 steht
            yield return new WaitForSecondsRealtime(delay);
            load();
        }
    }
}
