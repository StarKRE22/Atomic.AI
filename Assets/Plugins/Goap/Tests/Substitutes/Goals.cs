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
            result: EnemyAlive(false)
        );

        public static readonly IGoapGoal HealingGoal = new GoapGoal(
            name: nameof(HealingGoal),
            isValid: () => true,
            priority: () => 5,
            result: Injured(false)
        );
    }
}