using Project.Scripts.Philon.Levelup;
using UnityEngine;

namespace Project.Scripts.Dennis.UI
{
    // Sitzt am Canvas-Prefab. Buttons im Prefab rufen diese Methoden auf,
    // weil ein Prefab nicht direkt auf Manager in der Szene zeigen kann.
    // Die Manager werden beim ersten Klick in der Szene gesucht.
    public class UIButtonRelay : MonoBehaviour
    {
        private PauseManager _pauseManager;
        private LevelUpManager _levelUpManager;

        // Pause-Menü: Continue
        public void Continue()
        {
            if (!FindPauseManager()) return;
            _pauseManager.Continue();
        }

        // Pause-Menü: Main Menu
        public void BackToMenu()
        {
            if (!FindPauseManager()) return;
            _pauseManager.BackToMenu();
        }

        // Level-up-Karten: 0, 1 oder 2 im Button eintragen
        public void SelectUpgrade(int index)
        {
            if (_levelUpManager == null) _levelUpManager = FindAnyObjectByType<LevelUpManager>();
            if (_levelUpManager == null)
            {
                Debug.LogWarning("UIButtonRelay: kein LevelUpManager in der Szene");
                return;
            }
            _levelUpManager.SelectUpgrade(index);
        }

        private bool FindPauseManager()
        {
            if (_pauseManager == null) _pauseManager = FindAnyObjectByType<PauseManager>();
            if (_pauseManager != null) return true;

            Debug.LogWarning("UIButtonRelay: kein PauseManager in der Szene");
            return false;
        }
    }
}
