using System;

using Unity.Entities;
using Unity.Mathematics;

namespace TrafficSimulation
{
    [Serializable]
    public struct VehicleData : IComponentData
    {
        public int currentRoadID;

        //Way we're travelling down our current road (-1 = Going, 1 = Coming)
        public int currentDirection;
    }
}