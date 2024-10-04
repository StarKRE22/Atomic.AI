using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static AI.Goap.Substitutes;

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
            var agent = new GoapAgent(new AStarPlanner(), goals, actions, sensors, worldState);

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


        // public void MakePlan()
        // {
        //     
        // }
    }
}