using UnityEngine;
using Project.Scripts.Dennis.Nodes;

namespace Project.Scripts.Dennis.Network
{
    // Erzeugt beim Spielstart für jede Verbindung zwischen zwei Nodes eine Leitung
    public class LinkBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject _linkPrefab;

        void Start()
        {
            Node[] nodes = FindObjectsByType<Node>();
            foreach (Node node in nodes)
            {
                Node rightNeighbor = node.GetNeighbor(Direction.Right);
                if (rightNeighbor != null)
                {
                    BuildLink(node, rightNeighbor);
                }
                Node topNeighbor = node.GetNeighbor(Direction.Up);
                if (topNeighbor != null)
                {
                    BuildLink(node, topNeighbor);
                }
            }
        }
        private void BuildLink(Node from, Node to)
        {
            Vector3 middle = Vector3.Lerp(from.transform.position, to.transform.position, 0.5f);
            
            GameObject link = Instantiate(_linkPrefab, middle, Quaternion.identity);
            link.transform.LookAt(to.transform.position);
            link.transform.localScale = new Vector3(
                link.transform.localScale.x,
                link.transform.localScale.y,
                Vector3.Distance(from.transform.position, to.transform.position));
        }
    }
}