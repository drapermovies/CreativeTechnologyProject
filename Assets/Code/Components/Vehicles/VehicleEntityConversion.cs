using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace TrafficSimulation
{
    [Serializable]
    public struct VEntityConversion : IComponentData
    {
        public int amount;
        public Entity prefab;
        public TravelDirection roadDirection;
    }

    [RequiresEntityConversion]
    public class VehicleEntityConversion : MonoBehaviour, 
                                           IDeclareReferencedPrefabs, 
                                           IConvertGameObjectToEntity
    {
        public uint amount = 100;
        public GameObject prefab;
        public TravelDirection travelDirection;

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
                prefab = conversionSystem.GetPrimaryEntity(prefab),
                roadDirection = travelDirection
            };
            dstManager.AddComponentData(entity, spawnerData);
        }
    }

    public enum TravelDirection
    {
        LEFT,
        RIGHT
    }
}