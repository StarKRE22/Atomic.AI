using System;
using AI.Goap;
using Atomic.AI;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapGoalAsset : IGoapGoalAsset<IBlackboard>
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private SerializedFact[] result;

        [SerializeReference]
        private IBlackboardCondition isValid = null;
        
        [SerializeReference]
        private IIntBlackboardFunction priority = null;

        public IGoapGoal Create(IBlackboard source)
        {
            return new GoapGoal(
                this.name,
                this.result.ToLocalState(),
                () => this.isValid.Invoke(source),
                () => this.priority.Invoke(source)
            );
        }
    }
}