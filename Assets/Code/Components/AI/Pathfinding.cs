using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace TrafficSimulation
{ 
    public struct RoadNode
    {
        public float3 startPos;
        public float3 endPos;

        public int index;

        public int gCost; //Heuristic cost from start node
        public int hCost; //Heuristic cost to end node
        public int fCost; //G & H combined values

        public bool canTravelThrough;

        public int previousRoadIndex;

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }
}
