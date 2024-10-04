using System;
using System.Collections.Generic;
using AI.Goap;
using Atomic.AI;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapAgentAsset : GoapAgentAsset<IBlackboard>
    {
        [SerializeField]
        private GoapGoalAsset[] goals;

        [SerializeField]
        private GoapActionAsset[] actions;

        protected override IEnumerable<IGoapGoalAsset<IBlackboard>> Goals => this.goals;
        protected override IEnumerable<IGoapActionAsset<IBlackboard>> Actions => this.actions;
        protected override IEnumerable<IGoapSensorAsset<IBlackboard>> Sensors { get; }
    }
}