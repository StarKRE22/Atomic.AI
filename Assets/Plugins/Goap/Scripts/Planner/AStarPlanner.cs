using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
// ReSharper disable FieldCanBeMadeReadOnly.Global

// ReSharper disable UseDeconstruction

[assembly: InternalsVisibleTo("Goap.Tests")]

namespace AI.Goap
{
    public sealed class AStarPlanner : IGoapPlanner
    {
        internal readonly int heuristicPoints;
        internal readonly int heuristicUndefined;

        internal readonly Dictionary<IGoapAction, Node> openList = new();
        internal readonly HashSet<IGoapAction> closedList = new();

        public AStarPlanner(int heuristicPoints = 1, int heuristicUndefined = int.MaxValue)
        {
            this.heuristicPoints = heuristicPoints;
            this.heuristicUndefined = heuristicUndefined;
        }

        public bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IGoapAction[] actions,
            List<IGoapAction> plan
        )
        {
            if (worldState == null)
                throw new ArgumentNullException(nameof(worldState));

            if (goal == null)
                throw new ArgumentNullException(nameof(goal));

            if (actions == null)
                throw new ArgumentNullException(nameof(actions));
            
            if (actions.Length == 0)
                return false;
            
            return this.PlanInternal(worldState, goal, actions, plan);
        }

        internal bool PlanInternal(
            in WorldState worldState,
            in IGoapGoal goal,
            in IGoapAction[] actions,
            List<IGoapAction> plan
        )
        {
            plan.Clear();

            LocalState goalState = goal.Result;
            if (worldState.Overlaps(goalState))
                return true;
            
            this.openList.Clear();
            this.closedList.Clear();

            this.VisitGoal(goalState, worldState, actions);

            while (this.TryPeekAction(out Node node))
            {
                IGoapAction action = node.Action;
                this.openList.Remove(action);

                if (worldState.Overlaps(action.Conditions))
                {
                    this.CreatePlan(node, plan);
                    return true;
                }

                this.VisitAction(node, worldState, actions);
                this.closedList.Add(action);
            }

            return false;
        }

        internal void VisitGoal(in LocalState goalState, in WorldState worldState, in IGoapAction[] actions)
        {
            for (int i = 0, count = actions.Length; i < count; i++)
            {
                IGoapAction action = actions[i];

                if (this.OverlapsCondition(goalState, action.Effects, worldState))
                {
                    int heuristic = this.GetHeuristic(worldState, action.Conditions);
                    Node node = new Node(action, null, action.Cost, heuristic);
                    this.openList.Add(action, node);
                }
            }
        }
        
        internal void VisitAction(in Node visitingNode, in WorldState worldState, in IGoapAction[] actions)
        {
            LocalState conditionState = visitingNode.Action.Conditions;

            for (int i = 0, count = actions.Length; i < count; i++)
            {
                IGoapAction action = actions[i];
               
                if (this.closedList.Contains(action))
                    continue;

                if (!this.OverlapsCondition(conditionState, action.Effects, worldState))
                    continue;

                int cost = visitingNode.Cost + action.Cost;
                
                if (this.openList.TryGetValue(action, out Node actionNode))
                {
                    if (actionNode.Cost > cost)
                    {
                        actionNode.Previous = visitingNode;
                        actionNode.Cost = cost;
                    }
                }
                else
                {
                    var heuristic =  this.GetHeuristic(worldState, action.Conditions);
                    actionNode = new Node(action, visitingNode, cost, heuristic);
                    this.openList.Add(action, actionNode);
                }
            }
        }

        internal bool TryPeekAction(out Node result)
        {
            result = null;
            int minWeight = int.MaxValue;

            foreach (Node node in this.openList.Values)
            {
                int weight = node.Weight;
                if (weight < minWeight)
                {
                    result = node;
                    minWeight = weight;
                }
            }

            return result != null;
        }

        internal void CreatePlan(Node end, List<IGoapAction> plan)
        {
            while (end != null)
            {
                plan.Add(end.Action);
                end = end.Previous;
            }
        }

        internal int GetHeuristic(in WorldState worldState, in LocalState localState)
        {
            int result = 0;

            for (int i = 0, count = localState.Count; i < count; i++)
            {
                KeyValuePair<string, bool> condition = localState[i];
                if (worldState.TryGetValue(condition.Key, out bool value))
                {
                    if (value != condition.Value)
                        result += this.heuristicPoints;
                }
                else
                {
                    return this.heuristicUndefined;
                }
            }

            return result;
        }

        internal bool OverlapsCondition(in LocalState conditions, in LocalState effects, in WorldState worldState)
        {
            for (int i = 0, count = conditions.Count; i < count; i++)
            {
                KeyValuePair<string, bool> condition = conditions[i];
                if (!effects.Overlaps(condition) && !worldState.Overlaps(condition))
                    return false;
            }

            return true;
        }

        internal sealed class Node
        {
            public IGoapAction Action;
            public Node Previous;
            public int Heuristic;
            public int Cost;

            public int Weight
            {
                get { return this.Cost + this.Heuristic; }
            }

            public Node(in IGoapAction action, in Node previous, in int cost, in int heuristic)
            {
                this.Action = action;
                this.Previous = previous;
                this.Cost = cost;
                this.Heuristic = heuristic;
            }
        }
    }
}