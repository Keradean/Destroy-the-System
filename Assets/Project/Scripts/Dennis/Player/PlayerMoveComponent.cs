using Project.Scripts.Dennis.Nodes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Dennis.Player
{
    public class PlayerMoveComponent : MonoBehaviour
    {
        [SerializeField] private Node _currentNode;
        PlayerInputReader _input;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _input = GetComponent<PlayerInputReader>();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
