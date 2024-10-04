using System;
using System.Collections.Generic;
using AI.Goap;
using Atomic.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapAgentAsset : GoapAgentAsset<IBlackboard>
    {
        [Header("Goals")]
        [SerializeField, HideLabel]
        private GoapGoalAsset[] goals;

        [Header("Actions")]
        [SerializeField, HideLabel]
        private GoapActionAsset[] actions;

        [Header("Sensors")]
        [SerializeField, HideLabel]
        private GoapSensorAsset[] sensors;

        protected override IEnumerable<IGoapGoalAsset<IBlackboard>> Goals => this.goals;
        protected override IEnumerable<IGoapActionAsset<IBlackboard>> Actions => this.actions;
        protected override IEnumerable<IGoapSensorAsset<IBlackboard>> Sensors => this.sensors;
    }
}