// using System;
// using Atomic.AI;
// using UnityEngine;
//
// namespace Game.Engine
// {
//     [Serializable]
//     public sealed class AssignWaypointBTNode : BTNode
//     {
//         protected override BTResult OnUpdate(IBlackboard blackboard, float deltaTime)
//         {
//             if (!blackboard.TryGetWaypoints(out Transform[] waypoints))
//             {
//                 return BTResult.FAILURE;
//             }
//             
//             int index = blackboard.GetWaypointIndex();
//             GameObject targetWaypoint = waypoints[index].gameObject;
//             
//             blackboard.SetTarget(targetWaypoint);
//             return BTResult.SUCCESS;
//         }
//     }
// }