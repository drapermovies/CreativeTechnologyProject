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

        struct MovementJob : IJobForEachWithEntity<MovementData, Translation,
                                                   LocalToWorld, Rotation> 
        {
            public EntityCommandBuffer.Concurrent commandBuffer;
            public float deltaTime;

            [BurstCompile]
            public void Execute(Entity instance,
                                int index,
                                ref MovementData movement,
                                ref Translation translation,
                                ref LocalToWorld worldPosition,
                                ref Rotation rotation)
            {
                ECSMathsPlus eCSMaths = new ECSMathsPlus();

                //We don't have a movement direction
                //if (eCSMaths.CompareFloat3Values(movement.direction, float3.zero))
                //{
                //    movement.direction = worldPosition.Forward;
                //}

                float3 posChange = (deltaTime * movement.currentSpeed) * movement.direction;
                float3 newPosition = math.transform(worldPosition.Value, posChange);

                if (movement.currentSpeed < movement.maxSpeed)
                {
                    movement.currentSpeed += ((movement.maxSpeed * deltaTime) / 
                                               movement.accelerationTime);
                }

                float3 targetPos = movement.waypointLocation;
                targetPos *= Mathf.Deg2Rad;
                float3 forward = math.atan2(targetPos, newPosition);

                quaternion lookAtPoint = quaternion.LookRotation(forward,
                                                                 worldPosition.Up);

                quaternion lookAt = new quaternion(0, lookAtPoint.value.y,
                                                   0, lookAtPoint.value.w);

                lookAt = math.normalize(lookAt);

                rotation.Value = lookAt;

                translation.Value = newPosition;

                worldPosition.Value = Matrix4x4.TRS(newPosition,
                                                    lookAt,
                                                    Vector3.one);
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