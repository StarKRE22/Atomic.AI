using System.Collections;
using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class WorldState : IGoapState
    {
        private readonly Dictionary<string, bool> pairs;

        public WorldState(params KeyValuePair<string, bool>[] values)
        {
            this.pairs = new Dictionary<string, bool>(values);
        }

        public bool TryGetValue(string key, out bool value)
        {
            return this.pairs.TryGetValue(key, out value);
        }

        public bool Equals(IGoapState other)
        {
            foreach (var (oKey, oValue) in other)
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