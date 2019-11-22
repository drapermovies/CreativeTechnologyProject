using Unity.Entities; //ECS Library 

public struct Spawner : IComponentData
{
    public int count_x;
    public int count_y;
    public Entity prefab;
}
