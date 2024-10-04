// using System;
// using Atomic.AI;
// using UnityEngine;
//
// namespace Game.Engine
// {
//     [Serializable]
//     public sealed class NextWaypointBTNode : BTNode
//     {
//         protected override BTResult OnUpdate(IBlackboard blackboard, float deltaTime)
//         {
//             Transform[] waypoints = blackboard.GetWaypoints();
//             int index = blackboard.GetWaypointIndex();
//             index = (index + 1) % waypoints.Length;
//             
//             blackboard.SetWaypointIndex(index);
//             return BTResult.SUCCESS;
//         }
//     }
// }