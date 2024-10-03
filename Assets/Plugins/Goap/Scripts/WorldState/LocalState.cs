using System.Collections;
using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class LocalState : IGoapState
    {
        private readonly KeyValuePair<string, bool>[] pairs;

        public LocalState(params KeyValuePair<string, bool>[] pairs)
        {
            this.pairs = pairs;
        }

        public bool TryGetValue(string key, out bool value)
        {
            for (int i = 0, count = this.pairs.Length; i < count; i++)
            {
                (string pKey, bool pValue) = this.pairs[i];
                if (pKey == key)
                {
                    value = pValue;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool Equals(IGoapState other)
        {
            for (int i = 0, count = this.pairs.Length; i < count; i++)
            {
                (string key, bool value) = this.pairs[i];

                if (!other.TryGetValue(key, out bool otherValue))
                    return false;

                if (value != otherValue)
                    return false;
            }

            return true;
        }

        public IEnumerator<KeyValuePair<string, bool>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private struct Enumerator : IEnumerator<KeyValuePair<string, bool>>
        {
            public KeyValuePair<string, bool> Current => this.current;
            object IEnumerator.Current => this.Current;

            private readonly KeyValuePair<string, bool>[] _pairs;
            private KeyValuePair<string, bool> current;
            private int index;

            public Enumerator(LocalState state)
            {
                _pairs = state.pairs;
                this.current = default;
                this.index = -1;
            }

            public bool MoveNext()
            {
                this.index++;
                
                if (this.index < _pairs.Length)
                {
                    this.current = _pairs[this.index];
                    return true;
                }

                this.current = default;
                return false;
            }

            public void Reset()
            {
                this.current = default;
                this.index = -1;
            }
            
            public void Dispose()
            {
                //Do nothing...
            }
        }
    }
}