using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadValues : MonoBehaviour
{
    public int id { get; private set; }
    public int speedLimit = 30;
    public int lanes = 1;
    public bool isOneWay;

    private void Awake()
    {
        if(lanes < 1)
        {
            lanes = 1;
        }
    }
}
