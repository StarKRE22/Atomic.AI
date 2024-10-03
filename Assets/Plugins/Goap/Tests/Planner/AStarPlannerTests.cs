using System;
using System.Collections.Generic;
using NUnit.Framework;
using static AI.Goap.Substitutes;

// ReSharper disable ArgumentsStyleAnonymousFunction

// ReSharper disable ArgumentsStyleOther
// ReSharper disable ArgumentsStyleNamedExpression

namespace AI.Goap
{
    public sealed class AStarPlannerTests
    {
        [Test]
        public void CreatePlanner()
        {
            //Act:
            AStarPlanner planner = new AStarPlanner();

            //Assert:
            Assert.AreEqual(1, planner.heuristicPoints);
            Assert.AreEqual(int.MaxValue, planner.heuristicUndefined);
        }

        [TestCaseSource(nameof(ArgumentNullExceptionCases))]
        public void ArgumentNullException(WorldState worldState, IGoapGoal goal, IGoapAction[] actions)
        {
            //Arrange:
            AStarPlanner planner = new AStarPlanner();

            //Assert:
            Assert.Catch<ArgumentNullException>(() => { planner.Plan(worldState, goal, actions, out _); });
        }

        private static IEnumerable<TestCaseData> ArgumentNullExceptionCases()
        {
            yield return new TestCaseData(null, GoalStub, new[] {ActionStub})
                .SetName("World State");

            yield return new TestCaseData(new WorldState(), null, new[] {ActionStub})
                .SetName("Goal");

            yield return new TestCaseData(new WorldState(), GoalStub, null)
                .SetName("Actions");
        }

        [TestCaseSource(nameof(FailedPlanCases))]
        public void FailedPlan(WorldState worldState, IGoapGoal goal, IGoapAction[] actions)
        {
            //Arrange:
            var planner = new AStarPlanner();

            //Act:
            bool success = planner.Plan(worldState, goal, actions, out List<IGoapAction> plan);

            //Assert:
            Assert.IsFalse(success);
            Assert.AreEqual(0, plan.Count);
        }

        private static IEnumerable<TestCaseData> FailedPlanCases()
        {
            yield return new TestCaseData(
                    new WorldState(
                        EnemyAlive(true)
                    ),
                    new GoapGoal(
                        "DestroyEnemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyAlive(false)
                    ),
                    Array.Empty<IGoapAction>()
                )
                .SetName("Action array is empty");

            yield return new TestCaseData(
                    new WorldState(
                        Injured(true),
                        EnemyAlive(true)
                    ),
                    new GoapGoal(
                        "DestroyEnemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyAlive(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "SelfTreatment",
                            effects: new LocalState(Injured(false)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 10,
                            onUpdate: null
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyAlive(true)),
                            isValid: () => true,
                            cost: () => 10,
                            onUpdate: null
                        )
                    }
                )
                .SetName("Not compatitive actions")
                .SetCategory("Primitive");
        }

