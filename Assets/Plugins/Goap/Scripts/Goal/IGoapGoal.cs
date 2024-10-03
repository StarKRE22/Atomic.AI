using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapGoal
    {
        string Name { get; }
        
        bool IsValid { get; }
        int Priority { get; }
        IReadOnlyDictionary<string, bool> Effects { get; }
    }
}