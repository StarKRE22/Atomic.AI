using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapPlanner
    {
        bool Plan(
            in IReadOnlyDictionary<string, bool> worldState,
            in IGoapGoal goal,
            in IEnumerable<IGoapAction> actions,
            out List<IGoapAction> plan
        );
    }
}