using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    [RequireComponent(typeof(ResourceStorageComponent))]
    public sealed class TakeResourceComponent : MonoBehaviour
    {
        private ResourceStorageComponent _resourceStorage;

        [SerializeField]
        private int extractCount;

        private void Awake()
        {
            _resourceStorage = this.GetComponent<ResourceStorageComponent>();
        }

        [Button]
        public bool TakeResources(GameObject target)
        {
            if (!target.TryGetComponent(out ResourceStorageComponent targetStorage))
            {
                return false;
            }

            if (targetStorage.IsEmpty())
            {
                return false;
            }

            int extractCount = Math.Min(this.extractCount, targetStorage.Current);

            targetStorage.RemoveResources(extractCount);
            _resourceStorage.AddResources(this.extractCount);
            return true;
        }
    }
}