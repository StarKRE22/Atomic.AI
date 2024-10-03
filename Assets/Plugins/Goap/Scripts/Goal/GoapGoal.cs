using System;
using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class GoapGoal : IGoapGoal
    {
        public string Name => _name;
        public bool IsValid => _isValid.Invoke();
        public int Priority => _priority.Invoke();
        public LocalState Result => _result;

        private readonly string _name;
        private readonly Func<bool> _isValid;
        private readonly Func<int> _priority;
        private readonly LocalState _result;

        public GoapGoal(
            in string name,
            in Func<bool> isValid,
            in Func<int> priority,
            params KeyValuePair<string, bool>[] result
        )
        {
            _name = name;
            _isValid = isValid;
            _priority = priority;
            _result = new LocalState(result);
        }
        
        public GoapGoal(
            in string name,
            in Func<bool> isValid,
            in Func<int> priority,
            in LocalState result
        )
        {
            _name = name;
            _isValid = isValid;
            _priority = priority;
            _result = result;
        }
    }
}