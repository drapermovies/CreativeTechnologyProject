using Unity.Entities;
using Unity.Jobs;

public class BuilderSpawner : JobComponentSystem
{
    EntityManager entity_manager; 

    protected override void OnCreate()
    {
        entity_manager = World.GetOrCreateSystem<EntityManager>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        throw new System.NotImplementedException();
    }
}
