using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VelocityMatch : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {

        float maxAccelerarion = miAgente.mAcceleration;

        float timeToTarget = 0.1f;

        // Empty Stering
        this.steering = new Steering(0, new Vector3(0, 0, 0));

        // Obtenemos la rotacion hacie el objetivo

        steering.lineal = this.target.vVelocidad - miAgente.vVelocidad;
        steering.lineal /= timeToTarget;

        // Si vamos muy rapido la normalizamos
        if (steering.lineal.magnitude > maxAccelerarion)
        {
            steering.lineal.Normalize();
            steering.lineal *= maxAccelerarion;
        }

        steering.angular = 0;
        return this.steering;
    }

}
