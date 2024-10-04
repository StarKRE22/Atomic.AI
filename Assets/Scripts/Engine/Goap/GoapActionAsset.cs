using System;
using AI.Goap;
using Atomic.AI;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapActionAsset : IGoapActionAsset<IBlackboard>
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private SerializedFact[] effects;

        [SerializeField]
        private SerializedFact[] conditions;

        [SerializeReference]
        private IBlackboardCondition isValid = null;

        [SerializeReference]
        private IIntBlackboardFunction cost = null;

        public IGoapAction Create(IBlackboard source)
        {
            return new GoapAction(
                this.name,
                this.effects.ToLocalState(),
                this.conditions.ToLocalState(),
                () => this.isValid.Invoke(source),
                () => this.cost.Invoke(source)
            );
        }
    }
}