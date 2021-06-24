using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LookWhereYouGoing : Align
{

    // Quiero que mi agente mire hacia delante es decir hacia donde tiene su vector velocidad
    public override Steering GetSteering(AgentNPC miAgente)
    {
        steering = new Steering(0, new Vector3(0, 0, 0));
        // Vamos a crear un nuevo target en la posicion donde estaria nuestro target
        Vector3 predictedPosition = miAgente.transform.position + miAgente.vVelocidad;
        this.customRotation = (float)miAgente.MinAngleToRotate(predictedPosition);
        this.useCustom = true;

        return base.GetSteering(miAgente);
    }
}
