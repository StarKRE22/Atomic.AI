using UnityEngine;

namespace Game.Engine
{
    [RequireComponent(typeof(Animator))]
    public sealed class MoveAnimMechanics : MonoBehaviour
    {
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

        private Animator _animator;

        [SerializeField]
        private MoveComponent _moveComponent;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
        }

        private void LateUpdate()
        {
            _animator.SetBool(IsMovingHash, _moveComponent.IsMoving);
        }
    }
}