using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class ResourceStorageComponent : MonoBehaviour
    {
        public event Action OnStateChanged;

        public int Current => this.current;
        public int FreeSlots => this.capacity - this.current;
        
        [SerializeField]
        private int capacity;

        [SerializeField]
        private int current;

        public bool CanAddResources(int range)
        {
            return this.current + range <= this.capacity;
        }

        [Button]
        public void AddResources(int range)
        {
            this.current = Mathf.Min(this.capacity, this.current + range);
            this.OnStateChanged?.Invoke();
        }

        [Button]
        public bool RemoveResources(int count)
        {
            if (this.current - count >= 0)
            {
                this.current -= count;
                this.OnStateChanged?.Invoke();
                return true;
            }

            return false;
        }

        [Button]
        public int ExtractAllResources()
        {
            int result = this.current;
            this.current = 0;
            this.OnStateChanged?.Invoke();
            return result;
        }

        [Button]
        public void Clear()
        {
            this.current = 0;
            this.OnStateChanged?.Invoke();
        }
        
        public bool IsFull()
        {
            return this.current == this.capacity;
        }

        public bool IsNotFull()
        {
            return this.current < this.capacity;
        }

        public bool IsNotEmpty()
        {
            return this.current > 0;
        }

        public bool IsEmpty()
        {
            return this.current == 0;
        }
    }
}