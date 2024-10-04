using Atomic.AI;
using Game;

namespace Engine
{
    public sealed class HasResourceBlackboardCondition : IBlackboardCondition
    {
        public bool Invoke(IBlackboard blackboard)
        {
            return blackboard.HasResource();
        }
    }
}