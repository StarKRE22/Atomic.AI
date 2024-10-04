using System;
using AI.Goap;
using Atomic.AI;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapAgentInstaller : IBlackboardInstaller
    {
        [SerializeField]
        private GoapAgentAsset agentAsset;

        public void Install(IBlackboard blackboard)
        {
            try
            {
                GoapAgent agent = this.agentAsset?.Create(blackboard);
                blackboard.SetGoapAgent(agent);
            }
            catch (Exception _)
            {
                // ignored
            }
        }
    }
}