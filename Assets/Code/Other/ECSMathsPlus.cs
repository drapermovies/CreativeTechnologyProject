using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace TrafficSimulation
{
    public struct IntBufferElement : IBufferElementData
    {
        public int Value;
    }

    public struct FloatBufferElement : IBufferElementData
    {
        public float Value;
    }

    public struct Float3BufferElement : IBufferElementData
    {
        public float3 Value;
    }

    public class ECSMathsPlus
    {
        public float Float3Distance(float3 position1, float3 position2)
        {
            float x = math.abs(position1.x - position2.x);
            float y = math.abs(position1.y - position2.y);
            float z = math.abs(position1.z - position2.z);

            float returnValue = (x + y + z) / 3;

            return returnValue;
        }

        public bool CompareFloat3Values(float3 value1, float3 value2)
        {
            if(value1.x == value2.x 
                && value1.y == value2.y 
                && value1.z == value2.z)
            {
                return true;
            }
            return false;
        }

        public quaternion LookAtPoint(float3 pointToLook)
        {
            float3 euler = new float3(0);

            euler.x = (float)Math.Atan2(pointToLook.y, pointToLook.z);
            euler.y = (float)Math.Atan2(pointToLook.x * Math.Cos(euler.x), pointToLook.z);
            euler.z = (float)Math.Atan2(Math.Cos(euler.x), Math.Sin(euler.x) * Math.Sin(euler.y));

            euler *= Mathf.Deg2Rad;

            return quaternion.Euler(euler);
        }
    }
}