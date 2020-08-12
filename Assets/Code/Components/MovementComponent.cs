using System;
using UnityEngine;

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

        public float3 direction;
    }

    public class MovementComponent : MonoBehaviour, 
                                     IConvertGameObjectToEntity
    {
        public int maxSpeed;

        public TransportType transportType;

        public void Convert(Entity entity, 
                            EntityManager dstManager, 
                            GameObjectConversionSystem system)
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(2);

            EntityQuery roads = dstManager.CreateEntityQuery(typeof(Translation),
                                                             typeof(RoadComponentData));

            NativeArray<Translation> translations = roads.ToComponentDataArray<Translation>(Allocator.TempJob);

            int randomNum = random.NextInt(0, translations.Length);

            //Adds Car Data
            dstManager.AddComponentData(entity, new TrafficSimulation.MovementData
            {
                currentSpeed = 0.0f,
                maxSpeed = maxSpeed,
                endGoal = translations[randomNum].Value,
                transportType = transportType,
                accelerationTime = 6.0f
            });

            if (transportType != TransportType.PERSON)
            {
                //We need to initialise it, but not set the data just yet
                dstManager.AddComponentData(entity, new TrafficSimulation.VehicleData
                {
                    currentDirection = 1
                });
            }

            translations.Dispose();
        }
    }

    public enum TransportType
    {
        PERSON = 0,
        CAR = 1,
        BUS = 2
    }
}
