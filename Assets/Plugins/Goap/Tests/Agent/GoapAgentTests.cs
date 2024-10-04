using System;
using System.Collections.Generic;
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
    }
}