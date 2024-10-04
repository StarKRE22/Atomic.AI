using System;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable MemberInitializerValueIgnored

namespace Game.Engine
{
    public sealed class MoveComponent : MonoBehaviour
    {
        public event Action OnMove;
        
        public bool IsMoving => this.isMoving;
        public Vector3 MoveDirection => this.moveDirection;

        [SerializeField]
        private float moveSpeed = 3.0f;

        [ShowInInspector, ReadOnly]
        private Vector3 moveDirection;

        [ShowInInspector, ReadOnly]
        private bool isMoving;

        public void MoveStep(in Vector3 direction)
        {
            this.moveDirection = direction;
            this.isMoving = true;
        }

        private void FixedUpdate()
        {
            if (this.isMoving)
            {
                this.transform.position += this.moveSpeed * Time.fixedDeltaTime * this.moveDirection;
                this.isMoving = false;
                this.OnMove?.Invoke();
            }
        }
    }
}