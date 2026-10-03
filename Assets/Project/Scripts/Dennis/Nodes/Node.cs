using System;
using UnityEngine;

namespace Project.Scripts.Dennis.Nodes
{
    public abstract class Node : MonoBehaviour
    {
        [SerializeField] private Node up;
        [SerializeField] private Node down;
        [SerializeField] private Node left;
        [SerializeField] private Node right;

        // Switch expression to get the neighbor node based on the direction
        public Node GetNeighbor(Direction direction)
        {
            return direction switch
            {
                Direction.Up => up,
                Direction.Down => down,
                Direction.Left => left,
                Direction.Right => right,
                _ => throw new System.ArgumentException("Invalid Direction")
            };
        }
        // Darf der Player diesen Node betreten? Gesperrte Nodes (z.B. EndNode, CoreNode) überschreiben das
        public virtual bool CanEnter()
        {
            return true;
        }
        // virtual method that can be overridden by derived classes to handle player entering the node
        public virtual void OnPlayerEnter()
        {
            Debug.Log("Player entered node: " + gameObject.name);
        }
        // virtual method that can be overridden by derived classes to handle player exiting the node
        public virtual void OnPlayerExit()
        {
            Debug.Log("Player exited node: " + gameObject.name);
        }

        // Draws a half line from the current node to the neighbor node in the editor for visualization
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            DrawHalfLine(up);
            DrawHalfLine(down);
            DrawHalfLine(left);
            DrawHalfLine(right);
        }
        // Draws a half line from the current node to the neighbor node
        void DrawHalfLine(Node neighbor)
        {
            if (neighbor == null) return;
            
            Vector3 middle = Vector3.Lerp(transform.position, neighbor.transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, middle);
        }
    }
}
