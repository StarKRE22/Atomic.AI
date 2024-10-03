using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable UseDeconstruction

[assembly: InternalsVisibleTo("Goap.Tests")]

namespace AI.Goap
{
    //TODO: Сделать вложенный A*
    public sealed class AStarPlanner : IGoapPlanner
    {
        internal readonly int heuristicPoints;
        internal readonly int heuristicUndefined;

        public AStarPlanner(int heuristicPoints = 1, int heuristicUndefined = int.MaxValue)
        {
            this.heuristicPoints = heuristicPoints;
            this.heuristicUndefined = heuristicUndefined;
        }

        public bool Plan(
            in WorldState worldState,
            in IGoapGoal goal,
            in IGoapAction[] actions,
            out List<IGoapAction> plan
        )
        {
            plan = new List<IGoapAction>();
            return this.Plan(worldState, goal, actions, plan);
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

            if (actions.Length == 0)
                return false;

            bool complete = false;

            Dictionary<IGoapAction, Node> openList = DictionaryPool<IGoapAction, Node>.Get();
            HashSet<IGoapAction> closedList = HashSetPool<IGoapAction>.Get();

            this.VisitGoal(goalState, worldState, actions, openList);

            while (this.TryPeekAction(openList, out Node node))
            {
                IGoapAction action = node.action;
                openList.Remove(action);

                Debug.Log($"LOOK ACTION {action.Name}");

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
            in IGoapAction[] actions,
            Dictionary<IGoapAction, Node> openList
        )
        {
            int satisfied = 0;

            for (int i = 0, count = actions.Length; i < count; i++)
            {
                IGoapAction action = actions[i];

                if (this.IsActionSatisfied(goalState, action.Effects, worldState))
                {
                    int cost = action.Cost;
                    int heuristic = this.GetHeuristic(worldState, action.Conditions);
                    int weight = cost + heuristic;

                    Node node = new Node
                    {
                        action = action,
                        previous = null,
                        cost = cost,
                        heuristic = heuristic,
                        weight = weight
                    };

                    openList.Add(action, node);
                    satisfied++;
                }
            }

            if (satisfied == 0 &&
                this.ResolveSatisfiedAction(goalState, worldState, actions, out SequenceAction sequence))
            {
                int cost = sequence.Cost;
                int heuristic = this.GetHeuristic(worldState, sequence.Conditions);
                int weight = cost + heuristic;

                Node node = new Node
                {
                    action = sequence,
                    previous = null,
                    cost = cost,
                    heuristic = heuristic,
                    weight = weight
                };

                openList.Add(sequence, node);
            }
        }


        internal void VisitAction(
            in Node visitingNode,
            in WorldState worldState,
            in IGoapAction[] actions,
            Dictionary<IGoapAction, Node> openList,
            in HashSet<IGoapAction> closedList
        )
        {
            LocalState visitingConditions = visitingNode.action.Conditions;
            int visitingWeight = visitingNode.weight;

            int satisfied = 0;

            for (int i = 0, count = actions.Length; i < count; i++)
            {
                IGoapAction action = actions[i];

                if (closedList.Contains(action))
                    continue;

                if (!this.IsActionSatisfied(visitingConditions, action.Effects, worldState))
                    continue;

                if (openList.TryGetValue(action, out Node actionNode))
                {
                    int actionWeight = visitingWeight + actionNode.cost + actionNode.heuristic;
                    if (actionNode.weight > actionWeight)
                    {
                        actionNode.previous = visitingNode;
                        actionNode.weight = actionWeight;
                    }
                }
                else
                {
                    int actionCost = action.Cost;
                    int actionHeuristic = this.GetHeuristic(worldState, action.Conditions);
                    int actionWeight = visitingWeight + actionCost + actionHeuristic;

                    actionNode = new Node
                    {
                        action = action,
                        previous = visitingNode,
                        cost = actionCost,
                        heuristic = actionHeuristic,
                        weight = actionWeight
                    };

                    openList.Add(action, actionNode);
                }

                satisfied++;
            }
            
            if (satisfied == 0 &&
                this.ResolveSatisfiedAction(visitingConditions, worldState, actions, out SequenceAction sequence))
            {
                int cost = sequence.Cost;
                int heuristic = this.GetHeuristic(worldState, sequence.Conditions);
                int weight = cost + heuristic;

                Node node = new Node
                {
                    action = sequence,
                    previous = visitingNode,
                    cost = cost,
                    heuristic = heuristic,
                    weight = weight
                };

                openList.Add(sequence, node);
            }
        }

        internal bool TryPeekAction(Dictionary<IGoapAction, Node> openList, out Node result)
        {
            result = null;
            int minWeight = int.MaxValue;

            foreach (Node node in openList.Values)
            {
                int weight = node.weight;
                Debug.Log($"OPEN NODE {node.action.Name}: {node.weight}");
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

                if (action is SequenceAction sequence)
                    plan.AddRange(sequence.Actions);
                else
                    plan.Add(action);

                end = end.previous;
            }

            Debug.Log($"PLAN {string.Join(',', plan.Select(it => it.Name))}");
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

        internal bool IsActionSatisfied(in LocalState conditions, in LocalState effects, in WorldState worldState)
        {
            for (int i = 0, count = conditions.Count; i < count; i++)
            {
                KeyValuePair<string, bool> condition = conditions[i];
                if (!effects.Overlaps(condition) && !worldState.Overlaps(condition))
                    return false;
            }

            return true;
        }

        internal bool ResolveSatisfiedAction(
            in LocalState conditions,
            in WorldState worldState,
            in IGoapAction[] actions,
            out SequenceAction result
        )
        {   
            result = default;
            List<IGoapAction> sequence = new List<IGoapAction>();

            for (int i = 0, conditionCount = conditions.Count; i < conditionCount; i++)
            {
                KeyValuePair<string, bool> condition = conditions[i];
                if (worldState.Overlaps(condition))
                    continue;

                if (!this.IsAnyActionSatisfied(condition, actions, out IGoapAction action))
                    return false;

                sequence.Add(action);
            }

            result = new SequenceAction(string.Join(',', sequence.Select(it => it.Name)), sequence);
            return true;
        }

        private bool IsAnyActionSatisfied(
            in KeyValuePair<string,bool> condition,
            in IGoapAction[] actions,
            out IGoapAction result)
        {

            int minCost = int.MaxValue;
            result = default;

            for (int i = 0, count = actions.Length; i < count; i++)
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
            public int heuristic;
            public int cost;
            public Node previous;
            public int weight;
        }
    }
}