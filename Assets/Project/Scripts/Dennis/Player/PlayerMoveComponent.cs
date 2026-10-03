using Project.Scripts.Dennis.Nodes;
using UnityEngine;
using UnityEngine.Animations;

namespace Project.Scripts.Dennis.Player
{
    public class PlayerMoveComponent : MonoBehaviour
    {
        [SerializeField] Node _currentNode;

        PlayerInputReader _input;
        public bool IsMoving => _targetNode != null;
        private Node _targetNode;
        private float _moveSpeed;
        private Direction? _bufferedDirection;   // Tastendruck während der Bewegung, wird beim Ankommen ausgeführt
        ///////////////////////////////////////////////////////////
        void Awake()
        {
            _currentNode.OnPlayerEnter();
            _input = GetComponent<PlayerInputReader>();
            _moveSpeed = GetComponent<PlayerSetup>().Data.moveSpeed;
        }
        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_targetNode == null)
            {
                if(!_input.GetDirection(out Direction direction)) return;
                TryStartMove(direction);
            }
            else
            {
                if (_input.GetDirection(out Direction direction))
                {
                    if (_targetNode.GetNeighbor(direction) == _currentNode)
                    {
                        Node oldTarget = _targetNode;
                        _targetNode = _currentNode;
                        _currentNode = oldTarget;
                        _bufferedDirection = null;   // Umkehren ersetzt alles, was vorher gemerkt war
                        transform.LookAt(_targetNode.transform.position);
                    }
                    else
                    {
                        _bufferedDirection = direction;   // letzter Tastendruck gewinnt
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, _targetNode.transform.position, _moveSpeed * Time.deltaTime);
                if (transform.position != _targetNode.transform.position) return;
                _currentNode = _targetNode;
                _targetNode = null;
                _currentNode.OnPlayerEnter();

                if (_bufferedDirection == null) return;
                Direction buffered = _bufferedDirection.Value;
                _bufferedDirection = null;
                TryStartMove(buffered);   // geht es in die Richtung nicht weiter, bleibt er einfach stehen
            }
        }

        // Startet die Bewegung zum Nachbarn in dieser Richtung, falls es ihn gibt und er betreten werden darf
        private void TryStartMove(Direction direction)
        {
            Node nextNode = _currentNode.GetNeighbor(direction);
            if(nextNode == null) return;
            if(!nextNode.CanEnter()) return;   // gesperrter Node, stehen bleiben
            _currentNode.OnPlayerExit();
            _targetNode = nextNode;
            transform.LookAt(nextNode.transform.position);
        }
    }
}
