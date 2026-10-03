using Project.Scripts.Dennis.Game;
using UnityEngine;

namespace Project.Scripts.Dennis.Nodes
{
    public class EndNode : Node
    {
        [SerializeField] private bool isOpen = false;
        [SerializeField] private GameManager _gameManager;
        [ContextMenu("Test: Öffnen")]
        public void Open()
        {
            isOpen = true;
        }

        // Solange der Ausgang zu ist, kommt der Player gar nicht erst hin
        public override bool CanEnter()
        {
            return isOpen;
        }

        public override void OnPlayerEnter()
        {
            // TODO: später erst Exit-Animation, Win dann bei AE_OnExitEndFrame
            _gameManager.Win();
        }
    }
}
