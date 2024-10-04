using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using static AI.Goap.Substitutes;

// ReSharper disable ArgumentsStyleOther
// ReSharper disable ArgumentsStyleAnonymousFunction

namespace AI.Goap
{
    public sealed class GoapAgentTests
    {
        [Test]
        public void InstantiateAgent()
        {
            //Arrange:
            var goals = new List<IGoapGoal> {GoalStub};
            var actions = new List<IGoapAction> {ActionStub};
            var sensors = new List<IGoapSensor> {SensorStub};
            var worldState = new WorldState(Injured(false), HasAmmo(true));

            //Act:
            var agent = new GoapAgent(new AStarPlanner(), goals, actions, sensors);
            agent.SetWorldState(worldState);

            //Assert:
            Assert.IsNotNull(agent);
            Assert.AreEqual(goals, agent.GetGoals());
            Assert.AreEqual(actions, agent.GetActions());
            Assert.AreEqual(sensors, agent.GetSensors());
            Assert.AreEqual(worldState, agent.GetWorldState());
        }

        [Test]
        public void WhenInstantiateAndPlannerArgumentIsNullThenThrowsException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                var _ = new GoapAgent(planner: null);
            }, "planner");
        }

        [Test]
        public void WhenDesideWithNullPlanThenThrowsException()
        {
            //Arrange:
            var agent = new GoapAgent(new PlannerMock());

            //Assert:
            Assert.Catch<ArgumentNullException>(() => agent.Decide(plan: null, out _), "plan");
        }

        [TestCaseSource(nameof(FindPriorityGoalSuccessfulCases))]
        public void FindPriorityGoalSuccessful(IGoapGoal[] goals, string expectedGoal)
        {
            //Arrange:
            var agent = new GoapAgent(new PlannerMock(), goals);

            //Act:
            bool success = agent.GetPriorityGoal(out IGoapGoal actualGoal);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(expectedGoal, actualGoal.Name);
        }

        private static IEnumerable<TestCaseData> FindPriorityGoalSuccessfulCases()
        {
            yield return new TestCaseData(new IGoapGoal[]
                    {
                        new GoapGoal("A", () => true, () => 5, null),
                        new GoapGoal("B", () => true, () => 15, null),
                        new GoapGoal("C", () => true, () => 3, null),
                    },
                    "B")
                .SetName("Max Priority")
                .SetCategory("Primitive");

            yield return new TestCaseData(new IGoapGoal[]
                    {
                        new GoapGoal("A", () => true, () => 5, null),
                        new GoapGoal("B", () => false, () => 15, null),
                        new GoapGoal("C", () => true, () => 3, null),
                        new GoapGoal("D", () => true, () => 10, null)
                    },
                    "D")
                .SetName("Max Priority but not valid")
                .SetCategory("Primitive");

            yield return new TestCaseData(new IGoapGoal[]
                    {
                        new GoapGoal("A", () => true, () => 5, null),
                        new GoapGoal("B", () => true, () => 10, null),
                        new GoapGoal("C", () => true, () => 3, null),
                        new GoapGoal("D", () => true, () => 10, null)
                    },
                    "B")
                .SetName("Equal goal priority")
                .SetCategory("Primitive");
        }

        [TestCaseSource(nameof(GetPriorityGoalFailedCases))]
        public void GetPriorityGoalFailed(IGoapGoal[] goals)
        {
            //Arrange:
            var agent = new GoapAgent(new PlannerMock(), goals);

            //Act:
            bool success = agent.GetPriorityGoal(out IGoapGoal actualGoal);

            //Assert:
            Assert.IsFalse(success);
            Assert.IsNull(actualGoal);
        }

        private static IEnumerable<TestCaseData> GetPriorityGoalFailedCases()
        {
            yield return new TestCaseData(arg: Array.Empty<IGoapGoal>())
                .SetName("Goal collection is empty")
                .SetCategory("Primitive");

            yield return new TestCaseData(arg: new IGoapGoal[]
                {
                    new GoapGoal("A", () => false, () => 5, null),
                    new GoapGoal("B", () => false, () => 10, null),
                    new GoapGoal("C", () => false, () => 3, null),
                    new GoapGoal("D", () => false, () => 10, null)
                })
                .SetName("No valid goals")
                .SetCategory("Primitive");
        }

        [TestCaseSource(nameof(GetValidActionsCases))]
        public void GetValidActions(IGoapAction[] actions, string[] expectedActions)
        {
            //Arrange:
            var agent = new GoapAgent(new PlannerMock(), actions: actions);

            //Act:
            var actualActions = new List<IGoapAction>();
            agent.GetValidActions(actualActions);

            //Assert:
            Assert.AreEqual(expectedActions.Length, actualActions.Count);
            Assert.AreEqual(expectedActions, actualActions.Select(it => it.Name).ToArray());
        }

        private static IEnumerable<TestCaseData> GetValidActionsCases()
        {
            yield return new TestCaseData(new IGoapAction[]
                    {
                        new GoapAction("A", null, null, () => false, () => 5),
                        new GoapAction("B", null, null, () => true, () => 10),
                        new GoapAction("C", null, null, () => false, () => 3),
                        new GoapAction("D", null, null, () => true, () => 10)
                    },
                    new[] {"B", "D"})
                .SetName("Default")
                .SetCategory("Primitive");

            yield return new TestCaseData(new IGoapAction[]
                    {
                        new GoapAction("A", null, null, () => false, () => 5),
                        new GoapAction("B", null, null, () => false, () => 10),
                        new GoapAction("C", null, null, () => false, () => 3),
                        new GoapAction("D", null, null, () => false, () => 10)
                    },
                    Array.Empty<string>())
                .SetName("No valid actions")
                .SetCategory("Primitive");

            yield return new TestCaseData(
                    Array.Empty<IGoapAction>(),
                    Array.Empty<string>())
                .SetName("No actions")
                .SetCategory("Primitive");
        }

        [TestCaseSource(nameof(SyncWorldStateCases))]
        public void SyncWorldState(IGoapSensor[] sensors, WorldState expectedState)
        {
            //Arrange:
            var agent = new GoapAgent(new PlannerMock(), null, null, sensors);
            agent.SetWorldState(new WorldState(
                new KeyValuePair<string, bool>("A", true),
                new KeyValuePair<string, bool>("B", false),
                new KeyValuePair<string, bool>("C", true),
                new KeyValuePair<string, bool>("D", false)
            ));

            //Act:
            agent.SyncWorldState();

            //Assert:
            Assert.AreEqual(expectedState, agent.GetWorldState());
        }

        private static IEnumerable<TestCaseData> SyncWorldStateCases()
        {
            yield return new TestCaseData(
                new IGoapSensor[]
                {
                    new GoapSensor("Injured", ws => ws["Injured"] = false),
                    new GoapSensor("EnemyAlive", ws => ws["EnemyAlive"] = true)
                },
                new WorldState(
                    new KeyValuePair<string, bool>("Injured", false),
                    new KeyValuePair<string, bool>("EnemyAlive", true)
                ));
        }

        [TestCaseSource(nameof(DecideFailedCases))]
        public void DecideFailed(IGoapGoal[] goals, IGoapAction[] actions, string expectedGoal)
        {
            //Arrange:
            var agent = new GoapAgent(new PlannerMock(), goals, actions);

            //Act:
            var plan = new List<IGoapAction>();
            bool success = agent.Decide(plan, out IGoapGoal goal);

            //Assert:
            Assert.IsFalse(success);
            Assert.AreEqual(expectedGoal, goal?.Name);
            Assert.AreEqual(0, plan.Count);
        }

        private static IEnumerable<TestCaseData> DecideFailedCases()
        {
            yield return new TestCaseData(
                    Array.Empty<IGoapGoal>(),
                    new[] {ActionStub},
                    null
                )
                .SetName("Goal collection is empty")
                .SetCategory("Primitive");

            yield return new TestCaseData(
                    new IGoapGoal[]
                    {
                        new GoapGoal("A", () => false, () => 5, null),
                        new GoapGoal("B", () => false, () => 10, null),
                        new GoapGoal("C", () => false, () => 3, null),
                        new GoapGoal("D", () => false, () => 10, null)
                    },
                    new[] {ActionStub},
                    null
                )
                .SetName("No valid goals")
                .SetCategory("Primitive");

            yield return new TestCaseData(
                    new[] {GoalStub},
                    Array.Empty<IGoapAction>(),
                    GoalStub.Name
                )
                .SetName("Action collection is empty")
                .SetCategory("Primitive");

            yield return new TestCaseData(
                    new[] {GoalStub},
                    new IGoapAction[]
                    {
                        new GoapAction("A", null, null, () => false, () => 5),
                        new GoapAction("B", null, null, () => false, () => 10),
                        new GoapAction("C", null, null, () => false, () => 3),
                        new GoapAction("D", null, null, () => false, () => 10)
                    },
                    GoalStub.Name
                )
                .SetName("No valid actions")
                .SetCategory("Primitive");
        }

        [TestCaseSource(nameof(PlanSuccessfulCases))]
        public void DecideSuccessful(
            IGoapPlanner planner,
            IGoapGoal[] goals,
            IGoapAction[] actions,
            IGoapSensor[] sensors,
            string expectedGoal,
            string[] expectedPlan
        )
        {
            //Arrange:
            var agent = new GoapAgent(planner, goals, actions, sensors);

            //Act:
            var actualPlan = new List<IGoapAction>();
            bool success = agent.Decide(actualPlan, out IGoapGoal actualGoal);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(expectedGoal, actualGoal.Name);

            foreach (IGoapAction action in actualPlan)
            {
                Debug.Log($"ACTION {action.Name}");
            }
            
            Assert.AreEqual(expectedPlan.Length, actualPlan.Count);
            Assert.AreEqual(expectedPlan, actualPlan.Select(it => it.Name).ToArray());
        }

        private static IEnumerable<TestCaseData> PlanSuccessfulCases()
        {
            yield return MockPlannerCase();
            yield return RangeCombatHardAStarCase();
        }


        private static TestCaseData MockPlannerCase()
        {
            var a1 = new GoapAction("A", null, null, () => false, () => 5);
            var a2 = new GoapAction("B", null, null, () => true, () => 10);
            var a3 = new GoapAction("C", null, null, () => true, () => 3);
            var a4 = new GoapAction("D", null, null, () => true, () => 10);

            var g1 = new GoapGoal("G1", () => false, () => 5, null);
            var g2 = new GoapGoal("G2", () => true, () => 10, null);
            var g3 = new GoapGoal("G3", () => false, () => 3, null);
            var g4 = new GoapGoal("G4", () => false, () => 15, null);

            return new TestCaseData(
                    new PlannerMock((_, _, actions, plan) =>
                    {
                        plan.Add(actions[0]);
                        plan.Add(actions[2]);
                        return true;
                    }),
                    new[] {g1, g2, g3, g4},
                    new[] {a1, a2, a3, a4},
                    Array.Empty<IGoapSensor>(),
                    "G2",
                    new[] {"B", "D"}
                )
                .SetName("Mock Planner")
                .SetCategory("Primitive");
        }

        private static TestCaseData RangeCombatHardAStarCase()
        {
            var planner = new AStarPlanner();
            var goals = new IGoapGoal[]
            {
                new GoapGoal(
                    "DestroyEnemy",
                    () => true,
                    () => 5,
                    EnemyExists(false)
                ),
                new GoapGoal(
                    "SelfTreatment",
                    () => true,
                    () => 3,
                    Injured(false)
                ),
                new GoapGoal(
                    "CollectResources",
                    () => true,
                    () => 1,
                    ResourcesCollected(true)
                )
            };
            var actions = new[]
            {
                new GoapAction(
                    "SelfTreatment",
                    effects: new LocalState(Injured(false)),
                    conditions: new LocalState(),
                    isValid: () => true,
                    cost: () => 10
                ),


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

                //Gathering
                new GoapAction(
                    "GatherResource",
                    effects: new LocalState(ResourceExists(false)),
                    conditions: new LocalState(AtResource(true)),
                    isValid: () => true,
                    cost: () => 2 //heuristic: 0
                ),
                new GoapAction(
                    "MoveAtResource",
                    effects: new LocalState(AtResource(true)),
                    conditions: new LocalState(ResourceExists(true)),
                    isValid: () => true,
                    cost: () => 3 //heuristic: 0
                )
            };
            var sensors = new IGoapSensor[]
            {
                new GoapSensor("Injured", ws => ws["Injured"] = true),
                new GoapSensor("Enemy", ws =>
                {
                    ws[nameof(EnemyExists)] = true;
                    ws[nameof(NearEnemy)] = false;
                    ws[nameof(AtEnemy)] = false;
                }),
                new GoapSensor("Ammo", ws =>
                {
                    ws[nameof(HasAmmo)] = false;
                }),
                new GoapSensor("Resource", ws =>
                {
                    ws["ResourceExists"] = true;
                    ws["NearResource"] = false;
                })
            };

            return new TestCaseData(
                    planner,
                    goals,
                    actions,
                    sensors,
                    "DestroyEnemy",
                    new[] {"PickUpAmmo", "MoveNearEnemy", "RangeCombat"}
                )
                .SetName("Range Combat AStar Planner")
                .SetCategory("Hard");
        }
    }
}