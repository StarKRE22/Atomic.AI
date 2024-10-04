using System;
using AI.Goap;
using Atomic.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapGoalAsset : IGoapGoalAsset<IBlackboard>
    {
        [GUIColor(1f, 0.92156863f, 0.015686275f)]
        [SerializeField]
        private string name;

        [SerializeField]
        private StateProperty[] result;

        [SerializeReference]
        private IBlackboardCondition isValid = null;

        [SerializeReference]
        private IBlackboardInt priority = null;

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