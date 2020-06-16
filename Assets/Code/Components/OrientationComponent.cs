using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace TrafficSimulation
{
    [Serializable]
    public struct Orientation : IComponentData
    {
        public float3 worldPosition;
        public quaternion rotation;
    }
    
    public class OrientationComponent : MonoBehaviour,
                                        IConvertGameObjectToEntity
    {
        public float3 worldPosition;
        public quaternion rotation;

        public void Convert(Entity entity, EntityManager entityManager, 
                            GameObjectConversionSystem conversion)
        {
            entityManager.AddComponentData(entity, new TrafficSimulation.Orientation
            {
                worldPosition = worldPosition,
                rotation = rotation
            });
        }
    }
                                        
}