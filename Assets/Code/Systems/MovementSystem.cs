using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Unity.Transforms;

namespace TrafficSimulation
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(SimulationManager))]
    public class MovementSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBuffer;

        protected override void OnCreate()
        {
            entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct MovementJob : IJobForEachWithEntity<MovementData, LocalToWorld> 
        {
            public EntityCommandBuffer.Concurrent commandBuffer;
            public float deltaTime;

            [BurstCompile]
            public void Execute(Entity instance,
                                int index,
                                ref MovementData movement,
                                ref LocalToWorld position)
            {
                float3 posChange = (deltaTime * movement.maxSpeed) * position.Forward;
                float3 newPosition = math.transform(position.Value, posChange);

                Quaternion rotation = new Quaternion().normalized;

                commandBuffer.SetComponent(index, instance, new LocalToWorld()
                {
                    Value = Matrix4x4.TRS(newPosition,
                                            rotation,
                                            Vector3.one)
                });
                commandBuffer.SetComponent(index, instance, new Translation 
                { 
                    Value = newPosition 
                });
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            MovementJob movementJob = new MovementJob
            {
                commandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent(),
                deltaTime = Time.deltaTime
            };

            JobHandle movementHandle = movementJob.Schedule(this, inputDeps);

            entityCommandBuffer.AddJobHandleForProducer(movementHandle);

            return movementHandle;
        }
    }
}