        [TestCaseSource(nameof(SuccessfulPlanCases))]
        public void SucessfulPlan(
            WorldState worldState,
            IGoapGoal goal,
            IGoapAction[] actions,
            string[] expectedPlan
        )
        {
            //Arrange:
            AStarPlanner planner = new AStarPlanner();

            //Act:
            bool success = planner.Plan(worldState, goal, actions,
                plan: out List<IGoapAction> actualPlan
            );

            //Assert:
            Assert.IsTrue(success);

            int count = expectedPlan.Length;
            Assert.AreEqual(count, actualPlan.Count);

            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedPlan[i], actualPlan[i].Name);
            }
        }

        private static IEnumerable<TestCaseData> SuccessfulPlanCases()
        {
            //Test:
            yield return new TestCaseData(
                    new WorldState(EnemyAlive(false)),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyAlive(false)
                    ),
                    Array.Empty<IGoapAction>(),
                    Array.Empty<string>()
                )
                .SetName("Goal state equals world state")
                .SetDescription("Even if the action list is empty, then first check for world and goal states");

            //Test:
            yield return new TestCaseData(
                    new WorldState(Injured(true)),
                    HealingGoal,
                    new[]
                    {
                        new GoapAction(
                            "SelfTreatment",
                            effects: new LocalState(Injured(false)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 10,
                            onUpdate: null
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyAlive(true)),
                            isValid: () => true,
                            cost: () => 10,
                            onUpdate: null
                        )
                    },
                    new[] {"SelfTreatment"}
                )
                .SetName("Healing plan")
                .SetCategory("Primitive");

            //Test:
            yield return new TestCaseData(
                    new WorldState(
                        EnemyAlive(true),
                        NearEnemy(false),
                        AtEnemy(false),
                        Injured(true),
                        HasAmmo(true)
                    ),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyAlive(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyAlive(true)),
                            isValid: () => true,
                            cost: () => 7,
                            onUpdate: null
                        ),
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyAlive(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 2, //heuristic: 1
                            onUpdate: null
                        ),

                        new GoapAction(
                            "MoveNearEnemy",
                            effects: new LocalState(NearEnemy(true)),
                            conditions: new LocalState(EnemyAlive(true)),
                            isValid: () => true,
                            cost: () => 5,
                            onUpdate: null
                        ),
                        new GoapAction(
                            "RangeCombat",
                            effects: new LocalState(EnemyAlive(false)),
                            conditions: new LocalState(NearEnemy(true)),
                            isValid: () => true,
                            cost: () => 4, //heuristic: 2
                            onUpdate: null
                        )
                    },
                    new[] {"MoveAtEnemy", "MeleeCombat"}
                )
                .SetName("Melee plan")
                .SetCategory("Easy")
                .SetDescription("Planner should select melee branch because melee cost 10, but range — 11");
        }

        //TODO: WITH AMMO RANGE BRANCH

        // [Test]
        // public void MeleeCombatPlanTest()
        // {
        //
        //     var goal = new FactState(
        //         new Fact("enemyExists", false)
        //     );
        //
        //     var actions = new[]
        //     {
        //         SwordCombatAction,
        //         MoveAtEnemyAction,
        //         MakeHealAction
        //     };
        //
        //     //Act:
        //     var success = this.planner.TryMakePlan(worldState, goal, actions, out var actualPlan);
        //
        //     //Assert:
        //     Assert.True(success);
        //
        //     var expectedPlan = new List<IActor>
        //     {
        //         MoveAtEnemyAction,
        //         SwordCombatAction
        //     };
        //     Assert.True(EqualsPlans(expectedPlan, actualPlan));
        // }
        //
        // [Test]
        // public void RangeCombatPlanTest()
        // {
        //     //Arrange:
        //     var worldState = new FactState(
        //         new Fact("enemyExists", true),
        //         new Fact("nearEnemy", false),
        //         new Fact("atEnemy", false),
        //         new Fact("isInjured", true),
        //         new Fact("arrowsExists", true)
        //     );
        //
        //     var goal = new FactState(
        //         new Fact("enemyExists", false)
        //     );
        //
        //     var actions = new[]
        //     {
        //         SwordCombatAction,
        //         BowCombatAction,
        //         MoveNearEnemyAction,
        //         MoveAtEnemyAction,
        //         MoveToResourceAction,
        //         MakeHealAction
        //     };
        //
        //     //Act:
        //     var success = this.planner.TryMakePlan(worldState, goal, actions, out var actualPlan);
        //
        //     //Assert:
        //     Assert.True(success);
        //
        //     var expectedPlan = new List<IActor>
        //     {
        //         MoveNearEnemyAction,
        //         BowCombatAction
        //     };
        //
        //     Assert.True(EqualsPlans(expectedPlan, actualPlan));
        // }
        //
        // [Test]
        // public void DrinkBeerTest()
        // {
        //     //Arrange:
        //     var worldState = new FactState(
        //         new Fact("enemyExists", true),
        //         new Fact("nearEnemy", false),
        //         new Fact("atEnemy", false),
        //         new Fact("isInjured", true),
        //         new Fact("hasAmmo", true),
        //         new Fact("moneyEnough", false),
        //         new Fact("resourceEnough", false),
        //         new Fact("atResource", false)
        //     );
        //
        //     var goal = new FactState(
        //         new Fact("happy", true)
        //     );
        //
        //     var actions = new[]
        //     {
        //         SwordCombatAction,
        //         MoveAtEnemyAction,
        //         MakeHealAction,
        //         MoveToResourceAction,
        //         HarvestResourceAction,
        //         GoToMarketAction,
        //         GoBeerAction
        //     };
        //
        //     //Act:
        //     var success = this.planner.TryMakePlan(worldState, goal, actions, out var actualPlan);
        //
        //     //Assert:
        //     Assert.True(success);
        //
        //     var expectedPlan = new List<IActor>
        //     {
        //         MoveToResourceAction,
        //         HarvestResourceAction,
        //         GoToMarketAction,
        //         GoBeerAction
        //     };
        //     Assert.True(EqualsPlans(expectedPlan, actualPlan));
        // }
    }
}