using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Pool;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable UseDeconstruction

[assembly: InternalsVisibleTo("Goap.Tests")]

namespace AI.Goap
{
    public sealed class AStarPlanner : IGoapPlanner
    {
        internal readonly int heuristicPoints;
        internal readonly int heuristicUndefined;

        public AStarPlanner(int heuristicPoints = 1, int heuristicUndefined = int.MaxValue / 2)
        {
            this.heuristicPoints = heuristicPoints;
            this.heuristicUndefined = heuristicUndefined;
        }

        public bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IReadOnlyList<IGoapAction> actions,
            out List<IGoapAction> plan
        )
        {
            plan = new List<IGoapAction>();
            return this.Plan(worldState, goal, actions, plan);
        }

        public bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IReadOnlyList<IGoapAction> actions,
            List<IGoapAction> plan
        )
        {
            if (worldState == null)
                throw new ArgumentNullException(nameof(worldState));

            if (goal == null)
                throw new ArgumentNullException(nameof(goal));

            if (actions == null)
                throw new ArgumentNullException(nameof(actions));

            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            return this.PlanInternal(worldState, goal, actions, plan);
        }

        internal bool PlanInternal(
            in WorldState worldState,
            in IGoapGoal goal,
            in IReadOnlyList<IGoapAction> actions,
            List<IGoapAction> plan
        )
        {
            plan.Clear();

            LocalState goalState = goal.Result;

            if (!worldState.OverlapsKeys(goalState))
                return false;

            if (worldState.Overlaps(goalState))
                return true;

            if (actions.Count == 0)
                return false;

            var openList = DictionaryPool<IGoapAction, Node>.Get();
            var closedList = HashSetPool<IGoapAction>.Get();
            var complete = false;

            this.VisitGoal(goalState, worldState, actions, openList);

            while (this.TryPeekAction(openList, out Node node))
            {
                IGoapAction action = node.action;
                openList.Remove(action);

                if (worldState.Overlaps(action.Conditions))
                {
                    this.CreatePlan(node, plan);
                    complete = true;
                    break;
                }

                this.VisitAction(node, worldState, actions, openList, closedList);
                closedList.Add(action);
            }

            DictionaryPool<IGoapAction, Node>.Release(openList);
            HashSetPool<IGoapAction>.Release(closedList);

            return complete;
        }

        internal void VisitGoal(
            in LocalState goalState,
            in WorldState worldState,
            in IReadOnlyList<IGoapAction> actions,
            in Dictionary<IGoapAction, Node> openList
        )
        {
            int neighbours = 0;
            for (int i = 0, count = actions.Count; i < count; i++)
            {
                IGoapAction action = actions[i];

                if (!this.IsNeighbour(action, goalState, worldState))
                    continue;

                Node node = new Node
                {
                    action = action,
                    previous = null,
                    cost = action.Cost,
                    heuristic = this.GetHeuristic(worldState, action),
                };

                openList.Add(action, node);
                neighbours++;
            }

            if (neighbours == 0 && this.CreateNeighbour(goalState, worldState, actions, out IGoapAction neighbour))
            {
                Node node = new Node
                {
                    action = neighbour,
                    previous = null,
                    cost = neighbour.Cost,
                    heuristic = this.GetHeuristic(worldState, neighbour)
                };

                openList.Add(neighbour, node);
            }
        }

        internal void VisitAction(
            in Node visitingNode,
            in WorldState worldState,
            in IReadOnlyList<IGoapAction> actions,
            in Dictionary<IGoapAction, Node> openList,
            in HashSet<IGoapAction> closedList
        )
        {
            LocalState conditions = visitingNode.action.Conditions;
            
            int pathCost = visitingNode.cost;
            int neighbours = 0;

            for (int i = 0, count = actions.Count; i < count; i++)
            {
                IGoapAction action = actions[i];

                if (closedList.Contains(action))
                    continue;

                if (!this.IsNeighbour(action, conditions, worldState))
                    continue;

                if (openList.TryGetValue(action, out Node actionNode))
                {
                    int cost = pathCost + actionNode.cost;
                    if (actionNode.cost > cost)
                    {
                        actionNode.previous = visitingNode;
                        actionNode.cost = cost;
                    }
                }
                else
                {
                    actionNode = new Node
                    {
                        action = action,
                        previous = visitingNode,
                        cost = pathCost + action.Cost,
                        heuristic = this.GetHeuristic(worldState, action)
                    };

                    openList.Add(action, actionNode);
                }

                neighbours++;
            }

            if (neighbours == 0 && this.CreateNeighbour(conditions, worldState, actions, out IGoapAction neighbour))
            {

                Node actionNode = new Node
                {
                    action = neighbour,
                    previous = visitingNode,
                    cost = pathCost + neighbour.Cost,
                    heuristic = this.GetHeuristic(worldState, neighbour)
                };

                openList.Add(neighbour, actionNode);
            }
        }

        internal bool TryPeekAction(Dictionary<IGoapAction, Node> openList, out Node result)
        {
            result = null;
            int minWeight = int.MaxValue;

            foreach (Node node in openList.Values)
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
                IGoapAction action = end.action;

                if (action is ActionGroup sequence)
                {
                    plan.AddRange(sequence.Actions);
                }
                else
                {
                    plan.Add(action);
                }

                end = end.previous;
            }
        }

        internal int GetHeuristic(in WorldState worldState, in IGoapAction action)
        {
            int result = 0;
            LocalState conditions = action.Conditions;
            
            for (int i = 0, count = conditions.Count; i < count; i++)
            {
                KeyValuePair<string, bool> condition = conditions[i];
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

        internal bool IsNeighbour(in IGoapAction action, in LocalState conditions, in WorldState worldState)
        {
            LocalState effects = action.Effects;
            for (int i = 0, count = conditions.Count; i < count; i++)
            {
                KeyValuePair<string, bool> condition = conditions[i];
                if (!effects.Overlaps(condition) && !worldState.Overlaps(condition))
                    return false;
            }

            return true;
        }

        internal bool CreateNeighbour(
            in LocalState conditions,
            in WorldState worldState,
            in IReadOnlyList<IGoapAction> actions,
            out IGoapAction neighbour
        )
        {
            neighbour = default;
            List<IGoapAction> sequence = ListPool<IGoapAction>.Get();

            for (int i = 0, count = conditions.Count; i < count; i++)
            {
                KeyValuePair<string, bool> condition = conditions[i];
                if (worldState.Overlaps(condition))
                {
                    continue;
                }

                if (!this.FindCheapestAction(condition, actions, out IGoapAction action))
                {
                    ListPool<IGoapAction>.Release(sequence);
                    return false;
                }

                sequence.Add(action);
            }

            neighbour = new ActionGroup(null, sequence);
            ListPool<IGoapAction>.Release(sequence);
            return true;
        }

        internal bool FindCheapestAction(
            in KeyValuePair<string, bool> condition,
            in IReadOnlyList<IGoapAction> actions,
            out IGoapAction result)
        {
            int minCost = int.MaxValue;
            result = default;

            for (int i = 0, count = actions.Count; i < count; i++)
            {
                IGoapAction action = actions[i];
                if (!action.Effects.Overlaps(condition))
                    continue;

                int cost = action.Cost;
                if (cost < minCost)
                {
                    minCost = cost;
                    result = action;
                }
            }

            return result != null;
        }

        internal sealed class Node
        {
            public IGoapAction action;
            public Node previous;
            public int heuristic;
            public int cost;
            
            public int Weight
            {
                get { return this.cost + this.heuristic; }
            }
        }
    }
}