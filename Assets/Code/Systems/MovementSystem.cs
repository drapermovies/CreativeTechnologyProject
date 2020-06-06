using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;

namespace TrafficSimulation
{
    public class MovementSystem : JobComponentSystem
    {
        [BurstCompile]
        struct MovementJob : IJobForEach<Orientation, MovementData>
        {
            public float deltaTime;

            public void Execute(ref Orientation orientation, ref MovementData movement)
            {
                float3 position = orientation.worldPosition;

                position += deltaTime * movement.maxSpeed * math.forward(orientation.rotation);

                orientation.worldPosition = position;
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            MovementJob movementJob = new MovementJob
            {
                deltaTime = Time.deltaTime
            };

            JobHandle movementHandle = movementJob.Schedule(this, inputDeps);

            return movementHandle;
        }
    }
}