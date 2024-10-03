using System.Collections.Generic;

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static IReadOnlyDictionary<string, bool> Dictionary(params KeyValuePair<string, bool>[] values)
        {
            return new Dictionary<string, bool>(values);
        }
    }
}