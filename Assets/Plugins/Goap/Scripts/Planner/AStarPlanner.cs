using System;
using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class AStarPlanner : IGoapPlanner
    {
        public bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in ICollection<IGoapAction> actions,
            out List<IGoapAction> plan
        )
        {
            if (worldState == null)
                throw new ArgumentNullException(nameof(worldState));

            if (goal == null)
                throw new ArgumentNullException(nameof(goal));

            if (actions == null)
                throw new ArgumentNullException(nameof(actions));

            plan = default;

            if (actions.Count == 0)
                return false;
            
            if (worldState.Equals(goal.Result))
            {
                plan = new List<IGoapAction>(0);
                return true;
            }
            
            throw new System.NotImplementedException();
        }
    }
}