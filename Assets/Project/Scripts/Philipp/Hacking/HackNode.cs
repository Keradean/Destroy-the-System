using UnityEngine;
using Project.Scripts.Dennis.Nodes;
using Project.Scripts.Dennis.Game;   // Dennis: für den GameManager

namespace Project.Scripts.Philipp.Hacking
{
    public class HackNode : Node
    {
        [Header("Dependencies")]
        [SerializeField] private GameManager gameManager;   // Dennis: GameManager existiert jetzt
        [Header("Settings")]
        [SerializeField] private float hackingTime = 5f;

        [Header("Hacking State")]
        [SerializeField] private float progress = 0f;
        [SerializeField] private bool isHacked = false;
        [SerializeField] private bool isPlayerInside = false;

        #region Public Getters
        public bool IsHacked => isHacked;
        public bool IsPlayerInside => isPlayerInside;
        #endregion

        #region Unity Callbacks
        // Dennis: Feld leer gelassen, dann den GameManager in der Szene suchen
        void Awake()
        {
            if (gameManager == null) gameManager = FindAnyObjectByType<GameManager>();
        }

        void Update()
        {
            if (isHacked || !isPlayerInside) return;

            progress += Time.deltaTime;

            // Dennis: ALARM steigt pro Sekunde Hacken
            gameManager.AddAlarm(Time.deltaTime);

            if (progress >= hackingTime)
            {
                isHacked = true;
                isPlayerInside = false;   // Dennis: fertig gehackt, Player zählt nicht mehr als "drin"
                Debug.Log("[HackNode] Hacking Completed!");
                gameManager.NodeHacked();   // Dennis: Hack an den GameManager melden
            }
        }
        #endregion

        #region Methods
        public override void OnPlayerEnter()
        {
            base.OnPlayerEnter();

            // Dennis: ein anderer Node ist angefangen und muss erst fertig werden
            if (!isHacked && !gameManager.CanStartHack(this))
            {
                Debug.Log("[HackNode] Gesperrt, erst den angefangenen Node fertig hacken");
                return;
            }

            if (!isHacked)
            {
                gameManager.HackStarted(this);   // Dennis: dieser Node ist jetzt der aktive Hack
                isPlayerInside = true;
                Debug.Log("[HackNode] Hacking started!");
            }
        }

        public override void OnPlayerExit() // Geändert von Dennis zu Override um einen weg anstatt zwei zu haben.
        {
            // Dennis: nur melden, wenn wirklich ein Hack abgebrochen wird
            if (!isPlayerInside) return;

            isPlayerInside = false;
            Debug.Log("[HackNode] Hacking interrupted!");
        }
        #endregion

        #region Gizmos
        //TODO implement gizmo color change based on hacking state
        #endregion
    }
}
