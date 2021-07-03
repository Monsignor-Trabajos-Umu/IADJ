using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Steering
{
    public float angular;// O rotacion
    public Vector3 lineal; // O velocidad

    public float rotacion { get => angular; set => angular = value; }
    public Vector3 velocidad { get => lineal; set => lineal = value; }

    public Steering(float angular, Vector3 lineal)
    {
        this.angular = angular;
        this.lineal = lineal;
    }
    public Steering(float angular)
    {
        this.angular = angular;
        this.lineal = new Vector3(0, 0, 0);
    }
    public Steering(Vector3 lineal)
    {
        this.angular = 0;
        this.lineal = lineal;
    }


    public override string ToString() => $"( Angular = {angular} | Lineal = {lineal}";
}