using System;
using System.Collections.Generic;

namespace AI.Goap
{
    [Serializable]
    public struct SerializedFact
    {
        public string key;
        public bool value;
    }

    public static class SerializedFactUtils
    {
        public static LocalState ToLocalState(this SerializedFact[] state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            int count = state.Length;
            var facts = new KeyValuePair<string, bool>[count];

            for (int i = 0; i < count; i++)
            {
                SerializedFact fact = state[i];
                facts[i] = new KeyValuePair<string, bool>(fact.key, fact.value);
            }

            return new LocalState(facts);
        }
    }
}