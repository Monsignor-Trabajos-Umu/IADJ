using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallAvoidance1B : Seek
{

    // Distancia minima a la pared
    public float avoidDistance;
    // Distancia del rayo
    public float lookAhead;

    public override Steering GetSteering(AgentNPC miAgente)
    {
        // Calculamos el target para delegarlo a seek
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        Vector3 rayVector = miAgente.vVelocidad;
        rayVector = rayVector.normalized;
        rayVector *= lookAhead;

        Debug.DrawRay(miAgente.transform.position, Quaternion.AngleAxis(30, Vector3.up) * (rayVector.normalized * lookAhead), Color.blue);
        RaycastHit hit;
        //Debug.DrawRay(miAgente.transform.position, rayVector, Color.blue);
        if (Physics.Raycast(miAgente.transform.position, rayVector, out hit, lookAhead))
        {

            Vector3 predictedPosition = hit.point + hit.normal * avoidDistance;
            base.UseCustomDirectionAndRotation(predictedPosition - miAgente.transform.position);
            /*
            this.predictedPosition = hit.point + normalPared * avoidDistance;
            this.usePredicted = true;
            */
            Debug.DrawLine(miAgente.transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, hit.normal * avoidDistance, Color.green);

            steering = base.GetSteering(miAgente);

        }
        return steering;
    }

}