using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapGoal
    {
        string Name { get; }
        
        IGoapState Result { get; }
        bool IsValid { get; }
        int Priority { get; }
    }
}