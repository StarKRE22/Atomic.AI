using System;
using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class PlannerMock : IGoapPlanner
    {
        private readonly Func<WorldState, IGoapGoal, IReadOnlyList<IGoapAction>, List<IGoapAction>, bool> _plan;

        public PlannerMock(Func<WorldState, IGoapGoal, IReadOnlyList<IGoapAction>, List<IGoapAction>, bool> plan = null)
        {
            _plan = plan;
        }

        public bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IReadOnlyList<IGoapAction> actions,
            out List<IGoapAction> plan
        )
        {
            plan = new List<IGoapAction>();
            return this.Plan(worldState, goal, actions, plan);
        }

        public bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IReadOnlyList<IGoapAction> actions,
            List<IGoapAction> plan
        )
        {
            return _plan != null && _plan.Invoke(worldState, goal, actions, plan);
        }
    }
}