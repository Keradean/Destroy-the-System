using UnityEngine;

namespace Project.Scripts.Dennis.Nodes
{
    public class EndNode : Node
    {
        [SerializeField] private bool isOpen = false;
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
            // TODO: GameManager.Win() aufrufen, sobald dieser halt existiert
            Debug.Log("Juhu Ich habe Gewonnen, Ihr Pfeifen: " + gameObject.name);
        }
    }
}
