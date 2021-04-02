using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrive : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {

        float maxAccelerarion = miAgente.mAceleracion;
        float maxSpeed = miAgente.mVelocidad;

        // Radio para llegar al objetivo
        float targetRadius = (float)target.rInterior;
        float slowRadius = (float)target.rExterior;

        float timeToTarget = 0.1f;

        // Empty Stering
        this.steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration

        Vector3 direction = target.transform.position - miAgente.transform.position;

        float distance = direction.magnitude;
        // Si ya hemos llegado no devolvemos stearing
        if (distance < targetRadius)
            return steering;
        // Si estamos fuera del slowRaidus vamos a maxima velocidad
        float targetSpeed = (distance < slowRadius) ? maxSpeed : maxSpeed * distance / slowRadius;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
