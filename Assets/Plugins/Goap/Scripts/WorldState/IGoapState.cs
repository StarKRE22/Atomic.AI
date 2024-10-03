using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapState : IEnumerable<KeyValuePair<string, bool>>
    {
        bool TryGetValue(string key, out bool value);
        
        bool Equals(IGoapState other);
    }
}