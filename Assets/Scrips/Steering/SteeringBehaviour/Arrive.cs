using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrive : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {

        float maxAccelerarion = miAgente.mAcceleration;
        float maxSpeed = miAgente.mVelocity;

        // Radio para llegar al objetivo
        float targetRadius = (float)miAgente.rInterior;
        float slowRadius = (float)miAgente.RExterior;

        float timeToTarget = 0.1f;

        // Empty Stering
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        // Si existe ya una direcion predecida usamos esa sino la calculamos
        Vector3 direction = this.usePredicted ? this.predictedDirection : target.transform.position - miAgente.transform.position;
        float distance = direction.magnitude;
        // Si ya hemos llegado no devolvemos stearing
        if (distance < targetRadius)
            return steering;
        // Si estamos fuera del slowRaidus vamos a maxima velocidad
        float targetSpeed = (distance > slowRadius) ? maxSpeed : maxSpeed * distance / slowRadius;
        //Combinamos la velocidad con la direccion
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        //Intentamos crear una aceleraccion que consiga esa velocidad

        steering.lineal = targetVelocity - miAgente.vVelocidad;
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
