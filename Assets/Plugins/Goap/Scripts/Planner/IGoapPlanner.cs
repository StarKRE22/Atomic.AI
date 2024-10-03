using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapPlanner
    {
        bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IGoapAction[] actions,
            out List<IGoapAction> plan
        );
        
        bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IGoapAction[] actions,
            List<IGoapAction> plan
        );
    }
}