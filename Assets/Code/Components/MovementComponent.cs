using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace TrafficSimulation
{
    [Serializable]
    public struct MovementData : IComponentData
    {
        public float currentSpeed;
        public int maxSpeed;
        public float3 endGoal;
    }
    public class MovementComponent : MonoBehaviour, 
                                     IConvertGameObjectToEntity
    {
        public float currentSpeed;
        public int maxSpeed;
        public float3 endGoal;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem system)
        {
            dstManager.AddComponentData(entity, new TrafficSimulation.MovementData
            {
                currentSpeed = currentSpeed,
                maxSpeed = maxSpeed,
                endGoal = endGoal
            });
        }
    }
}
