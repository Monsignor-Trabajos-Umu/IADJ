using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallAvoidance : Seek
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




        Debug.DrawRay(miAgente.transform.position, rayVector.normalized * lookAhead, Color.blue);
        RaycastHit hit;
        //Debug.DrawRay(miAgente.transform.position, rayVector, Color.blue);
        if (Physics.Raycast(miAgente.transform.position, rayVector, out hit, lookAhead))
        {
            Vector3 miAgenteHit = hit.point - miAgente.transform.position;
            Vector3 normalPared = Vector3.Reflect(miAgenteHit, hit.normal);



            this.predictedPosition = hit.point + normalPared * avoidDistance;
            this.usePredicted = true;

            Debug.DrawLine(miAgente.transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, normalPared * avoidDistance, Color.green);


            steering = base.GetSteering(miAgente);

        }
        return steering;


    }


}
