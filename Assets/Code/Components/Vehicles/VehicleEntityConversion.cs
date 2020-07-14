using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;

namespace TrafficSimulation
{
    [Serializable]
    public struct VEntityConversion : IComponentData
    {
        public int amount;
        public Entity prefab;
    }

    [RequiresEntityConversion]
    public class VehicleEntityConversion : MonoBehaviour, 
                                           IDeclareReferencedPrefabs, 
                                           IConvertGameObjectToEntity
    {
        public uint amount = 100;
        public GameObject prefab;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(prefab);
        }

        public void Convert(Entity entity, 
                            EntityManager dstManager, 
                            GameObjectConversionSystem conversionSystem)
        {
            VEntityConversion spawnerData = new VEntityConversion
            {
                amount = (int)amount,
                prefab = conversionSystem.GetPrimaryEntity(prefab)
            };
            dstManager.AddComponentData(entity, spawnerData);
        }
    }
}