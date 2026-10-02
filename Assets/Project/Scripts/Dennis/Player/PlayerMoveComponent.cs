using Project.Scripts.Dennis.Nodes;
using UnityEngine;
using UnityEngine.Animations;

namespace Project.Scripts.Dennis.Player
{
    public class PlayerMoveComponent : MonoBehaviour
    {
        [SerializeField] Node _currentNode;
        [SerializeField] private float _moveSpeed = 5f;
        
        PlayerInputReader _input;
        public bool IsMoving => _targetNode != null;
        private Node _targetNode;
        ///////////////////////////////////////////////////////////
        void Awake()
        {
            _currentNode.OnPlayerEnter();
            _input = GetComponent<PlayerInputReader>();
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
                Node nextNode = _currentNode.GetNeighbor(direction);
                if(nextNode == null) return;
                _targetNode = nextNode;
                transform.LookAt(nextNode.transform.position);
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
                        transform.LookAt(_targetNode.transform.position);
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, _targetNode.transform.position, _moveSpeed * Time.deltaTime);
                if (transform.position != _targetNode.transform.position) return;
                _currentNode = _targetNode;
                _targetNode = null;
                _currentNode.OnPlayerEnter();
            }
        }
    }
}
