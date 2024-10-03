// using System.Collections.Generic;
//
// namespace AI.Goap
// {
//     public static class GoapUtils
//     {
//         //TODO: TESTS!
//         public static void BuildNeighbourGraph(
//             in IGoapAction[] actions,
//             in WorldState worldState,
//             Dictionary<IGoapAction, List<IGoapAction>> graph
//         )
//         {
//             graph.Clear();
//             
//             int count = actions.Length;
//             if (count == 0)
//                 return;
//
//             for (int i = 0; i < count; i++)
//             {
//                 IGoapAction action = actions[i];
//                 LocalState conditions = action.Conditions;
//                 
//                 for (var j = i + 1; j < count; j++)
//                 {
//                     IGoapAction other = actions[j];
//                     LocalState effects = other.Effects;
//                 
//                     if (!conditions.OverlapsBy(worldState, effects)) //TODO Advanced variant!
//                         continue;
//                 
//                     if (!graph.TryGetValue(action, out var neighbours))
//                     {
//                         neighbours = new List<IActor>();
//                         graph.Add(action, neighbours);
//                     }
//                 
//                     neighbours.Add(other);
//                 }
//                 
//                 
//                 List<IGoapAction> neighbours = FindNeighbours(, actions, worldState, i);
//                 graph.Add(action, neighbours);
//             }
//         }
//
//         private static List<IGoapAction> FindNeighbours(LocalState condition, in IGoapAction[] actions, in WorldState worldState, int i)
//         {
//             
//         }
//
//         //COMPOSITE
//         
//
//         //TODO: TESTS!
//         
//     }
// }