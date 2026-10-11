using System.Collections;
using Project.Scripts.Dennis.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Dennis.UI
{
    // Web-Version ist eine Demo: Nach dem Sieg im letzten Demo-Level erscheint ein Hinweis
    // zum Download, und der "Next Level"-Button wird ausgeblendet.
    // Sitzt am Canvas-Prefab. In Windows- und Linux-Builds passiert nichts.
    public class WebDemoNotice : MonoBehaviour
    {
        [SerializeField] private GameObject _demoNotice;        // Text/Panel im VictoryScreen, am Anfang inaktiv
        [SerializeField] private GameObject _nextLevelButton;   // wird in der Demo ausgeblendet
        [SerializeField] private string _lastDemoLevel = "Level_1";
        [SerializeField] private bool _testInEditor;            // zum Ausprobieren im Editor anhaken

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindAnyObjectByType<GameManager>();
            if (_demoNotice != null) _demoNotice.SetActive(false);
        }

        private void OnEnable()
        {
            if (_gameManager != null) _gameManager.OnWin += HandleWin;
        }

        private void OnDisable()
        {
            if (_gameManager != null) _gameManager.OnWin -= HandleWin;
        }

        private void HandleWin()
        {
            if (!IsWebDemo()) return;
            if (SceneManager.GetActiveScene().name != _lastDemoLevel) return;

            if (_demoNotice != null) _demoNotice.SetActive(true);
            StartCoroutine(HideNextLevelButton());
        }

        // Ein Frame warten, weil der VictoryScreen den Button beim Sieg selbst wieder einschalten kann
        private IEnumerator HideNextLevelButton()
        {
            yield return null;   // läuft auch bei timeScale 0 weiter
            if (_nextLevelButton != null) _nextLevelButton.SetActive(false);
        }

        private bool IsWebDemo()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) return true;
            return _testInEditor && Application.isEditor;
        }
    }
}
