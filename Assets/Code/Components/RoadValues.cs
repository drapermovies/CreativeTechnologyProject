using System;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TrafficSimulation
{
    [Serializable]
    public struct RoadBufferElement : IBufferElementData
    {
        public int ID;
        public Entity entity;
    }

    [Serializable]
    public struct RoadComponentData : IComponentData
    {
        public int ID;
        public int speedLimit;
        public int lanes;
        public bool isOneWay;
        public int trafficAmount;

        public RoadBufferElement bufferElement;

        public bool isActive;
    }

    public class RoadValues : MonoBehaviour,
                              IConvertGameObjectToEntity
    {
        public int id { get; private set; }
        public int speedLimit = 30;
        public int lanes = 1;
        public bool isOneWay;
        
        public void Convert(Entity entity,
                            EntityManager dstManager,
                            GameObjectConversionSystem conversionSystem)
        {
            if (lanes < 1)
            {
                lanes = 1;
            }

            Vector3 objScale = this.GetComponent<Transform>().localScale;
            objScale.z = 2 * lanes * (isOneWay ? 1 : 2);

            dstManager.SetComponentData(entity, new NonUniformScale()
            {
                Value = objScale
            });

            dstManager.AddComponentData(entity, new RoadComponentData()
            {
                ID = this.GetInstanceID(),
                speedLimit = speedLimit,
                lanes = lanes,
                isOneWay = isOneWay,
                trafficAmount = 0,
                bufferElement = new RoadBufferElement(),
                isActive = false
            });
        }
    }
}
