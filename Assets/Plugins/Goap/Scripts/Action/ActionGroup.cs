using System.Collections.Generic;
using UnityEngine.Pool;
// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace AI.Goap
{
    internal sealed class ActionGroup : IGoapAction
    {
        public string Name => this.name;
        public LocalState Effects => this.effects;
        public LocalState Conditions => this.conditions;
        
        public IEnumerable<IGoapAction> Actions => this.actions;

        public bool IsValid
        {
            get
            {
                for (int i = 0, count = this.actions.Count; i < count; i++)
                {
                    if (!this.actions[i].IsValid)
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
                    cost += this.actions[i].Cost;

                return cost;
            }
        }
        
        private readonly string name;
        private readonly LocalState effects;
        private readonly LocalState conditions;
        private readonly List<IGoapAction> actions;

        public ActionGroup(in string name, in IReadOnlyList<IGoapAction> actions)
        {
            HashSet<KeyValuePair<string, bool>> effects = HashSetPool<KeyValuePair<string, bool>>.Get();
            HashSet<KeyValuePair<string, bool>> conditions = HashSetPool<KeyValuePair<string, bool>>.Get();
            
            for (int i = 0, count = actions.Count; i < count; i++)
            {
                IGoapAction action = actions[i];
                effects.UnionWith(action.Effects);
                conditions.UnionWith(action.Conditions);
            }

            this.name = name;
            this.actions = new List<IGoapAction>(actions);
            this.effects = new LocalState(effects);
            this.conditions = new LocalState(conditions);

            HashSetPool<KeyValuePair<string, bool>>.Release(effects);
            HashSetPool<KeyValuePair<string, bool>>.Release(conditions);
        }
    }
}