// ReSharper disable ArgumentsStyleOther
// ReSharper disable ArgumentsStyleAnonymousFunction
namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static readonly IGoapSensor SensorStub = new GoapSensor(
            name: nameof(SensorStub),
            onPopulate: _ => { }
        );
    }
}