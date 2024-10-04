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

        //TODO: CHECK PLAN NULL!
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
            yield return ActionArrayEmptyCase();
            yield return NoCompatitiveActionsCase();
            yield return MeleeCombatNoPlanCase();
            yield return NoEnemyAliveStateCase();
            yield return GoalStateNotReachableCase();
        }

        private static TestCaseData NoEnemyAliveStateCase()
        {
            return new TestCaseData(
                    new WorldState(
                        NearEnemy(false),
                        AtEnemy(false),
                        HasAmmo(true)
                    ),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 1 //heuristic: 1
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 5
                        ),

                        new GoapAction(
                            "RangeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(NearEnemy(true), HasAmmo(true)),
                            isValid: () => true,
                            cost: () => 1 //heuristic: 1
                        ),
                        new GoapAction(
                            "MoveNearEnemy",
                            effects: new LocalState(NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 2
                        ),
                    }
                )
                .SetName("No Enemy Alive State")
                .SetCategory("Easy")
                .SetDescription("Planner should't make a plan because world state hasn't property");
        }

        private static TestCaseData GoalStateNotReachableCase()
        {
            return new TestCaseData(
                    new WorldState(
                        Injured(true)
                    ),
                    new GoapGoal(
                        "DestroyEnemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "SelfTreatment",
                            effects: new LocalState(Injured(false)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 10
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 10
                        )
                    })
                .SetName("Goal State Not Reachable");
        }

        private static TestCaseData ActionArrayEmptyCase()
        {
            return new TestCaseData(
                    new WorldState(
                        EnemyExists(true)
                    ),
                    new GoapGoal(
                        "DestroyEnemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    Array.Empty<IGoapAction>()
                )
                .SetName("Action array is empty");
        }

        private static TestCaseData NoCompatitiveActionsCase()
        {
            return new TestCaseData(
                    new WorldState(
                        Injured(true),
                        EnemyExists(true)
                    ),
                    new GoapGoal(
                        "DestroyEnemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "SelfTreatment",
                            effects: new LocalState(Injured(false)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 10
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 10
                        )
                    }
                )
                .SetName("Not compatitive actions")
                .SetCategory("Primitive");
        }

        private static TestCaseData MeleeCombatNoPlanCase()
        {
            return new TestCaseData(
                    new WorldState(
                        EnemyExists(true),
                        NearEnemy(false),
                        AtEnemy(false),
                        HasAmmo(true)
                    ),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 1 //heuristic: 1
                        ),
                        new GoapAction(
                            "MoveNearEnemy",
                            effects: new LocalState(NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 2
                        ),
                    }
                )
                .SetName("Melee Combat")
                .SetCategory("Easy")
                .SetDescription("Planner should not found plan because there aren't MoveAtEnemy action");
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
                Assert.AreEqual(expectedPlan[i], actualPlan[i].Name);
        }

        private static IEnumerable<TestCaseData> SuccessfulPlanCases()
        {
            yield return GoalStateEqualsWorldStateCase();
            yield return HealingGoalPrimitveCase();
            yield return MeleeCombatEasyCase();
            yield return RangeCombatEasyCase();
            yield return CombatBranchesHasEqualsCostsCase();
            yield return CompositeGoalPrimitive();
            yield return RangeCombatWithAmmoCase();
            yield return OppositeGoalPrimitiveCase();
        }

        private static TestCaseData OppositeGoalPrimitiveCase()
        {
            return new TestCaseData(
                    new WorldState(
                        Injured(true)
                    ),
                    new GoapGoal(
                        "Opposite",
                        isValid: () => true,
                        priority: () => 1,
                        result: new[] {Injured(false), Injured(true)}
                    ),
                    new[]
                    {
                        new GoapAction(
                            "SelfTreatment",
                            effects: new LocalState(Injured(false)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 5
                        ),
                        new GoapAction(
                            "SelfAbusing",
                            effects: new LocalState(Injured(true)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 5
                        ),
                    },
                    new[] {"SelfTreatment"}
                )
                .SetName("Opposite Goal")
                .SetCategory("Primitive")
                .SetDescription("Planner should only treat himself because injured=false is already reached!");
        }

        private static TestCaseData RangeCombatWithAmmoCase()
        {
            return new TestCaseData(
                    
                    new WorldState(
                        EnemyExists(true),
                        AtEnemy(false),
                        NearEnemy(false),
                        HasAmmo(false)
                    ),
                    
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    
                    
                    new[]
                    {
                        //Melee branch: weight: 16
                        
                        
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 10 //heuristic: 1
                        ),
                        
                        
                        
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 5 //heuristic: 0
                        ),
                        
                        

                        //Range branch: 9
                        new GoapAction(
                            "RangeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(HasAmmo(true), NearEnemy(true)),
                            isValid: () => true,
                            cost: () => 1 //heuristic: 2
                        ),
                        
                        
                        new GoapAction(
                            "MoveNearEnemy",
                            effects: new LocalState(NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 2 //heuristic: 0
                        ),
                        
                        
                        new GoapAction(
                            "PickUpAmmo",
                            effects: new LocalState(HasAmmo(true)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 4 //heuristic: 0
                        ),
                    },
                    new[] {"PickUpAmmo", "MoveNearEnemy", "RangeCombat"}
                )
                .SetName("Range Combat With Ammo")
                .SetCategory("Normal")
                .SetDescription("Planner should resolve composite action for range branch because range cost 9, " +
                                "but melee — 16");
        }


        private static TestCaseData RangeCombatEasyCase()
        {
            return new TestCaseData(
                    new WorldState(
                        EnemyExists(true),
                        NearEnemy(false),
                        AtEnemy(false),
                        HasAmmo(true)
                    ),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 1 //heuristic: 1
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 5
                        ),

                        new GoapAction(
                            "RangeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(NearEnemy(true), HasAmmo(true)),
                            isValid: () => true,
                            cost: () => 1 //heuristic: 1
                        ),
                        new GoapAction(
                            "MoveNearEnemy",
                            effects: new LocalState(NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 2
                        ),
                    },
                    new[] {"MoveNearEnemy", "RangeCombat"}
                )
                .SetName("Range Combat")
                .SetCategory("Easy")
                .SetDescription("Planner should select range branch because range cost 4, but melee — 7");
        }

        private static TestCaseData MeleeCombatEasyCase()
        {
            return new TestCaseData(
                    new WorldState(
                        EnemyExists(true),
                        NearEnemy(false),
                        AtEnemy(false),
                        HasAmmo(true)
                    ),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 7
                        ),
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 2 //heuristic: 1
                        ),

                        new GoapAction(
                            "MoveNearEnemy",
                            effects: new LocalState(NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 5
                        ),
                        new GoapAction(
                            "RangeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(NearEnemy(true), HasAmmo(true)),
                            isValid: () => true,
                            cost: () => 4 //heuristic: 2
                        )
                    },
                    new[] {"MoveAtEnemy", "MeleeCombat"}
                )
                .SetName("Melee Combat")
                .SetCategory("Easy")
                .SetDescription("Planner should select melee branch because melee cost 10, but range — 11");
        }

        private static TestCaseData GoalStateEqualsWorldStateCase()
        {
            return new TestCaseData(
                    new WorldState(EnemyExists(false)),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    Array.Empty<IGoapAction>(),
                    Array.Empty<string>()
                )
                .SetName("Goal State equals World State")
                .SetDescription("Even if the action list is empty, then first check for world and goal states");
        }

        private static TestCaseData HealingGoalPrimitveCase()
        {
            return new TestCaseData(
                    new WorldState(Injured(true)),
                    new GoapGoal(
                        "Healing",
                        isValid: () => true,
                        priority: () => 5,
                        result: Injured(false)
                    ),
                    new[]
                    {
                        new GoapAction(
                            "SelfTreatment",
                            effects: new LocalState(Injured(false)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 10
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 10
                        )
                    },
                    new[] {"SelfTreatment"}
                )
                .SetName("SelfTreatment")
                .SetCategory("Primitive");
        }


        private static TestCaseData CombatBranchesHasEqualsCostsCase()
        {
            return new TestCaseData(
                    new WorldState(
                        EnemyExists(true),
                        NearEnemy(false),
                        AtEnemy(false),
                        HasAmmo(true)
                    ),
                    new GoapGoal(
                        "Destroy Enemy",
                        isValid: () => true,
                        priority: () => 1,
                        result: EnemyExists(false)
                    ),
                    new[]
                    {
                        //Melee branch: weight 8
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 2 //heuristic: 1
                        ),
                        new GoapAction(
                            "MoveAtEnemy",
                            effects: new LocalState(AtEnemy(true), NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 5
                        ),

                        //Range branch: weight 8
                        new GoapAction(
                            "RangeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(NearEnemy(true), HasAmmo(true)),
                            isValid: () => true,
                            cost: () => 4 //heuristic: 1
                        ),
                        new GoapAction(
                            "MoveNearEnemy",
                            effects: new LocalState(NearEnemy(true)),
                            conditions: new LocalState(EnemyExists(true)),
                            isValid: () => true,
                            cost: () => 3
                        )
                    },
                    new[] {"MoveAtEnemy", "MeleeCombat"}
                )
                .SetName("Combat branches have equal cost")
                .SetCategory("Easy")
                .SetDescription("When there is several branch with equals costs, planner should select first branch");
        }


        private static TestCaseData CompositeGoalPrimitive()
        {
            return new TestCaseData(
                    new WorldState(
                        EnemyExists(true),
                        Injured(true),
                        AtEnemy(true)
                    ),
                    new GoapGoal(
                        "Destroy Enemy And Heaing",
                        isValid: () => true,
                        priority: () => 1,
                        result: new[] {EnemyExists(false), Injured(false)}
                    ),
                    new[]
                    {
                        //Melee branch: weight 8
                        new GoapAction(
                            "MeleeCombat",
                            effects: new LocalState(EnemyExists(false)),
                            conditions: new LocalState(AtEnemy(true)),
                            isValid: () => true,
                            cost: () => 2 //heuristic: 1
                        ),
                        new GoapAction(
                            "SelfTreatment",
                            effects: new LocalState(Injured(false)),
                            conditions: new LocalState(),
                            isValid: () => true,
                            cost: () => 10
                        )
                    },
                    new[] {"MeleeCombat", "SelfTreatment"}
                )
                .SetName("Composite Goal")
                .SetCategory("Primitive")
                .SetDescription("When all actions not satisfied goal state then create composite action");
        }

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