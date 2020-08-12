using System;
using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace TrafficSimulation
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(SimulationManager))]
    public class VehicleDataSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBuffer;

        EntityQuery roadsQuery;

        ECSMathsPlus ECSMaths;

        protected override void OnCreate()
        {
            entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            roadsQuery = EntityManager.CreateEntityQuery(typeof(RoadComponentData),
                                                             typeof(Translation));
        }

        struct VehicleDataProcessing : IJobForEachWithEntity<VehicleData, Translation>
        {
            public EntityCommandBuffer.Concurrent commandBuffer;

            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<Translation> translation;

            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<RoadComponentData> roads;

            [BurstCompile]
            public void Execute(Entity entity,
                                int index,
                                ref VehicleData data,
                                ref Translation position)
            {
                if (data.currentRoadID == 0)
                {
                    float shortestDistance = float.MaxValue;
                    int translationID = 0;

                    for (int i = 0; i < translation.Length; i++)
                    {
                        ECSMathsPlus mathsPlus = new ECSMathsPlus();
                        float currentDistance = mathsPlus.Float3Distance
                                                (position.Value,
                                                 translation[i].Value);
                        if (currentDistance < shortestDistance)
                        {
                            shortestDistance = currentDistance;
                            translationID = i;
                        }
                    }

                    data.currentRoadID = roads[translationID].ID;
                }
            }
        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            VehicleDataProcessing vehicleJob = new VehicleDataProcessing
            {
                commandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent(),
                translation = roadsQuery.ToComponentDataArray<Translation>(Allocator.TempJob),
                roads = roadsQuery.ToComponentDataArray<RoadComponentData>(Allocator.TempJob)
            };
            
            JobHandle vehicleHander = vehicleJob.ScheduleSingle(this, inputDeps);

            entityCommandBuffer.AddJobHandleForProducer(vehicleHander);

            return vehicleHander;


        }
    }
}