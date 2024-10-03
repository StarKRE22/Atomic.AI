using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapState
    {
        IReadOnlyDictionary<string, bool> Current { get; }
    }
}