using Project.Scenes.Sandbox.Ender.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Dennis.UI
{
    // Sitzt am Canvas-Prefab und kennt die UI-Teile, die Manager in der Szene brauchen.
    // Prefabs können nicht auf Szenen-Objekte zeigen, deshalb holen sich die Manager
    // die Verknüpfungen beim Start hier ab. So muss man sie nur einmal im Prefab setzen.
    public class UIReferences : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _levelUpScreen;

        [Header("Level-up")]
        [SerializeField] private Image[] _levelUpOptions = new Image[3];

        [Header("Anzeigen")]
        [SerializeField] private BossHealthUI _bossHealthUI;
        [SerializeField] private ExpBarUI _expBarUI;

        public GameObject PauseMenu => _pauseMenu;
        public GameObject LevelUpScreen => _levelUpScreen;
        public Image[] LevelUpOptions => _levelUpOptions;
        public BossHealthUI BossHealthUI => _bossHealthUI;
        public ExpBarUI ExpBarUI => _expBarUI;

        // Findet die Referenzen auch, wenn der Canvas gerade inaktiv ist
        public static UIReferences Find()
        {
            return FindAnyObjectByType<UIReferences>(FindObjectsInactive.Include);
        }
    }
}
