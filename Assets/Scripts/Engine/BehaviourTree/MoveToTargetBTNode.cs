// using System;
// using Atomic.AI;
// using UnityEngine;
//
// namespace Game.Engine
// {
//     [Serializable]
//     public sealed class MoveToTargetBTNode : BTNode
//     {
//         [SerializeField, BlackboardKey]
//         private int targetKey;
//         
//         protected override BTResult OnUpdate(IBlackboard blackboard, float deltaTime)
//         {
//             if (!blackboard.TryGetCharacter(out GameObject character) ||
//                 !blackboard.TryGetObject(this.targetKey, out GameObject target) ||
//                 !blackboard.TryGetStoppingDistance(out float stoppingDistance))
//             {
//                 return BTResult.FAILURE;
//             }
//
//             Vector3 distanceVector = target.transform.position - character.transform.position;
//             distanceVector.y = 0;
//
//             if (distanceVector.sqrMagnitude <= stoppingDistance * stoppingDistance)
//             {
//                 return BTResult.SUCCESS;
//             }
//
//             Vector3 moveDirection = distanceVector.normalized;
//             character.GetComponent<MoveComponent>().MoveStep(moveDirection);
//             return BTResult.RUNNING;
//         }
//     }
// }