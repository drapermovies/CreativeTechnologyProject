using System.Collections.Generic;

using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

using Unity.Mathematics;
using Unity.Transforms;

//Automatically spawns the 'vehicles' 
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class VehicleSpawnerSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem entity_command_buffer;
    protected override void OnCreate()
    {
        entity_command_buffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    struct SpawnJob : IJobForEachWithEntity<VehicleComponent, LocalToWorld>
    {
        public EntityCommandBuffer.Concurrent command_buffer;

        public void Execute(Entity entity, int index, ref VehicleComponent vehicle, ref LocalToWorld location)
        {
            for (uint i = 0; i < vehicle.cars_to_spawn; i++)
            {
                Unity.Mathematics.Random random = new Unity.Mathematics.Random(i + 1);

                Entity instance = command_buffer.Instantiate(index, vehicle.prefab);

                float3 position = math.transform(location.Value, new float3(i * 4.3f, 0, 0));

                //Set vehicle variables here
                vehicle.id = i;

                vehicle.pos = position;
                vehicle.end_pos = GenerateEndPos(i);
                vehicle.caution_rating = random.NextFloat(0, 100);
                vehicle.top_speed = random.NextUInt(120, 250);

                command_buffer.SetComponent(index, instance, new Translation { Value = position });
            }
            command_buffer.DestroyEntity(index, entity);
        }

        private float3 GenerateEndPos(uint id)
        {
            float3 pos = new float3(0, 0, 0);
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(id + 1);

            //GameObject[] objects = GameObject.FindGameObjectsWithTag("Building");
            //Get
            //List<float3> end_pos = new List<float3>();

            //foreach(GameObject obj in objects)
            //{
            //    end_pos.Add(obj.transform.position);
            //}

            //int num = random.NextInt(0, end_pos.Count);

            //pos = end_pos[num];

            return pos;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle job = new SpawnJob
        {
            command_buffer = entity_command_buffer.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);

        entity_command_buffer.AddJobHandleForProducer(job);

        return job;
    }
}