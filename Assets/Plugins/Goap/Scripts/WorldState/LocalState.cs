using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AI.Goap
{
    public sealed class LocalState : IGoapState
    {
        public int Count => this.pairs.Length;
        
        private readonly KeyValuePair<string, bool>[] pairs;

        public LocalState(IEnumerable<KeyValuePair<string, bool>> pairs)
        {
            this.pairs = pairs.ToArray();
        }
        
        public LocalState(params KeyValuePair<string, bool>[] pairs)
        {
            this.pairs = pairs;
        }
        
        public bool this[in string key]
        {
            get
            {
                for (int i = 0, count = this.pairs.Length; i < count; i++)
                {
                    (string pKey, bool pValue) = this.pairs[i];
                    if (pKey == key)
                        return pValue;
                }

                throw new KeyNotFoundException(nameof(key));
            }
        }

        public KeyValuePair<string, bool> this[in int index]
        {
            get { return this.pairs[index]; }
        }

        public bool Overlaps(in LocalState other)
        {
            return this.Overlaps(other.pairs);
        }

        public bool Overlaps(in KeyValuePair<string, bool>[] other)
        {
            int count = other.Length;

            if (this.pairs.Length < count)
                return false;

            for (int i = 0; i < count; i++)
            {
                if (!this.Overlaps(other[i]))
                    return false;
            }

            return true;
        }

        public bool Overlaps(in KeyValuePair<string, bool> other)
        {
            return this.Overlaps(other.Key, other.Value);
        }

        public bool Overlaps(in string key, bool value)
        {
            for (int i = 0, count = this.pairs.Length; i < count; i++)
            {
                (string k, bool v) = this.pairs[i];
                if (k == key && v == value)
                    return true;
            }

            return false;
        }

       

        public bool TryGetValue(in string key, out bool value)
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