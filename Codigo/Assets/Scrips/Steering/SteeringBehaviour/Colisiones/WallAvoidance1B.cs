using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallAvoidance1B : Seek
{
    // Distancia minima a la pared
    [SerializeField, Range(0,20)] private float avoidDistance=0;

    // Distancia del rayo
    [SerializeField, Range(1,40)]  private float lookAhead=1;
    

    public override Steering GetSteering(AgentNpc miAgente)
    {
        // Calculamos el target para delegarlo a seek
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        Vector3 rayVector = miAgente.vVelocidad;
        rayVector = rayVector.normalized;
        rayVector *= lookAhead;
        if(debug)
            Debug.DrawRay(miAgente.transform.position,  (rayVector.normalized * lookAhead), Color.yellow);
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
            if (debug)
            {
                Debug.DrawLine(miAgente.transform.position, hit.point, Color.red);
                Debug.DrawRay(hit.point, hit.normal * avoidDistance, Color.green);
            }
          

            steering = base.GetSteering(miAgente);

        }
        return steering;
    }

}