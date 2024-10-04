using System;

namespace AI.Goap
{
    public sealed class GoapSensor : IGoapSensor
    {
        public string Name => _name;

        private readonly string _name;
        private readonly Action<WorldState> _onPopulate;

        public GoapSensor(in string name, in Action<WorldState> onPopulate)
        {
            _name = name;
            _onPopulate = onPopulate;
        }

        public void PopulateState(WorldState worldState)
        {
            _onPopulate.Invoke(worldState);
        }
    }
}