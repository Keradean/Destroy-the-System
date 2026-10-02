using UnityEngine;
using Project.Scripts.Dennis.Nodes;

namespace Project.Scripts.Philipp.Hacking
{
    public class HackNode : Node
    {
        [Header("Dependencies")]
        // [SerializeField] private GameManager gameManager;    //TODO remove comment as soon as GameManager is implemented
        [Header("Settings")]
        [SerializeField] private float hackingTime = 5f;
        [SerializeField] private string playerTag = "Player";

        [Header("Hacking State")]
        [SerializeField] private float progress = 0f;
        [SerializeField] private bool isHacked = false;
        [SerializeField] private bool isPlayerInside = false;

        #region Public Getters
        public bool IsHacked => isHacked;
        public bool IsPlayerInside => isPlayerInside;
        #endregion

        #region Unity Callbacks
        void Update()
        {
            if (isHacked || !isPlayerInside) return;

            progress += Time.deltaTime;

            // TODO Remove Comment as soon GameManager is implemented
            // gameManager.AddAlarm();

            if (progress >= hackingTime)
            {
                isHacked = true;
                Debug.Log("[HackNode] Hacking Completed!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                OnPlayerEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                OnPlayerExit();
            }
        }
        #endregion

        #region Methods
        public override void OnPlayerEnter()
        {
            base.OnPlayerEnter();

            if (!isHacked)
            {
                isPlayerInside = true;
                Debug.Log("[HackNode] Hacking started!");
            }
        }

        public void OnPlayerExit()
        {
            isPlayerInside = false;
            Debug.Log("[HackNode] Hacking interrupted!");
        }
        #endregion

        #region Gizmos
        //TODO implement gizmo color change based on hacking state
        #endregion
    }
}
