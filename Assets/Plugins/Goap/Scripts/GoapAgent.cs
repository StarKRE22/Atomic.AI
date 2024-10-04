using System;
using System.Collections.Generic;

namespace AI.Goap
{
    public sealed class GoapAgent
    {
        private readonly List<IGoapGoal> goals;
        private readonly List<IGoapAction> actions;
        private readonly List<IGoapSensor> sensors;
        private readonly WorldState worldState;

        private IGoapPlanner _planner;

        public GoapAgent(
            in IGoapPlanner planner,
            in IEnumerable<IGoapGoal> goals = null,
            in IEnumerable<IGoapAction> actions = null,
            in IEnumerable<IGoapSensor> sensors = null,
            in WorldState worldState = null
        )
        {
            _planner = planner ?? throw new ArgumentNullException(nameof(planner));

            this.goals = goals != null ? new List<IGoapGoal>(goals) : new List<IGoapGoal>();
            this.actions = actions != null ? new List<IGoapAction>(actions) : new List<IGoapAction>();
            this.sensors = sensors != null ? new List<IGoapSensor>(sensors) : new List<IGoapSensor>();
            this.worldState = worldState ?? new WorldState();
        }

        #region Goals

        public IReadOnlyList<IGoapGoal> GetGoals()
        {
            return this.goals;
        }

        public bool AddGoal(in IGoapGoal goal)
        {
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));

            if (this.goals.Contains(goal))
                return false;

            this.goals.Add(goal);
            return true;
        }

        public bool RemoveGoal(in IGoapGoal goal)
        {
            return goal != null && this.goals.Remove(goal);
        }

        public bool ContainsGoal(in IGoapGoal goal)
        {
            return goal != null && this.goals.Contains(goal);
        }

        #endregion

        #region Actions

        public IReadOnlyList<IGoapAction> GetActions()
        {
            return this.actions;
        }

        public bool AddAction(in IGoapAction action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (this.actions.Contains(action))
                return false;

            this.actions.Add(action);
            return true;
        }

        public bool RemoveAction(in IGoapAction action)
        {
            return action != null && this.actions.Remove(action);
        }

        public bool ContainsAction(in IGoapAction action)
        {
            return action != null && this.actions.Contains(action);
        }

        #endregion

        #region Sensors

        public IReadOnlyList<IGoapSensor> GetSensors()
        {
            return this.sensors;
        }

        public bool AddSensor(in IGoapSensor sensor)
        {
            if (sensor == null)
                throw new ArgumentNullException(nameof(sensor));

            if (this.sensors.Contains(sensor))
                return false;
            
            this.sensors.Add(sensor);
            return true;
        }

        public bool RemoveSensor(in IGoapSensor sensor)
        {
            return sensor != null && this.sensors.Remove(sensor);
        }

        public bool ContainsSensor(in IGoapSensor sensor)
        {
            return sensor != null && this.sensors.Contains(sensor);
        }
        
        #endregion

        #region WorldState

        public IGoapState GetWorldState()
        {
            return this.worldState;
        }

        #endregion
    }
}