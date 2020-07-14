using System;
using UnityEngine;
using Unity.Collections;
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

            dstManager.SetComponentData(entity, new Translation
            {
                Value = GetComponent<Transform>().position
            });

            dstManager.AddComponentData(entity, new TrafficSimulation.JunctionData
            {
                maxLightsCountdown = 3.0f,
                currentLightsTime = 0.0f,
                //roadConnections = roadBuffer
            });
        }
    }
}
