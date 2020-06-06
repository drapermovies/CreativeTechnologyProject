using UnityEngine;
using Unity.Entities;

namespace TrafficSimulation
{
    public struct Vehicle : IComponentData
    {
        public Entity prefab;
    }
}
