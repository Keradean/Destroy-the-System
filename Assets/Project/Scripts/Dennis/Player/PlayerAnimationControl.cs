using UnityEngine;
using UnityEngine.Animations;

namespace Project.Scripts.Dennis.Player
{
    public class PlayerAnimationControl : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private Animator _animator;
        private PlayerMoveComponent _playerMoveComponent;
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerMoveComponent = GetComponent<PlayerMoveComponent>();
        }
        private void Update()
        {
            _animator.SetBool(IsMoving, _playerMoveComponent.IsMoving);
        }
    }
}
