using UnityEngine;
using Unity.Entities;

public class RoadValues : MonoBehaviour
{
    public int id { get; private set; }
    public int speedLimit = 30;
    public int lanes = 1;
    public bool isOneWay;

    private void Awake()
    {
        if(lanes < 1)
        {
            lanes = 1;
        }
    }
}

namespace TrafficSimulation
{
    public struct RoadBufferElement : IBufferElementData
    {
        public int ID;
        public int speedLimit;
        public int lanes;
        public bool isOneWay;
        public int trafficAmount;

        public RoadNode roadNode;

        public bool isActive;
    }
}
