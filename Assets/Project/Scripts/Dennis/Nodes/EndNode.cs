using UnityEngine;

namespace Project.Scripts.Dennis.Nodes
{
    public class EndNode : Node
    {
        [SerializeField] private bool isOpen = false;
        public void Open()
        {
            isOpen = true;
        }

        public override void OnPlayerEnter()
        {
            if (!isOpen)
            {
                Debug.Log("Du kommst hier ned rein!!!: " + gameObject.name);
                // TODO: Add logic for when the player enters the end node and it is open
            }
            else
            {
                // TODO: GameManager.Win() aufrufen, sobald dieser halt existiert
                Debug.Log("Juhu Ich habe Gewonnen, Ihr Pfeifen: " + gameObject.name);
            }
        }
    }
}
