using System;
using UnityEngine;

public class WallFollowingCircular : Seek
{
    // Distancia del rayo
    [SerializeField ,Range(0.0f, 10f)] private float leftWhiskerSize;


    [SerializeField,Range(0.0f, 10f)] private float rightWhiskerSize;
    /* No hace falda dado que se mantiene a la misma distancia de la pared
     * Distancia minima a la pared
     *[SerializeField, Range(0.0f, 10f)] private float wallDistance;
     */
    [SerializeField] private bool useStandalone;
    [SerializeField,Range(0.0f, 90f)] private float whiskerAngle;


    private void Start()
    {
        steeringGroup = SteeringGroup.Collision;
    }


    public override Steering GetSteering(AgentNpc miAgente)
    {
        // Calculamos el target para delegarlo a seek
        steering = new Steering(0, new Vector3(0, 0, 0));

        // Añadimos un goforward 

        var miAgentePosition = miAgente.transform.position;
        var rayVector = useStandalone?miAgente.OrientationToVector():miAgente.vVelocidad;
        var leftVector = Quaternion.AngleAxis(-(whiskerAngle % 90), Vector3.up) *
                         rayVector * leftWhiskerSize;
        var rightVector = Quaternion.AngleAxis((whiskerAngle % 90), Vector3.up) *
                          rayVector * rightWhiskerSize;

        if (debug)
        {
            //Pinto los 2 bigotes
            Debug.DrawRay(miAgente.transform.position, leftVector,
                Color.yellow);
            Debug.DrawRay(miAgente.transform.position, rightVector,
                Color.yellow);
        }

        var leftWhiskerHit = Physics.Raycast(miAgentePosition, leftVector,
            out var hit, leftWhiskerSize);

        if (!leftWhiskerHit || !hit.collider.CompareTag("WallCircle"))
        {
            var rightWhiskerHit = Physics.Raycast(miAgentePosition, rightVector, out hit,
                rightWhiskerSize);
            if ( !rightWhiskerHit || !hit.collider.CompareTag("WallCircle") )
            {
                whiskerAngle += 5;
                return steering;
            }
        }

        var angle = leftWhiskerHit ? -10f : 10f;
        var newTargetPoint = Quaternion.AngleAxis(angle, Vector3.up) *
            (miAgente.transform.position -
             hit.transform.position) + hit.transform.position;

        if (debug)
        {
            Debug.DrawLine(hit.transform.position, newTargetPoint, Color.blue);
            Debug.DrawLine(hit.transform.position, miAgente.transform.position,
                Color.green);
            // Si hay hit 
            Debug.DrawLine(newTargetPoint, miAgentePosition, Color.red); // Hit a la pared
            //Debug.DrawLine(miAgentePosition, hitPoint, Color.green);
        }

        UseCustomDirectionAndRotation(newTargetPoint - miAgentePosition);
        return base.GetSteering(miAgente);
    }
}