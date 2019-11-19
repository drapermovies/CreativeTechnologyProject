using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class RotationSpeedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float degrees_per_second = 360;

    // The MonoBehaviour data is converted to ComponentData on the entity.
    // We are specifically transforming from a good editor representation of the data (Represented in degrees)
    // To a good runtime representation (Represented in radians)
    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversion_system)
    {
        RotationSpeed data = new RotationSpeed { radians_per_second = math.radians(degrees_per_second) };
        manager.AddComponentData(entity, data);
    }
}