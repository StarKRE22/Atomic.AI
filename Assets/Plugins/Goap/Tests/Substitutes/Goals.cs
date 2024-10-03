

// ReSharper disable ArgumentsStyleAnonymousFunction
// ReSharper disable ArgumentsStyleOther

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static readonly IGoapGoal GoalStub = new GoapGoal(
            name: nameof(GoalStub),
            isValid: () => true,
            priority: () => 1,
            result: new LocalState()
        );
    }
}