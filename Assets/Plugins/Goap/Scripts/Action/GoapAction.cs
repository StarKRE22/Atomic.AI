using System;

namespace AI.Goap
{
    public sealed class GoapAction : IGoapAction
    {
        public string Name => _name;
        public bool IsValid => _isValid.Invoke();
        public int Cost => _cost.Invoke();
        public LocalState Effects => _effects;
        public LocalState Conditions => _conditions;
        
        private readonly string _name;
        private readonly LocalState _effects;
        private readonly LocalState _conditions;
        private readonly Func<bool> _isValid;
        private readonly Func<int> _cost;

        public GoapAction(
            in string name,
            in LocalState effects,
            in LocalState conditions,
            in Func<bool> isValid,
            in Func<int> cost
        )
        {
            _name = name;
            _effects = effects;
            _conditions = conditions;
            _isValid = isValid;
            _cost = cost;
        }
    }
}