using System;
using Sirenix.OdinInspector;

namespace Atomic.AI
{
    [Serializable]
    public sealed class BTNodeSelector : BTNodeComposite
    {
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private int nodeIndex;
        
        protected override void OnEnable(IBlackboard blackboard)
        {
            this.nodeIndex = 0;
        }
        
        protected override void OnAbort(IBlackboard blackboard)
        {
            BTNode currentNode = this.nodes[this.nodeIndex];
            currentNode.Abort(blackboard);
        }
        
        protected override BTResult OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            BTNode currentNode = this.nodes[this.nodeIndex];
            BTResult result = currentNode.Run(blackboard, deltaTime);
            
            if (result != BTResult.FAILURE)
            {
                return result;
            }

            //Failure:
            if (this.nodeIndex == this.nodes.Length - 1)
            {
                return BTResult.FAILURE;
            }

            this.nodeIndex++;
            return BTResult.RUNNING;
        }
    }
}