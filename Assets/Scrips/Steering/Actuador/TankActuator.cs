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
    public override Steering Act(Steering steering, AgentNpc agenteNPC)
    {
        Steering acted = new Steering(0, new Vector3(0, 0, 0));
        
        //Si velocidad es ~0 se gira
        if (steering.lineal.magnitude < 0.01)
        {
            acted.angular = steering.angular;
            return acted;
        }
        var angulo = agenteNPC.MinAngleToRotateVector(steering.lineal);

        Debug.Log("Angulo es " + angulo);

        //Si estamos mirando objetivo avanzamos. Dejamos unos angulos de error
        if (angulo < 5f)
        {
            acted.lineal = steering.lineal;
            return acted;
        }

        //Si angulo es mayor de 5, se quiere mover en diagonal
        //primero devolvemos el angulo
        acted.angular = (float) angulo;
        return acted;

    }
}
