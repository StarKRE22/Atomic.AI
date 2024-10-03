using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class GoapAgent
    {
        private readonly HashSet<IGoapGoal> goals;
        private readonly HashSet<IGoapAction> actions;

        private readonly IGoapPlanner _planner;
        private readonly IGoapState _state;

        public GoapAgent(
            in IGoapPlanner planner,
            in IGoapState state,
            in IEnumerable<IGoapGoal> goals = null,
            in IEnumerable<IGoapAction> actions = null
        )
        {
            this.goals = goals != null ? new HashSet<IGoapGoal>(goals) : new HashSet<IGoapGoal>();
            this.actions = actions != null ? new HashSet<IGoapAction>(actions) : new HashSet<IGoapAction>();

            _planner = planner;
            _state = state;
        }
    }
}