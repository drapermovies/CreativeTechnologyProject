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

        public float3 startPos;
        public float3 endGoal; //Movement Destination

        public float accelerationTime; //Time to reach full speed

        public float3 waypointLocation; //Next point of travel

        public TransportType transportType;
    }

    public class MovementComponent : MonoBehaviour, 
                                     IConvertGameObjectToEntity
    {
        public float currentSpeed;
        public int maxSpeed;

        public TransportType transportType;

        public void Convert(Entity entity, 
                            EntityManager dstManager, 
                            GameObjectConversionSystem system)
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(2);

            //Adds Car Data
            dstManager.AddComponentData(entity, new TrafficSimulation.MovementData
            {
                currentSpeed = currentSpeed,
                maxSpeed = maxSpeed,
                startPos = this.GetComponent<Transform>().position,
                endGoal = new float3(random.NextFloat(-100f, 100f), 
                                     0, random.NextFloat(-100f, 100f)),
                transportType = transportType,
                accelerationTime = 6.0f,
            });

            if (transportType != TransportType.PERSON)
            {
                //We need to initialise it, but not set the data just yet
                dstManager.AddComponentData(entity, new TrafficSimulation.VehicleData
                {});
            }
        }
    }

    public enum TransportType
    {
        PERSON = 0,
        CAR = 1,
        BUS = 2
    }
}
