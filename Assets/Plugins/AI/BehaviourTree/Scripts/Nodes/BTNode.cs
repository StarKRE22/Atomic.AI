using System;
using Sirenix.OdinInspector;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BTNode
    {
        public virtual string Name => this.GetType().Name;

        public bool IsActive => this.isActive;
        
        [ShowInInspector, ReadOnly]
        private bool isActive;

        internal BTResult Run(IBlackboard blackboard, float deltaTime)
        {
            if (!this.isActive)
            {
                this.isActive = true;
                this.OnEnable(blackboard);
            }

            BTResult result = this.OnUpdate(blackboard, deltaTime);

            if (result != BTResult.RUNNING)
            {
                this.isActive = false;
                this.OnDisable(blackboard);
            }

            return result;
        }

        internal void Abort(IBlackboard blackboard)
        {
            if (this.isActive)
            {
                this.isActive = false;
                this.OnAbort(blackboard);
                this.OnDisable(blackboard);
            }
        }

        protected abstract BTResult OnUpdate(IBlackboard blackboard, float deltaTime);
        
        protected virtual void OnEnable(IBlackboard blackboard)
        {
        }

        protected virtual void OnDisable(IBlackboard blackboard)
        {
        }

        protected virtual void OnAbort(IBlackboard blackboard)
        {
        }
    }
}