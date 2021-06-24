using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFollowing : Seek
{
    // Distancia minima a la pared
    public float avoidDistance;

    // Distancia del rayo
    [SerializeField] private float leftWhiskerSize;
    [SerializeField] private float midWhiskerSize;
    [SerializeField] private float rightWhiskerSize;


    private void Start()
    {
        steeringGroup = SteeringGroup.Collision;
    }

    public override Steering GetSteering(AgentNPC miAgente)
    {

        // Calculamos el target para delegarlo a seek
        steering = new Steering(0, new Vector3(0, 0, 0));
        var miAgentePosition = miAgente.transform.position;
        var rayVector = miAgente.vVelocidad.normalized;
        var middleVector = rayVector * midWhiskerSize;
        var leftVector = Quaternion.AngleAxis(-15f, Vector3.up) * rayVector * leftWhiskerSize;
        var rightVector = Quaternion.AngleAxis(15f, Vector3.up) * rayVector * rightWhiskerSize;


        var midWhiskerHit = Physics.Raycast(miAgentePosition, middleVector,
            out var midHit, midWhiskerSize);
        var leftWhiskerHit = Physics.Raycast(miAgentePosition, leftVector,
            out var leftHit, leftWhiskerSize);
        var rightWhiskerHit =
            Physics.Raycast(miAgentePosition, rightVector, out var rightHit,
                rightWhiskerSize);

        if (debug)
        {
            //Pinto los tres bigotes
            Debug.DrawRay(miAgente.transform.position, leftVector,
                Color.yellow);
            Debug.DrawRay(miAgente.transform.position, middleVector,
                Color.blue);
            Debug.DrawRay(miAgente.transform.position, rightVector,
                Color.yellow);
        }


        if (midWhiskerHit == leftWhiskerHit == rightWhiskerHit == false) return steering;


        var newTargetPoint = new Vector3(0, 0, 0);
        var hitPoint = new Vector3(0, 0, 0);

        var localTarget = new Vector3(0, 0, 0);
        int hits = 0;

        if (midWhiskerHit)
        {
            hitPoint = midHit.point;
            newTargetPoint = midHit.point + midHit.normal * avoidDistance;
            localTarget += miAgente.transform.InverseTransformPoint(newTargetPoint);
            hits++;
        }
        if (leftWhiskerHit)
        {

            hitPoint = leftHit.point;
            newTargetPoint = leftHit.point + leftHit.normal * avoidDistance;
            localTarget += miAgente.transform.InverseTransformPoint(newTargetPoint);
            hits++;
        }

        if (rightWhiskerHit)
        {
            hitPoint = rightHit.point;
            newTargetPoint = rightHit.point + rightHit.normal * avoidDistance;
            localTarget += miAgente.transform.InverseTransformPoint(newTargetPoint);
            hits++;
        }

        newTargetPoint = miAgente.transform.TransformPoint(localTarget / hits);
        UseCustomDirectionAndRotation(newTargetPoint - miAgentePosition);

        steering = base.GetSteering(miAgente);
        if (debug)
        {
            // Si hay hit 
            Debug.DrawLine(hitPoint, newTargetPoint, Color.red); // Hit a la pared
            Debug.DrawLine(miAgentePosition, hitPoint, Color.green);
        }

        return steering;
    }
}
