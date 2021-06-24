using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//La unidad de tanque solo se mueve hacia adelante o atrás
// Si el destino se encuentra en una direccion distinta a donde mira el objeto
// se realiza una rotacion antes de mover

public class TankActuator : BaseActuator
{
    public override Steering Act(Steering steering) => throw new System.NotImplementedException();

    //steering indica el vector hacia donde esta el destino
    //direccion indica hacia donde se mira
    public override Steering Act(Steering steering, Vector3 direccion)
    {
        Steering acted = new Steering(0, new Vector3(0, 0, 0));

        var angulo = Vector3.Angle(steering.lineal, direccion);

        if (angulo>0)
        {
            acted.angular = angulo;
        }
        return acted;

    }
}
