using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LookWhereYouGoing : Aling
{


    public override Steering GetSteering(AgentNPC miAgente)
    {
        steering = new Steering(0, new Vector3(0, 0, 0));
        // Vamosa  crear un nuevo target en la posicion donde estaria nuestro target
        Vector3 direction = target.transform.position - miAgente.transform.position;
        if (Math.Round(direction.magnitude) == 0)
            return steering;

        // TODO
        Vector3 predictedPosition = target.transform.position + target.vVelocidad;
        this.preditedRotation = (float)miAgente.MinAngleToRotate(predictedPosition);
        this.usePredicted = true;




        return base.GetSteering(miAgente);

    }
}
