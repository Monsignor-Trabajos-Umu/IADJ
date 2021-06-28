using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wander : Align
{
    // https://answers.unity.com/questions/421968/normal-distribution-random.html
    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
        float u, v, S;

        do
        {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }
    // Maximo rate es decir cuanto cambia la rotacion
    [Range(0.0f, 180f)]
    public float wanderRate;
    // Wander orientation
    public float wanderOrientation = 0;
    // Wander step
    [Range(0.0f, 50f)]
    public float wanderStep;

    // TODO Cambiar los valores y los nombres de las variables.
    //No calulamos un nuevo punto en x segundos
    [Range(0.0f, 10f)]
    public float wanderWait; //Tiempo en segundos
    private float wanderPrivateCounter = 0;
    private Vector3 targetPoint;
    private bool targetExists = false;
    public override Steering GetSteering(AgentNpc miAgente)
    {
        float wanderWaitCounter = wanderWait * 60;
        this.timeToTarget = wanderWait;

        if (targetExists  && wanderPrivateCounter <= wanderWaitCounter)
        {
            if (Vector3.Distance(targetPoint, miAgente.transform.position) <= (float) miAgente.RExterior)
            {
                targetExists = false;
            }

            // Calculo la minima rotacion para llegar a ese punto ,
            // si puede que sea rebundante pero mentalmente me resulta mas claro.
            this.customRotation = (float)miAgente.MinAngleToRotate(targetPoint);
            this.useCustom = true;
            steering = base.GetSteering(miAgente);

            steering.lineal = this.targetPoint - miAgente.transform.position;
            steering.lineal = RemoveY(steering.lineal); // Filtramos la y
            steering.lineal.Normalize();
            steering.lineal *= miAgente.mAcceleration;
            wanderPrivateCounter++;

            return steering;
        }

        // Calculamos la orientacion y un nuevo punto

        wanderOrientation += RandomGaussian(-1f, 1f) * wanderRate;
        double targetOrientation = wanderOrientation + miAgente.orientacion;

        //Calculo el vector forward que a donde mira miAngente
        Vector3 forward = miAgente.OrientationToVector();
        // Genero la nueva posicion aplicándole una matriz de rotacion on el nuevo angulo


        //Vector3 targetVector = new Vector3(0, 0, 0);
        /*  O Usamos Quaternion.AngleAxis() :/
            //Vamos a aplicar matriz de rotacion
            targetVector.x = (float)(Math.Cos(targetOrientation) * forward.x - Math.Sin(targetOrientation) * forward.z);
            targetVector.y = forward.y;
            targetVector.z = (float)(Math.Sin(targetOrientation) * forward.x + Math.Cos(targetOrientation) * forward.z);
        */
        Vector3 targetVector = Quaternion.AngleAxis((float)targetOrientation, Vector3.up) * forward;

        //Normalizamos el vector
        targetVector.Normalize();

        // Ecuacion de la recta x,y,z = P + k * vector
        this.targetPoint = miAgente.transform.position + wanderStep * targetVector;

        // Calculo la minima rotacion para llegar a ese punto ,
        // si puede que sea rebundante pero mentalmente me resulta mas claro.
        this.customRotation = (float)miAgente.MinAngleToRotate(this.targetPoint);
        this.useCustom = true;
        steering = base.GetSteering(miAgente);


        steering.lineal = this.targetPoint - miAgente.transform.position;
        steering.lineal = RemoveY(steering.lineal); // Filtramos la y
        steering.lineal.Normalize();
        steering.lineal *= miAgente.mAcceleration;
        targetExists = true;
        wanderPrivateCounter = 0;

        
        return steering;

    }

    protected override void OnDrawGizmos()
    {
        if(!debug) return;
        base.OnDrawGizmos();
        Gizmos.DrawSphere(targetPoint, 2);
    }
}

