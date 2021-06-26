using System;
using Unity.Collections;
using UnityEngine;

public class WallFollowing : Seek
{
    // Distancia minima a la pared
    [SerializeField,Range(1,20)] private float distanceToWall=1;

    // Distancia del rayo
     [SerializeField]private float whiskerSize;

     private void Start()
    {
        steeringGroup = SteeringGroup.Collision;
    }

    public override Steering GetSteering(AgentNpc miAgente)
    {
        // Calculamos el target para delegarlo a seek
        steering = new Steering(0, new Vector3(0, 0, 0));

        // Usamos pitágoras para calcular la longitud de los bigotes
        // d^2 = k^2 + k^2...
        // d = 2 * sqrt(k)
        // d = d + 1 Sumamos uno para que siempre haga colisión
        // Siendo k la distancia a la pared


        whiskerSize = (float)(2 * Math.Sqrt(distanceToWall)) + 1 ;

        var miAgentePosition = miAgente.transform.position;
        var rayVector = miAgente.vVelocidad.normalized;  // Vector hacia deltante 

        var leftVector = Quaternion.AngleAxis(-45, Vector3.up) * rayVector * whiskerSize;
        var rightVector = Quaternion.AngleAxis(45, Vector3.up) * rayVector * whiskerSize;

        var leftWhiskerHit = Physics.Raycast(miAgentePosition, leftVector,
            out var leftHit, whiskerSize);
        var rightWhiskerHit =
            Physics.Raycast(miAgentePosition, rightVector, out var rightHit,
                whiskerSize);

        if (debug)
        {
            //Pinto los tres bigotes
            Debug.DrawRay(miAgente.transform.position, leftVector,
                Color.yellow);

            Debug.DrawRay(miAgente.transform.position, rightVector,
                Color.yellow);
        }

        Vector3 newTargetPoint;
        Vector3 hitPoint;

        if (leftWhiskerHit)
        {
            hitPoint = leftHit.point;
            newTargetPoint = leftHit.point + leftHit.normal * distanceToWall;
        }else
        {
            if (!rightWhiskerHit)
                return steering;
            hitPoint = rightHit.point;
            newTargetPoint = rightHit.point + rightHit.normal * distanceToWall;
        }

        UseCustomDirectionAndRotation(newTargetPoint - miAgentePosition);

        steering = base.GetSteering(miAgente);
        if (debug)
        {
            // Si hay hit 
            Debug.DrawLine(hitPoint, newTargetPoint, Color.green); // Hit a la pared
            Debug.DrawLine(miAgentePosition, hitPoint, Color.red); // Vector hacia la pared
        }

        return steering;
    }
}