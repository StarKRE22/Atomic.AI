using System.Collections;
using System.Collections.Generic;
// ReSharper disable UseDeconstruction

namespace AI.Goap
{
    public sealed class WorldState : IEnumerable<KeyValuePair<string, bool>>
    {
        private readonly Dictionary<string, bool> pairs;

        public WorldState(params KeyValuePair<string, bool>[] values)
        {
            this.pairs = new Dictionary<string, bool>(values);
        }

        public bool TryGetValue(in string key, out bool value)
        {
            return this.pairs.TryGetValue(key, out value);
        }

        public bool Overlaps(in KeyValuePair<string, bool> other)
        {
            (string oKey, bool oValue) = other;
            return this.pairs.TryGetValue(oKey, out bool value) && value == oValue;
        }

        public bool Overlaps(LocalState other)
        {
            foreach ((string oKey, bool oValue) in other)
            {
                if (!this.pairs.TryGetValue(oKey, out bool value))
                    return false;
        
                if (value != oValue)
                    return false;
            }
        
            return true;
        }

        public IEnumerator<KeyValuePair<string, bool>> GetEnumerator()
        {
            return this.pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}