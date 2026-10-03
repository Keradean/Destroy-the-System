using Project.Scripts.Dennis.Game;
using UnityEngine;

namespace Project.Scripts.Dennis.Nodes
{
    // Gesperrt, bis genug Nodes gehackt sind. Beim Betreten kommt der Boss.
    public class CoreNode : Node
    {
        [SerializeField] private GameManager _gameManager;

        public override bool CanEnter()
        {
            return _gameManager != null && _gameManager.IsCoreUnlocked;
        }

        public override void OnPlayerEnter()
        {
            base.OnPlayerEnter();
            _gameManager.CoreReached();
        }
    }
}
