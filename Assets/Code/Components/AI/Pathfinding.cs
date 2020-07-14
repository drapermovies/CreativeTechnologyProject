using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TrafficSimulation
{
    //public class Pathfinding : JobComponentSystem
    //{
    //    BeginInitializationEntityCommandBufferSystem entityCommandBuffer;

    //    protected override void OnCreate()
    //    {
    //        entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    //    }

    //    struct CalculateHeuristics : IJobForEachWithEntity_EBC<RoadBufferElement, Translation>
    //    {
    //        public EntityCommandBuffer.Concurrent commandBuffer;
    //        public float deltaTime;

    //        [BurstCompile]
    //        public void Execute(Entity instance,
    //                            int index,
    //                            DynamicBuffer<RoadBufferElement> roadBuffer,
    //                            ref Translation translation)
    //        {
    //        }
    //    }

    //    protected override JobHandle OnUpdate(JobHandle inputDeps)
    //    {
    //        CalculateHeuristics heuristics = new CalculateHeuristics
    //        {
    //            commandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent(),
    //            deltaTime = Time.deltaTime
    //        };

    //        JobHandle calculationsHandle = heuristics.Schedule(this, inputDeps);

    //        entityCommandBuffer.AddJobHandleForProducer(calculationsHandle);

    //        return calculationsHandle;
    //    }
    //}

    //public struct RoadNode
    //{
    //    public float3 startPos;
    //    public float3 endPos;

    //    public int index;

    //    public int gCost; //Heuristic cost from start node
    //    public int hCost; //Heuristic cost to end node
    //    public int fCost; //G & H combined values

    //    public bool canTravelThrough;

    //    public int previousRoadIndex;

    //    public void CalculateFCost()
    //    {
    //        fCost = gCost + hCost;
    //    }
    //}
}
