using System;
using AI.Goap;
using Atomic.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapActionAsset : IGoapActionAsset<IBlackboard>
    {
        [GUIColor(1f, 0.92156863f, 0.015686275f)]
        [SerializeField]
        private string name;

        [SerializeField]
        private StateProperty[] effects;

        [SerializeField]
        private StateProperty[] conditions;

        [SerializeReference]
        private IBlackboardCondition isValid = null;

        [SerializeReference]
        private IBlackboardInt cost = null;

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