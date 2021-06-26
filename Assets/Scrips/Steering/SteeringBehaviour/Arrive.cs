using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrive : SteeringBehaviour
{


    [SerializeField] private float distance;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float targetSpeed;
    [SerializeField] private Vector3 targetVelocity;

    public override Steering GetSteering(AgentNPC miAgente)
    {

        float maxAccelerarion = miAgente.mAcceleration;
        float maxSpeed = miAgente.mVelocity;
        float targetRadius;
        float slowRadius;
        if (target != null)
        {
            // El radio /hitbox de nuestro objetivo mas el nuestro 
            targetRadius = (float)(target.rInterior + miAgente.rInterior);

            // La distancia que tenemos que empezar a reducir para no chocarnos
            slowRadius = (float)(target.RExterior + miAgente.RExterior);
        }
        else
        {
            targetRadius = (float) miAgente.rInterior;
            slowRadius = (float) miAgente.RExterior;
        }
        float timeToTarget = Time.deltaTime;

        // Empty Stering
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        // Si existe ya una direcion predecida usamos esa sino la calculamos


        direction = this.useCustom ? this.customDirection : target.transform.position - miAgente.transform.position;
        // Filtramos la y 
        direction = RemoveY(direction);
        distance = direction.magnitude;
        // Si ya hemos llegado no devolvemos stearing
        if (distance < targetRadius)
            return steering;
        // Si estamos fuera del slowRaidus vamos a maxima velocidad
        targetSpeed = (distance > slowRadius) ? maxSpeed : maxSpeed * distance / slowRadius;
        //Combinamos la velocidad con la direccion
        Vector3 temp = direction;
        temp.Normalize();
        targetVelocity = temp * targetSpeed;

        //Intentamos crear una aceleraccion que consiga esa velocidad

        steering.lineal = targetVelocity - miAgente.vVelocidad;
        steering.lineal /= timeToTarget;
        //Si vamos muy rapido la normalizamos
        if (steering.lineal.magnitude > maxAccelerarion)
        {
            steering.lineal.Normalize();
            steering.lineal *= maxAccelerarion;
        }

        steering.angular = 0;
        return this.steering;

    }

}
