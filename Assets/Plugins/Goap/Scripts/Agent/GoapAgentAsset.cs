using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Goap
{
    public abstract class GoapAgentAsset<TSource>
    {
        [SerializeReference]
        private IGoapPlannerAsset planner;

        protected abstract IEnumerable<IGoapGoalAsset<TSource>> Goals { get; }
        protected abstract IEnumerable<IGoapActionAsset<TSource>> Actions { get; }
        protected abstract IEnumerable<IGoapSensorAsset<TSource>> Sensors { get; }
        
        public GoapAgent Create(TSource source)
        {
            return new GoapAgent(
                this.planner?.Create(),
                this.Goals?.Select(it => it.Create(source)),
                this.Actions?.Select(it => it.Create(source)),
                this.Sensors?.Select(it => it.Create(source))
            );
        }
    }
}