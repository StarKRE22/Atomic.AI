using System.Collections;
using System.Collections.Generic;

// ReSharper disable UseDeconstruction

namespace AI.Goap
{
    public sealed class WorldState : IGoapState
    {
        private readonly Dictionary<string, bool> pairs;

        public WorldState()
        {
            this.pairs = new Dictionary<string, bool>();
        }

        public WorldState(params KeyValuePair<string, bool>[] values)
        {
            this.pairs = new Dictionary<string, bool>(values);
        }

        public bool this[in string key]
        {
            get { return this.pairs[key]; }
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
            for (int i = 0, count = other.Count; i < count; i++)
            {
                KeyValuePair<string, bool> pair = other[i];
                if (!this.pairs.TryGetValue(pair.Key, out bool value))
                    return false;

                if (value != pair.Value)
                    return false;
            }

            return true;
        }

        public bool OverlapsKeys(LocalState other)
        {
            for (int i = 0, count = other.Count; i < count; i++)
            {
                KeyValuePair<string, bool> pair = other[i];
                if (!this.pairs.ContainsKey(pair.Key))
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