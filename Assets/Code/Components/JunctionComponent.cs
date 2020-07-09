using System;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace TrafficSimulation
{
    [Serializable]
    public struct JunctionData : IComponentData
    {
        public float currentLightsTime;
        public float maxLightsCountdown;
        public DynamicBuffer<RoadBufferElement> roadConnections;
    }

    public class JunctionComponent : MonoBehaviour,
                                     IConvertGameObjectToEntity
    {
        public Transform[] connections;

        public void Convert(Entity entity, 
                            EntityManager dstManager, 
                            GameObjectConversionSystem system)
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

                RoadNode aStarNode = new RoadNode();
                aStarNode.canTravelThrough = !element.isOneWay; //If it's not one way, we can travel along it#
                aStarNode.startPos = connections[i].GetComponent<Transform>().position - connections[i].GetComponent<Transform>().localScale;
                aStarNode.endPos = connections[i].GetComponent<Transform>().position + connections[i].GetComponent<Transform>().localScale;
                aStarNode.index = roadBuffer.Length; //We can only be equivalent to the size of the buffer

                element.roadNode = aStarNode;

                roadBuffer.Add(element);
            }

            dstManager.SetComponentData(entity, new Translation
            {
                Value = GetComponent<Transform>().position
            });

            dstManager.AddComponentData(entity, new TrafficSimulation.JunctionData
            {
                maxLightsCountdown = 3.0f,
                currentLightsTime = 0.0f,
                roadConnections = roadBuffer
            });
        }
    }
}
