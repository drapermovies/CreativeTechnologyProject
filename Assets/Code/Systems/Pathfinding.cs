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
    public class Pathfinding : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBuffer;

        protected override void OnCreate()
        {
            entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct FindNearestJunction : IJobForEachWithEntity<Translation, MovementData>
        {
            public EntityCommandBuffer.Concurrent commandBuffer;

            [DeallocateOnJobCompletion]
            [ReadOnly]
            public NativeArray<Translation> translations;

            [BurstCompile]
            public void Execute(Entity entity, 
                                int index,
                                ref Translation position, 
                                ref MovementData movementData)
            {
                ECSMathsPlus eCSMathsPlus = new ECSMathsPlus();

                if (eCSMathsPlus.CompareFloat3Values(movementData.startPos, float3.zero))
                {
                    movementData.startPos = position.Value;
                }

                float shortestDistance = int.MaxValue;
                int shortDistanceID = -1;

                for(int i = 0; i < translations.Length; i++)
                {
                    float currentDistance = eCSMathsPlus.Float3Distance(position.Value, 
                                                                        translations[i].Value);
                    if (currentDistance < shortestDistance)
                    {
                        shortestDistance = currentDistance;
                        shortDistanceID = i;
                    }
                }

                //Next travel point is the junction
                movementData.waypointLocation = translations[shortDistanceID].Value;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityQuery junctions = EntityManager.CreateEntityQuery(typeof(Translation),
                                                        typeof(JunctionData));
            FindNearestJunction junction = new FindNearestJunction
            {
                commandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent(),
                translations = junctions.ToComponentDataArray<Translation>(Allocator.TempJob)
        };

            JobHandle junctionHandle = junction.Schedule(this, inputDeps);

            entityCommandBuffer.AddJobHandleForProducer(junctionHandle);

            return junctionHandle;
        }
    }
}
