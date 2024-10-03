using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace AI.Goap
{
    public sealed class SequenceAction : IGoapAction
    {
        private readonly string name;
        private readonly LocalState effects;
        private readonly LocalState conditions;
        private readonly List<IGoapAction> actions;
        private bool isRunning;

        public string Name => this.name;
        public LocalState Effects => this.effects;
        public LocalState Conditions => this.conditions;

        public bool IsRunning
        {
            get { return this.isRunning; }
        }

        public IReadOnlyList<IGoapAction> Actions
        {
            get { return this.actions; }
        }

        public bool IsValid
        {
            get
            {
                for (int i = 0, count = this.actions.Count; i < count; i++)
                {
                    IGoapAction action = this.actions[i];
                    if (!action.IsValid)
                        return false;
                }

                return true;
            }
        }

        public int Cost
        {
            get
            {
                int cost = 0;
                for (int i = 0, count = this.actions.Count; i < count; i++)
                {
                    IGoapAction action = this.actions[i];
                    cost += action.Cost;
                }

                return cost;
            }
        }

        public SequenceAction(in string name, in IReadOnlyList<IGoapAction> actions)
        {
            this.name = name;
            this.actions = new List<IGoapAction>();

            List<KeyValuePair<string, bool>> effects = ListPool<KeyValuePair<string, bool>>.Get();
            List<KeyValuePair<string, bool>> conditions = ListPool<KeyValuePair<string, bool>>.Get();

            foreach (IGoapAction action in actions)
            {
                this.actions.Add(action);
                effects.AddRange(action.Effects);
                conditions.AddRange(action.Conditions);
            }

            this.effects = new LocalState(effects);
            this.conditions = new LocalState(conditions);

            ListPool<KeyValuePair<string, bool>>.Release(effects);
            ListPool<KeyValuePair<string, bool>>.Release(conditions);
        }

        public IGoapAction.Result Run(in float deltaTime)
        {
            throw new NotImplementedException();
        }

        public bool Cancel()
        {
            throw new NotImplementedException();
        }
    }
}