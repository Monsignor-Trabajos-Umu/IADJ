using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wander : Aling
{
    // https://answers.unity.com/questions/421968/normal-distribution-random.html
    private static float RandomGaussian()
    {
        return UnityEngine.Random.value - UnityEngine.Random.value;

    }
    // Radio y ofset
    public float wanderOffset;
    public float wanderRadius;

    // Maximo rate
    public float wanderRate;
    // Wander orientation
    public float wanderOrientation;

    [Range(0.0f, 10.0f)]
    public float maxAcceleration;
    public override Steering GetSteering(AgentNPC miAgente)
    {
        steering = new Steering(0, new Vector3(0, 0, 0));

        // Calculamos la orientacion

        wanderOrientation += RandomGaussian() * wanderRate;
        float targetOrientation = wanderOrientation + miAgente.orientacion;


        Vector3 targetPosition = miAgente.transform.position + wanderOffset * miAgente.OrientationToVector();
        // Genero la nueva posicion aplicándole una matriz de rotacion on el nuevo angulo
        targetPosition = Quaternion.Euler(0, 0, targetOrientation) * targetPosition;

        this.preditedRotation = targetOrientation;
        this.usePredicted = true;
        steering = base.GetSteering(miAgente);




        steering.lineal = maxAcceleration * miAgente.OrientationToVector();

        return steering;

    }


}

