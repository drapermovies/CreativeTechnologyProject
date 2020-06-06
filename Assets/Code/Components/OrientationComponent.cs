using System;
using Unity.Entities;
using Unity.Mathematics;

namespace TrafficSimulation
{
    [Serializable]
    public struct Orientation : IComponentData
    {
        public float3 worldPosition;
        public quaternion rotation;
    }
    public class OrientationComponent : ComponentDataProxy<Orientation> { }
}