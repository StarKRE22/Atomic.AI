using System;
using Sirenix.OdinInspector;

namespace AI.Goap
{
    [Serializable]
    public struct StateProperty
    {
        [HorizontalGroup]
        public string key;

        [HorizontalGroup]
        public bool value;

        public StateProperty(string key, bool value)
        {
            this.key = key;
            this.value = value;
        }
    }
}