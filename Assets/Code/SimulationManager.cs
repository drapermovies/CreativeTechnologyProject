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

        protected override void OnCreate()
        {
            entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct SpawnJob : IJobForEachWithEntity<VEntityConversion, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent commandBuffer;

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
                    Entity instance = entities[i] = commandBuffer.Instantiate(index, vehicle.prefab);

                    float3 newPos = new float3(random.NextFloat(-100f, 100f), 0,
                                                 random.NextFloat(-100f, 100f));

                    float3 position = math.transform(localToWorld.Value, newPos);

                    float3 eulerAngles = new float3(0f,
                                                    random.NextFloat(0f, 360f),
                                                    0f);

                    quaternion quaternion = quaternion.EulerXYZ(eulerAngles);

                    //Physical Space Location
                    commandBuffer.SetComponent(index, instance, new Translation { Value = position });
                    commandBuffer.SetComponent(index, instance, new Rotation { Value = quaternion });

                    //Render Location
                    commandBuffer.SetComponent(index, instance, new LocalToWorld()
                    {
                        Value = Matrix4x4.TRS(position,
                                              quaternion,
                                              Vector3.one)
                    });

                    float3 newEndGoal = random.NextFloat3(new float3(-100), new float3(100));
                    newEndGoal.y = 0.0f;

                    commandBuffer.SetComponent(index, instance, new TrafficSimulation.MovementData()
                    {
                        currentSpeed = 0.0f,
                        maxSpeed = 20,
                        endGoal = newEndGoal
                    });
                }
                commandBuffer.DestroyEntity(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle job = new SpawnJob
            {
                commandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDeps);

            entityCommandBuffer.AddJobHandleForProducer(job);

            return job;
        }
    }
}