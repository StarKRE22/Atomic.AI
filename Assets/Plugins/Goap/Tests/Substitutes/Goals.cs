using System.Collections.Generic;

// ReSharper disable ArgumentsStyleAnonymousFunction
// ReSharper disable ArgumentsStyleOther

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static readonly IGoapGoal DestroyEnemyGoal = new GoapGoal(
            name: nameof(DestroyEnemyGoal),
            isValid: () => true,
            priority: () => 1,
            effects: EnemyAlive(false)
        );
    }
}