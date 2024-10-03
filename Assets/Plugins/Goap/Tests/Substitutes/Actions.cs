using System.Collections.Generic;
// ReSharper disable ArgumentsStyleStringLiteral
// ReSharper disable ArgumentsStyleOther
// ReSharper disable ArgumentsStyleAnonymousFunction

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static readonly IGoapAction MoveAtEnemyAction = new GoapAction(
            name: "MoveAtEnemy",
            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
            conditions: new LocalState(EnemyAlive(true)),
            isValid: () => true,
            cost: () => 10,
            onUpdate: null
        );

        public static readonly IGoapAction SelfTreatmentAction = new GoapAction(
            name: "SelfTreatment",
            effects: new LocalState(Injured(false)),
            conditions: new LocalState(),
            isValid: () => true,
            cost: () => 10,
            onUpdate: null
        );

        // public static readonly IGoapAction MoveToMedic = new GoapAction(
        //     name: "MoveToMedic",
        //     effects: new LocalState(AtEnemy(true), NearEnemy(true)),
        //     conditions: new LocalState(EnemyAlive(true)),
        //     isValid: () => true,
        //     cost: () => 10,
        //     onUpdate: null
        //
        // );

        // public static readonly IGoapAction UseHealthKitAction = new GoapAction(
        //     name: "UseHealthKit",
        //     effects: new LocalState(Injured(false)),
        //     conditions: 
        //     cost: 5,
        //     resultState: new FactState(new Fact("isInjured", false)),
        //     requiredState: new FactState()
        // );
    }
}