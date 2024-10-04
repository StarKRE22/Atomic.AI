// using AI.Goap;
// using Atomic.AI;
// using UnityEngine;
//
// namespace Game.Engine
// {
//     public sealed class EnemySensor : IWorldStateSensor
//     {
//         public void Invoke(IBlackboard blackboard, WorldState worldState)
//         {
//             if (blackboard.TryGetCharacter(out GameObject character) &&
//                 blackboard.TryGetEnemy(out GameObject enemy))
//             {
//                 float distance = DistanceUtils.DistanceBetween(character, enemy);
//
//                 worldState["Enemy"] = enemy;
//                 worldState["AtEnemy"] = distance < blackboard.GetMeleeDistance();
//                 worldState["NearEnemy"] = distance < blackboard.GetRangeDistance();
//             }
//         }
//     }
// }