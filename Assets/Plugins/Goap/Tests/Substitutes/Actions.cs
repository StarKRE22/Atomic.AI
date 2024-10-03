using System.Collections.Generic;

// ReSharper disable ArgumentsStyleStringLiteral
// ReSharper disable ArgumentsStyleOther
// ReSharper disable ArgumentsStyleAnonymousFunction

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static readonly IGoapAction ActionStub = new GoapAction(
            name: nameof(ActionStub),
            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
            conditions: new LocalState(EnemyAlive(true)),
            isValid: () => true,
            cost: () => 10,
            onUpdate: null
        );
    }
}