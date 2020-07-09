using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace TrafficSimulation
{
    [Serializable]
    public struct JunctionData : IComponentData
    {
        public float3 position;
        public float currentLightsTime;
        public float maxLightsCountdown;
        public DynamicBuffer<RoadBufferElement> roadConnections;
    }

    public struct RoadBufferElement : IBufferElementData
    {
        public int ID;
        public int speedLimit;
        public int lanes;
        public bool isOneWay;
        public int trafficAmount;

        public bool isActive;
    }

    public class JunctionComponent : MonoBehaviour,
                                     IConvertGameObjectToEntity
    {
        public Transform[] connections;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem system)
        {
            //Convert road objects to ECS Compatible data
            DynamicBuffer<RoadBufferElement> roadBuffer = dstManager.AddBuffer<RoadBufferElement>(entity);
           
            for(int i = 0; i < connections.Length; i++)
            {
                RoadBufferElement element = new RoadBufferElement();
                element.ID = connections[i].GetInstanceID();
                element.speedLimit = connections[i].GetComponent<RoadValues>().speedLimit;
                element.lanes = connections[i].GetComponent<RoadValues>().lanes;
                element.trafficAmount = 0;
                element.isOneWay = connections[i].GetComponent<RoadValues>().isOneWay;

                if(i == 0)
                {
                    element.isActive = true;
                }
                else
                {
                    element.isActive = false;
                }
                roadBuffer.Add(element);
            }

            dstManager.AddComponentData(entity, new TrafficSimulation.JunctionData
            {
                position = transform.position,
                maxLightsCountdown = 3.0f,
                currentLightsTime = 0.0f,
                roadConnections = roadBuffer
            });
        }
    }
}
