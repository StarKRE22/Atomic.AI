using System;
using AI.Goap;
using Atomic.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class GoapSensorAsset : IGoapSensorAsset<IBlackboard>
    {
        [GUIColor(1f, 0.92156863f, 0.015686275f)]
        [SerializeField]
        private string name;

        [SerializeReference]
        private IWorldStateSensor onPopulate = null;

        public IGoapSensor Create(IBlackboard source)
        {
            return new GoapSensor(
                this.name,
                ws => this.onPopulate.Invoke(source, ws)
            );
        }
    }
}