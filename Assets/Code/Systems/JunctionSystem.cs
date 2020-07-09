using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;

namespace TrafficSimulation
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(SimulationManager))]
    public class JunctionSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem entityCommandBuffer;

        protected override void OnCreate()
        {
            entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }
        struct JunctionJob : IJobForEachWithEntity<JunctionData>
        {
            public float deltaTime;

            [BurstCompile]
            public void Execute(Entity instance,
                                int index,
                                ref JunctionData junctionData)
            {
                if (junctionData.currentLightsTime < junctionData.maxLightsCountdown)
                {
                    junctionData.currentLightsTime += deltaTime;
                }
                else
                {
                    junctionData.currentLightsTime = 0.0f;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JunctionJob junctionJob = new JunctionJob
            {
                deltaTime = Time.deltaTime,
            };

            JobHandle junctionHandle = junctionJob.Schedule(this, inputDeps);

            entityCommandBuffer.AddJobHandleForProducer(junctionHandle);

            return junctionHandle;
        }
    }
}
