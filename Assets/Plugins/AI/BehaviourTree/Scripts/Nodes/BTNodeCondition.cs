using System;
using UnityEngine;

namespace Atomic.AI
{
    [Serializable]
    public sealed class BTNodeCondition : BTNode
    {
        public override string Name => this.name;

        [SerializeField]
        private string name;
        
        [SerializeReference]
        private IBlackboardCondition condition = default;
        
        protected override BTResult OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            return this.condition.Invoke(blackboard) ? BTResult.SUCCESS : BTResult.FAILURE;
        }
    }
}