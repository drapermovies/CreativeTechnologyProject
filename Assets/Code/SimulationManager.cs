using System;

using UnityEngine;

using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

namespace TrafficSimulation
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SimulationManager : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBuffer;

        EntityQuery transformsQuery;

        protected override void OnCreate()
        {
            entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            transformsQuery = EntityManager.CreateEntityQuery(typeof(RoadComponentData),
                                                              typeof(Translation));
        }

        struct SpawnJob : IJobForEachWithEntity<VEntityConversion, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent commandBuffer;

            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<Translation> roadPositions;

            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<RoadComponentData> roads;

            [BurstCompile]
            public void Execute(Entity entity, 
                                int index,
                                ref VEntityConversion vehicle,
                                [ReadOnly] ref LocalToWorld localToWorld)
            {
        
                int spawnAmount = (int)vehicle.amount;
                NativeArray<Entity> entities = new NativeArray<Entity>(spawnAmount, 
                                                                       Allocator.Temp);

                Unity.Mathematics.Random random = new Unity.Mathematics.Random(1);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Entity instance = entities[i] = commandBuffer.Instantiate(index, 
                                                                              vehicle.prefab);

                    int selectionID = random.NextInt(0, roadPositions.Length);
                    float3 newPos = roadPositions[selectionID].Value;

                    newPos.y = 0.5f;

                    float3 position = math.transform(localToWorld.Value, newPos);

                    //Physical Space Location
                    commandBuffer.SetComponent(index, instance, new Translation 
                    { 
                        Value = position 
                    });

                    float carRotation = 90f * Mathf.Deg2Rad;

                    quaternion rotationValue = math.normalize(quaternion.Euler(new float3(0, carRotation, 0)));

                    //Rotation
                    commandBuffer.SetComponent(index, instance, new Rotation
                    {
                        Value = rotationValue
                    });

                    //Render Location
                    commandBuffer.SetComponent(index, instance, new LocalToWorld()
                    {
                        Value = Matrix4x4.TRS(position,
                                              rotationValue,
                                              Vector3.one)
                    });
                }
                commandBuffer.DestroyEntity(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle job = new SpawnJob
            {
                commandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent(),
                roadPositions = transformsQuery
                                .ToComponentDataArray<Translation>(Allocator.TempJob),
                roads = transformsQuery.ToComponentDataArray<RoadComponentData>(Allocator.TempJob)

        }.Schedule(this, inputDeps);

            entityCommandBuffer.AddJobHandleForProducer(job);

            return job;
        }
    }
}