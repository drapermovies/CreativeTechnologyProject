using Unity.Entities;
using Unity.Mathematics;

public struct VehicleComponent : IComponentData
{
    public uint id;
    public Entity prefab;
    public float speed;
    public uint top_speed;
    public float3 pos;
    public float3 end_pos;
    public float caution_rating;
    public uint cars_to_spawn;
}