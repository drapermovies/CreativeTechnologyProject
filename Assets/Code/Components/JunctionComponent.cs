using System;
using System.Reflection;

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
        public DynamicBuffer<IntBufferElement> connectedRoadIDs;
    }

    public class JunctionComponent : MonoBehaviour,
                                     IConvertGameObjectToEntity
    {
        public Transform[] connections;

        public void Convert(Entity entity, 
                            EntityManager dstManager, 
                            GameObjectConversionSystem system)
        {
            Vector3 position = GetComponent<Transform>().position;
            float3 quartenionPosition = new float3(0.01f);
            if(position.x != 0)
            {
                quartenionPosition.x = position.x;
            }
            if(position.y != 0)
            {
                quartenionPosition.y = position.y;
            }
            if(position.z != 0)
            {
                quartenionPosition.z = position.z;
            }

            dstManager.SetComponentData(entity, new Translation
            {
                Value = quartenionPosition
            });

            DynamicBuffer<IntBufferElement> connectedRoadID = dstManager.AddBuffer<IntBufferElement>(entity);

            for (int i = 0; i < connections.Length; i++)
            {
                connectedRoadID.Add(new IntBufferElement 
                { 
                    Value = connections[i].GetInstanceID() 
                });
            }

            dstManager.AddComponentData(entity, new TrafficSimulation.JunctionData
            {
                maxLightsCountdown = 3.0f,
                currentLightsTime = 0.0f,
                connectedRoadIDs = connectedRoadID
            });
        }
    }
}
