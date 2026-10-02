using UnityEngine;

namespace Project.Scripts.Dennis.Nodes
{
    public class StartNode : Node
    {
        [SerializeField] private Transform player;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            player.position = transform.position;
        }
        
    }
}
