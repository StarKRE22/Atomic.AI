// using System;
// using System.Collections.Generic;
// using NUnit.Framework;
// using static AI.Goap.Substitutes;
//
// // ReSharper disable ArgumentsStyleOther
// // ReSharper disable ArgumentsStyleNamedExpression
//
// namespace AI.Goap
// {
//     public sealed class AStarPlannerTests
//     {
//         [Test]
//         public void CreatePlanner()
//         {
//             //Act:
//             IGoapPlanner planner = new AStarPlanner();
//
//             //Assert:
//             Assert.IsNotNull(planner);
//         }
//
//         [Test]
//         public void WhenWorldStateArgIsNullThenThrowException()
//         {
//             //Arrange:
//             IGoapPlanner planner = new AStarPlanner();
//
//             //Assert:
//             Assert.Catch<ArgumentNullException>(() =>
//             {
//                 planner.Plan(
//                     worldState: null,
//                     DestroyEnemyGoal,
//                     new[] {MoveAtEnemyAction},
//                     out _
//                 );
//             }, "worldState");
//         }
//
//         [Test]
//         public void WhenGoalArgIsNullThenThrowException()
//         {
//             //Arrange:
//             IGoapPlanner planner = new AStarPlanner();
//
//             //Assert:
//             Assert.Catch<ArgumentNullException>(() =>
//             {
//                 planner.Plan(
//                     new WorldState(),
//                     null,
//                     new[] {MoveAtEnemyAction},
//                     out _
//                 );
//             }, "goal");
//         }
//
//         [Test]
//         public void WhenActionsArgIsNullThenThrowException()
//         {
//             //Arrange:
//             IGoapPlanner planner = new AStarPlanner();
//
//             //Assert:
//             Assert.Catch<ArgumentNullException>(() =>
//             {
//                 planner.Plan(
//                     new WorldState(),
//                     DestroyEnemyGoal,
//                     null,
//                     out _
//                 );
//             }, "actions");
//         }
//
//         [Test]
//         public void WhenActionCollectionIsEmptyThenReturnFalse()
//         {
//             //Arrange:
//             var planner = new AStarPlanner();
//
//             //Act:
//             bool success = planner.Plan(
//                 worldState: new WorldState(),
//                 goal: DestroyEnemyGoal,
//                 actions: Array.Empty<IGoapAction>(),
//                 plan: out _
//             );
//             
//             //Assert:
//             Assert.IsFalse(success);
//         }
//
//         [Test]
//         public void WhenGoalStateEqualsWorldStateThenReturnTrue()
//         {
//             //Arrange:
//             AStarPlanner planner = new AStarPlanner();
//
//             bool success = planner.Plan(
//                 worldState: new WorldState(EnemyAlive(false)),
//                 goal: DestroyEnemyGoal,
//                 actions: new[]{MoveAtEnemyAction},
//                 plan: out List<IGoapAction> plan
//             );
//
//             Assert.IsTrue(success);
//             Assert.IsNotNull(plan);
//             Assert.AreEqual(0, plan.Count);
//         }
//
//         // [Test]
//         // public void When
//
//
//         //  [Test]
//         // public void MakeHealPlanTest()
//         // {
//         //     //Arrange:
//         //     var worldState = new FactState(
//         //         new Fact("isInjured", true)
//         //     );
//         //
//         //     var goal = new FactState(
//         //         new Fact("isInjured", false)
//         //     );
//         //
//         //     var actions = new[]
//         //     {
//         //         MakeHealAction
//         //     };
//         //
//         //     //Act:
//         //     var success = this.planner.TryMakePlan(worldState, goal, actions, out var actualPlan);
//         //
//         //     //Assert:
//         //     Assert.True(success);
//         //
//         //     var expectedPlan = new List<IActor>
//         //     {
//         //         MakeHealAction
//         //     };
//         //     Assert.True(EqualsPlans(expectedPlan, actualPlan));
//         // }
//         //
//         // [Test]
//         // public void MeleeCombatPlanTest()
//         // {
//         //     //Arrange:
//         //     var worldState = new FactState(
//         //         new Fact("enemyExists", true),
//         //         new Fact("nearEnemy", false),
//         //         new Fact("atEnemy", false),
//         //         new Fact("isInjured", true),
//         //         new Fact("hasAmmo", true)
//         //     );
//         //
//         //     var goal = new FactState(
//         //         new Fact("enemyExists", false)
//         //     );
//         //
//         //     var actions = new[]
//         //     {
//         //         SwordCombatAction,
//         //         MoveAtEnemyAction,
//         //         MakeHealAction
//         //     };
//         //
//         //     //Act:
//         //     var success = this.planner.TryMakePlan(worldState, goal, actions, out var actualPlan);
//         //
//         //     //Assert:
//         //     Assert.True(success);
//         //
//         //     var expectedPlan = new List<IActor>
//         //     {
//         //         MoveAtEnemyAction,
//         //         SwordCombatAction
//         //     };
//         //     Assert.True(EqualsPlans(expectedPlan, actualPlan));
//         // }
//         //
//         // [Test]
//         // public void RangeCombatPlanTest()
//         // {
//         //     //Arrange:
//         //     var worldState = new FactState(
//         //         new Fact("enemyExists", true),
//         //         new Fact("nearEnemy", false),
//         //         new Fact("atEnemy", false),
//         //         new Fact("isInjured", true),
//         //         new Fact("arrowsExists", true)
//         //     );
//         //
//         //     var goal = new FactState(
//         //         new Fact("enemyExists", false)
//         //     );
//         //
//         //     var actions = new[]
//         //     {
//         //         SwordCombatAction,
//         //         BowCombatAction,
//         //         MoveNearEnemyAction,
//         //         MoveAtEnemyAction,
//         //         MoveToResourceAction,
//         //         MakeHealAction
//         //     };
//         //
//         //     //Act:
//         //     var success = this.planner.TryMakePlan(worldState, goal, actions, out var actualPlan);
//         //
//         //     //Assert:
//         //     Assert.True(success);
//         //
//         //     var expectedPlan = new List<IActor>
//         //     {
//         //         MoveNearEnemyAction,
//         //         BowCombatAction
//         //     };
//         //
//         //     Assert.True(EqualsPlans(expectedPlan, actualPlan));
//         // }
//         //
//         // [Test]
//         // public void DrinkBeerTest()
//         // {
//         //     //Arrange:
//         //     var worldState = new FactState(
//         //         new Fact("enemyExists", true),
//         //         new Fact("nearEnemy", false),
//         //         new Fact("atEnemy", false),
//         //         new Fact("isInjured", true),
//         //         new Fact("hasAmmo", true),
//         //         new Fact("moneyEnough", false),
//         //         new Fact("resourceEnough", false),
//         //         new Fact("atResource", false)
//         //     );
//         //
//         //     var goal = new FactState(
//         //         new Fact("happy", true)
//         //     );
//         //
//         //     var actions = new[]
//         //     {
//         //         SwordCombatAction,
//         //         MoveAtEnemyAction,
//         //         MakeHealAction,
//         //         MoveToResourceAction,
//         //         HarvestResourceAction,
//         //         GoToMarketAction,
//         //         GoBeerAction
//         //     };
//         //
//         //     //Act:
//         //     var success = this.planner.TryMakePlan(worldState, goal, actions, out var actualPlan);
//         //
//         //     //Assert:
//         //     Assert.True(success);
//         //
//         //     var expectedPlan = new List<IActor>
//         //     {
//         //         MoveToResourceAction,
//         //         HarvestResourceAction,
//         //         GoToMarketAction,
//         //         GoBeerAction
//         //     };
//         //     Assert.True(EqualsPlans(expectedPlan, actualPlan));
//         // }
//     }
// }