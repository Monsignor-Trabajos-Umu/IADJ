using UnityEngine;

public class WallAvoidance2B : Seek
{
    // Distancia minima a la pared
    public float avoidDistance;

    // Distancia del rayo
    [SerializeField] private float leftWhiskerSize;
    [SerializeField] private float rightWhiskerSize;
    [SerializeField, Range(10,90)] private float range;

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

        var leftVector = Quaternion.AngleAxis(-range, Vector3.up) * rayVector * leftWhiskerSize;
        var rightVector = Quaternion.AngleAxis(range, Vector3.up) * rayVector * rightWhiskerSize;

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

            Debug.DrawRay(miAgente.transform.position, rightVector,
                Color.yellow);
        }

        var newTargetPoint = new Vector3(0, 0, 0);
        var hitPoint = new Vector3(0, 0, 0);

        var localTarget = new Vector3(0, 0, 0);

        if (leftWhiskerHit)
        {
            hitPoint = leftHit.point;
            newTargetPoint = leftHit.point + leftHit.normal * avoidDistance;
        }else
        {
            if (!rightWhiskerHit)
                return steering;
            hitPoint = rightHit.point;
            newTargetPoint = rightHit.point + rightHit.normal * avoidDistance;
        }

        UseCustomDirectionAndRotation(newTargetPoint - miAgentePosition);

        steering = base.GetSteering(miAgente);
        if (debug && (leftWhiskerHit || rightWhiskerHit))
        {
            // Si hay hit 
            Debug.DrawLine(hitPoint, newTargetPoint, Color.red); // Hit a la pared
            Debug.DrawLine(miAgentePosition, hitPoint, Color.green);
        }

        return steering;
    }
}