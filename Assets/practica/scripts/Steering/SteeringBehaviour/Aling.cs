﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Aling : SteeringBehaviour
{
    protected float timeToTarget = 0.1f;
    public override Steering GetSteering(AgentNPC miAgente)
    {

        float maxAngularAcceleration = miAgente.mAngularAceleracion;
        float maxRotation = miAgente.mRotacion;

        // Radio para llegar al objetivo
        float targetRadius = (float)miAgente.aInterior;
        float slowRadius = (float)miAgente.aExterior;

        // Empty Stering
        this.steering = new Steering(0, new Vector3(0, 0, 0));

        // Obtenemos la rotacion hacie el objetivo
        float rotation;
        if (this.usePredicted)
        {
            rotation = this.preditedRotation;
        }
        else
        {
            rotation = (float)miAgente.MinAngleToRotate(target.gameObject);
        }
        float rotationSize = Math.Abs(rotation);
        // Si ya estamos mirando no devolvemos stearing
        if (rotationSize < targetRadius)
            return steering;
        // Si estamos fuera del slowRaidus vamos a maxima rotacion
        float targetRotation = (rotationSize < slowRadius) ? maxRotation : maxRotation * rotationSize / slowRadius;
        //Combinamos la velocidad con la direccion
        targetRotation *= rotation / rotationSize;


        //Intentamos crear una aceleraccion que consiga esa velocidad

        steering.angular = targetRotation - miAgente.rotacion;
        steering.angular /= timeToTarget;

        // Si vamos muy rapido la normalizamos
        var angularAcceleration = Math.Abs(steering.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            steering.angular /= angularAcceleration;
            steering.angular *= maxAngularAcceleration;
        }

        steering.lineal = new Vector3(0, 0, 0);
        return this.steering;
    }

}
