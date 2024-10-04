using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    public sealed class OverlapSphereComponent : MonoBehaviour
    {
        private static readonly Collider[] buffer = new Collider[32];

        [SerializeField]
        private Transform center;

        [SerializeField]
        private float radius;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;

        [Button]
        public void OverlapSphere(Predicate<GameObject> action)
        {
            int size = Physics.OverlapSphereNonAlloc(
                this.center.position,
                this.radius,
                buffer,
                this.layerMask,
                this.queryTriggerInteraction
            );

            for (int i = 0; i < size; i++)
            {
                Collider collider = buffer[i];
                GameObject target = collider.gameObject;
                if (action.Invoke(target))
                {
                    return;
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            if (this.center != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(this.center.position, this.radius);
            }
        }
    }
}