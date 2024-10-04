using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapState : IEnumerable<KeyValuePair<string, bool>>
    {
        bool this[in string key] { get; }
        bool TryGetValue(in string key, out bool value);
    }
}