using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BehaviorAndWeight
{
    public Steering behavior;
    public float weight;

    public BehaviorAndWeight(Steering behavior, float weight)
    {
        this.behavior = behavior;
        this.weight = weight;
    }
}

public abstract class ArbitroSteering : MonoBehaviour
{
    [SerializeField]
    public bool debugGreen = false;

    public abstract Steering GetSteering();
}
