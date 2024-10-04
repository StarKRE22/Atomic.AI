

// ReSharper disable ArgumentsStyleAnonymousFunction
// ReSharper disable ArgumentsStyleOther

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static readonly IGoapGoal GoalStub = new GoapGoal(
            name: nameof(GoalStub),
            result: new LocalState(), isValid: () => true, priority: () => 1);
    }
}