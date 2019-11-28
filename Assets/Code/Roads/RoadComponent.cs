using Unity.Entities;
using Unity.Mathematics;

public struct RoadComponent : IComponentData
{
    public bool is_open;
    public uint lanes;
    public RoadStates road_state;
    public float3 start_point;
    public float3 end_point;
}

public enum RoadStates
{
    one_way = 0, 
    two_way = 1,
}