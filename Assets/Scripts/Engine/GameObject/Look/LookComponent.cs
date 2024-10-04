using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    public sealed class LookComponent : MonoBehaviour
    {
        public Vector3 Direction
        {
            get => this.direction;
            set => this.direction = value;
        }

        [ShowInInspector]
        private Vector3 direction;

        private void FixedUpdate()
        {
            if (this.direction != Vector3.zero)
            {
                this.transform.rotation = Quaternion.LookRotation(this.direction, Vector3.up);
            }
        }
    }
}