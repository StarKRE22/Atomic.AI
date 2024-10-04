using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace AI.Goap
{
    public sealed class GoapAgent
    {
        private readonly List<IGoapGoal> goals;
        private readonly List<IGoapAction> actions;
        private readonly List<IGoapSensor> sensors;
        private WorldState worldState;
        
        private IGoapPlanner planner;

        public GoapAgent(
            in IGoapPlanner planner,
            in IEnumerable<IGoapGoal> goals = null,
            in IEnumerable<IGoapAction> actions = null,
            in IEnumerable<IGoapSensor> sensors = null
        )
        {
            this.planner = planner ?? throw new ArgumentNullException(nameof(planner));
            this.goals = goals != null ? new List<IGoapGoal>(goals) : new List<IGoapGoal>();
            this.actions = actions != null ? new List<IGoapAction>(actions) : new List<IGoapAction>();
            this.sensors = sensors != null ? new List<IGoapSensor>(sensors) : new List<IGoapSensor>();
            this.worldState = new WorldState();
            this.SyncWorldState();
        }

        public bool Decide(out List<IGoapAction> plan, out IGoapGoal goal, bool syncState = true)
        {
            plan = new List<IGoapAction>();
            return this.Decide(plan, out goal, syncState);
        }

        public bool Decide(List<IGoapAction> plan, out IGoapGoal goal, bool syncState = true)
        {
            if (!this.GetPriorityGoal(out goal))
                return false;

            bool success = false;

            var actions = ListPool<IGoapAction>.Get();
            this.GetValidActions(actions);

            if (actions.Count == 0)
                goto EndPoint;

            if (syncState) 
                this.SyncWorldState();

            success = this.planner.Plan(this.worldState, goal, actions, plan);
            
            EndPoint:
            ListPool<IGoapAction>.Release(actions);
            return success;
        }
        
        #region Planner

        public IGoapPlanner GetPlanner()
        {
            return this.planner;
        }

        public void SetPlanner(IGoapPlanner planner)
        {
            this.planner = planner;
        } 

        #endregion

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

        public void ClearGoals()
        {
            this.goals.Clear();
        }

        internal bool GetPriorityGoal(out IGoapGoal result)
        {
            result = default;
            int maxPriority = int.MinValue;

            for (int i = 0, count = this.goals.Count; i < count; i++)
            {
                IGoapGoal goal = this.goals[i];
                if (!goal.IsValid)
                    continue;

                int priority = goal.Priority;
                if (priority > maxPriority)
                {
                    result = goal;
                    maxPriority = priority;
                }
            }

            return result != null;
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

        public void ClearActions()
        {
            this.actions.Clear();
        }

        internal void GetValidActions(List<IGoapAction> results)
        {
            results.Clear();

            for (int i = 0, count = this.actions.Count; i < count; i++)
            {
                IGoapAction action = this.actions[i];
                if (action.IsValid)
                {
                    results.Add(action);
                }
            }
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

        public void ClearSensors()
        {
            this.sensors.Clear();
        }

        #endregion

        #region WorldState

        public IGoapState GetWorldState()
        {
            return this.worldState;
        }

        internal void SetWorldState(WorldState worldState)
        {
            this.worldState = worldState;
        }
        
        internal void SyncWorldState()
        {
            this.worldState.Clear();

            for (int i = 0, count = this.sensors.Count; i < count; i++)
            {
                IGoapSensor sensor = this.sensors[i];
                sensor.PopulateState(this.worldState);
            }
        }

        #endregion
    }
}