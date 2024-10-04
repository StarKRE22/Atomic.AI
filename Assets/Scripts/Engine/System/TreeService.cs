using System.Collections.Generic;
using UnityEngine;

namespace Game.Engine
{
    ///Содержит информацию о всех ресурсах на карте
    public sealed class TreeService : MonoBehaviour
    {
        public IReadOnlyList<GameObject> Trees => this.trees;

        private GameObject[] trees;
        
        private void Awake()
        {
            this.trees = GameObject.FindGameObjectsWithTag(GameObjectTags.Tree);
        }
        
        public bool FindClosest(Vector3 position, out GameObject closestResource)
        {
            float minDistance = float.MaxValue;
            closestResource = default;

            for (int i = 0, count = this.trees.Length; i < count; i++)
            {
                GameObject resource = this.trees[i];
                if (!resource.activeSelf)
                {
                    continue;
                }

                Vector3 resourcePosition = resource.transform.position;
                Vector3 distanceVector = resourcePosition - position;
                distanceVector.y = 0;

                float resourceDistance = distanceVector.sqrMagnitude;
                if (resourceDistance < minDistance)
                {
                    minDistance = resourceDistance;
                    closestResource = resource;
                }
            }

            return closestResource != default;
        }

        
    }
}