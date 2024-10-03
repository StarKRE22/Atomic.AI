using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace AI.Goap
{
    public sealed class GoapActionSequence : IGoapAction
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

        public GoapActionSequence(in string name, in IReadOnlyList<IGoapAction> actions)
        {
            HashSet<KeyValuePair<string, bool>> effects = HashSetPool<KeyValuePair<string, bool>>.Get();
            HashSet<KeyValuePair<string, bool>> conditions = HashSetPool<KeyValuePair<string, bool>>.Get();

            this.name = name;
            this.actions = new List<IGoapAction>();
            
            foreach (IGoapAction action in actions)
            {
                this.actions.Add(action);
                effects.UnionWith(action.Effects);
                conditions.UnionWith(action.Conditions);
            }

            this.effects = new LocalState(effects);
            this.conditions = new LocalState(conditions);

            HashSetPool<KeyValuePair<string, bool>>.Release(effects);
            HashSetPool<KeyValuePair<string, bool>>.Release(conditions);
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