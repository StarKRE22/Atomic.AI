using System;
using System.Collections.Generic;

namespace AI.Goap
{
    public static partial class Extensions
    {
        public static LocalState ToLocalState(this StateProperty[] properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            int count = properties.Length;
            var facts = new KeyValuePair<string, bool>[count];

            for (int i = 0; i < count; i++)
            {
                StateProperty fact = properties[i];
                facts[i] = new KeyValuePair<string, bool>(fact.key, fact.value);
            }

            return new LocalState(facts);
        }
    }
}