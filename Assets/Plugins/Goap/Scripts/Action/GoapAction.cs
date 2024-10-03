using System;
using System.Collections.Generic;
using static AI.Goap.IGoapAction;

namespace AI.Goap
{
    public sealed class GoapAction : IGoapAction
    {
        public IReadOnlyDictionary<string, bool> Effects => _effects;
        public IReadOnlyDictionary<string, bool> Conditions => _conditions;
        public bool IsValid => _isValid.Invoke();
        public int Cost => _cost.Invoke();
        public bool IsRunning => isRunning;

        private readonly IReadOnlyDictionary<string, bool> _effects;
        private readonly IReadOnlyDictionary<string, bool> _conditions;
        private readonly Func<bool> _isValid;
        private readonly Func<int> _cost;

        private readonly Func<float, Result> _onUpdate;
        private readonly Action _onStart;
        private readonly Action _onStop;
        private readonly Action _onCancel;

        private bool isRunning;

        public GoapAction(
            in IReadOnlyDictionary<string, bool> effects,
            in IReadOnlyDictionary<string, bool> conditions,
            in Func<bool> isValid,
            in Func<int> cost,
            in Func<float, Result> onUpdate,
            in Action onStart = null,
            in Action onStop = null,
            in Action onCancel = null
        )
        {
            _effects = effects;
            _conditions = conditions;
            _isValid = isValid;
            _cost = cost;

            _onUpdate = onUpdate;
            _onStart = onStart;
            _onStop = onStop;
            _onCancel = onCancel;
        }

        public Result Run(in float deltaTime)
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                _onStart?.Invoke();
            }

            Result result = _onUpdate.Invoke(deltaTime);

            if (result != Result.RUNNING)
            {
                this.isRunning = false;
                _onStop?.Invoke();
            }

            return result;
        }

        public bool Cancel()
        {
            if (!this.isRunning)
                return false;

            this.isRunning = false;
            
            _onCancel?.Invoke();
            _onStop?.Invoke();
            return true;
        }
    }
}