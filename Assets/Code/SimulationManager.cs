using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;

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
                NativeArray<Entity> entities = new NativeArray<Entity>(spawnAmount, Allocator.Temp);

                Unity.Mathematics.Random random = new Unity.Mathematics.Random(100);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Entity instance = entities[i] = commandBuffer.Instantiate(index, vehicle.prefab);

                    float3 newPos = new float3(random.NextFloat(-100f, 100f), 0,
                                                 random.NextFloat(-100f, 100f));

                    float3 position = math.transform(localToWorld.Value, newPos);

                    commandBuffer.SetComponent(index, instance, new Translation { Value = position });
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