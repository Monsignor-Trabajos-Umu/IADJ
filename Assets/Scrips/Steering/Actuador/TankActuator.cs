using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//La unidad de tanque solo se mueve hacia adelante o atrás
// Si el destino se encuentra en una direccion distinta a donde mira el objeto
// se realiza una rotacion antes de mover

public class TankActuator : BaseActuator
{
    public override Steering Act(Steering steering) => throw new System.NotImplementedException();

    public override Steering Act(Steering steering, Vector3 angulo)
    {
        Steering acted = new Steering(0, new Vector3(0, 0, 0));
        //if (rotared)
        //{
        //    acted.angular
        //}
        return acted;

    }
}
