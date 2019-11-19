using Unity.Collections;
using Unity.Entities;
using Unity.Jobs; //Jobs compiler
using Unity.Mathematics; //ECS Compatible Maths
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SpawnerSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem m_entityCommandBufferSystem; //Command Buffer
    protected override void OnCreate()
    {
        m_entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    struct SpawnJob : IJobForEachWithEntity<Spawner, LocalToWorld>
    {
        public EntityCommandBuffer.Concurrent command_buffer; //Job Buffer

        public void Execute(Entity entity, int index, [ReadOnly] ref Spawner spawner,
                            [ReadOnly] ref LocalToWorld location)
        {
            for(uint x = 0; x < spawner.count_x; x++)
            {
                for(uint y = 0; y < spawner.count_y; y++)
                {
                    //Create Game Object
                    Entity instance = command_buffer.Instantiate(index, spawner.prefab);

                    //Initialise position
                    float3 position = math.transform(location.Value,
                                                     new float3(x * 1.3f, noise.cnoise(new float2(x, y) * 0.21f) * 2, y * 1.3f));

                    //Add to job queue 
                    command_buffer.SetComponent(index, instance, new Translation { Value = position });
                }
            }

            command_buffer.DestroyEntity(index, entity); //Destroy Game Object
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //Instead of performing structural changes directly, a Job can add a command to an EntityCommandBuffer to perform such changes on the main thread after the Job has finished.
        //Command buffers allow you to perform any, potentially costly, calculations on a worker thread, while queuing up the actual insertions and deletions for later.

        //Create job/task
        JobHandle job = new SpawnJob
        {
            command_buffer = m_entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);

        m_entityCommandBufferSystem.AddJobHandleForProducer(job);

        return job;
    }
}