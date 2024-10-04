using System;
using UnityEngine;

namespace AI.Goap
{
    [Serializable]
    public sealed class AStarPlannerAsset : IGoapPlannerAsset
    {
        [SerializeField]
        private int heuristicPoints = 1;

        [SerializeField]
        private int heuristicUndefined = int.MaxValue;
        
        public IGoapPlanner Create()
        {
            return new AStarPlanner(this.heuristicPoints, this.heuristicUndefined);
        }
    }
}