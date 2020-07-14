using System;
using Unity.Entities;

namespace TrafficSimulation
{
    [Serializable]
    public struct VehicleData : IComponentData
    {
        public int currentRoadID;
    }
}