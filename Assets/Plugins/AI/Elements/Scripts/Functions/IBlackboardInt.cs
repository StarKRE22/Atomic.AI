using System;
using Atomic.AI;
using UnityEngine;

namespace Game.Engine
{
    public interface IBlackboardInt : IBlackboardFunction<int>
    {
    }
    
    [Serializable]
    public sealed class BlackboardIntConst : IBlackboardInt
    {
        [SerializeField]
        private int value;
        
        public int Invoke(IBlackboard blackboard) => this.value;
    }
}