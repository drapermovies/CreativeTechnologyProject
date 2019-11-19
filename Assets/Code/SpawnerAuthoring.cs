using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class SpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject prefab;
    public int count_x = 0;
    public int count_y = 0;

    public void DeclareReferencedPrefabs(List<GameObject> referenced_prefabs)
    {
        referenced_prefabs.Add(prefab);
    }

    public void Convert(Entity entity, EntityManager manager, 
                        GameObjectConversionSystem conversion_system)
    {
        Spawner spawner_data = new Spawner
        {
            prefab = conversion_system.GetPrimaryEntity(prefab),
            count_x = count_x,
            count_y = count_y
        };

        manager.AddComponentData(entity, spawner_data);
    }
}