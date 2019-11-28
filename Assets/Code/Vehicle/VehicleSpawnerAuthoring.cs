using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class VehicleSpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private uint cars_to_spawn = 100;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        VehicleComponent vehicle_data = new VehicleComponent
        {
            prefab = conversionSystem.GetPrimaryEntity(prefab),
            speed = 0.0f,
            top_speed = 120,
            pos = new float3(0, 0, 0),
            end_pos = new float3(0, 0, 10),
            caution_rating = 25.0f,
            cars_to_spawn = cars_to_spawn
        };

        dstManager.AddComponentData(entity, vehicle_data);
    }
}