using System;
using Atomic.AI;
using Game;
using Game.Engine;
using UnityEngine;

namespace Engine
{
    [Serializable]
    public sealed class DistanceToTargetCostBlackboardFunction : IBlackboardInt
    {
        [SerializeField]
        private float multiplier = 1;

        [SerializeField, BlackboardKey]
        private int targetKey;
        
        public int Invoke(IBlackboard blackboard)
        {
            if (!blackboard.TryGetCharacter(out GameObject character) ||
                !blackboard.TryGetObject(this.targetKey, out GameObject target))
            {
                return int.MaxValue;
            }
            
            float distance = DistanceUtils.DistanceBetween(character, target);
            return (int) (distance * this.multiplier);
        }
    }
}