using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Steering
{
    [SerializeField]
    public float angulo;
    [SerializeField]
    public Vector3 velocidad;
    public Steering(float angulo, Vector3 velocidad)
    {
        this.angulo = angulo;
        this.velocidad = velocidad;
    }

    public override string ToString() => $"( Angulo = {angulo} | Velocidad = {velocidad})";
}