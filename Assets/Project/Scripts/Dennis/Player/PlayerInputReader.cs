using System;
using Project.Scripts.Dennis.Nodes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Dennis.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveAction;

        private void OnEnable()
        {
            moveAction.action.Enable();
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
        }

        public bool GetDirection(out Direction direction)
        {
            direction = Direction.Up; 
            if(!moveAction.action.WasPressedThisFrame()) return false;
            Vector2 input = moveAction.action.ReadValue<Vector2>();
            if(input == Vector2.zero)
            {
                return false;
            }
            if (Math.Abs(input.x) >= Math.Abs(input.y))
            {
                if(input.x > 0) direction = Direction.Right;
                else direction = Direction.Left;
            }
            else
            {
                if(input.y > 0) direction = Direction.Up;
                else direction = Direction.Down;
            }
            return true;
        }
    }
}
