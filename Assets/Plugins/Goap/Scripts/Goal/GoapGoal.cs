using System;
using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class GoapGoal : IGoapGoal
    {
        public string Name => _name;
        public bool IsValid => _isValid.Invoke();
        public int Priority => _priority.Invoke();
        public LocalState Result => _effects;

        private readonly string _name;
        private readonly Func<bool> _isValid;
        private readonly Func<int> _priority;
        private readonly LocalState _effects;

        public GoapGoal(
            in string name,
            in Func<bool> isValid,
            in Func<int> priority,
            params KeyValuePair<string, bool>[] effects
        )
        {
            _name = name;
            _isValid = isValid;
            _priority = priority;
            _effects = new LocalState(effects);
        }
        
        public GoapGoal(
            in string name,
            in Func<bool> isValid,
            in Func<int> priority,
            LocalState effects
        )
        {
            _name = name;
            _isValid = isValid;
            _priority = priority;
            _effects = effects;
        }
    }
}