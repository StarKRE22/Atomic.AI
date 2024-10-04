using UnityEngine;

namespace Game.Engine
{
    public static class DistanceUtils
    {
        public static float DistanceBetween(in GameObject source, in GameObject target)
        {
            Vector3 vector = target.transform.position - source.transform.position;
            vector.y = 0;
            return vector.magnitude;
        }
    }
}