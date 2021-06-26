using UnityEngine;

public class WallAvoidance2B : Seek
{
    // Distancia que nos queremos separar de la pared
    [SerializeField, Range(0,20)] private float avoidDistance=0;

    // Distancia del rayo
    [SerializeField, Range(1,40)]  private float leftWhiskerSize=1;
    [SerializeField, Range(1,40)]  private float rightWhiskerSize=1;
    [SerializeField, Range(10,90)] private float angle=10;

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

        var leftVector = Quaternion.AngleAxis(-angle, Vector3.up) * rayVector * leftWhiskerSize;
        var rightVector = Quaternion.AngleAxis(angle, Vector3.up) * rayVector * rightWhiskerSize;

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


        // No hay hit no hacemos nada
        if (!leftWhiskerHit && !rightWhiskerHit)
            return steering;


        Vector3 newTargetPoint;


        switch (leftWhiskerHit)
        {
            // Los dos hacen hit cogemos el punto medio
            case true when rightWhiskerHit:
            {
                var leftPoint = leftHit.point + leftHit.normal * avoidDistance;
                var rightPoint = rightHit.point + rightHit.normal * avoidDistance;
                newTargetPoint = (leftPoint + rightPoint) / 2;
                if (debug)
                {
                    // Hit a la paredes
                    Debug.DrawLine(miAgentePosition, leftHit.point, Color.red);
                    Debug.DrawLine(miAgentePosition, rightHit.point, Color.red);
                    
                    Debug.DrawLine(leftHit.point, newTargetPoint, Color.green);
                    Debug.DrawLine(rightHit.point, newTargetPoint, Color.green); 

                }

                break;
            }
            // Si solo lo hace el izquierdo
            case true:
                newTargetPoint = leftHit.point + leftHit.normal * avoidDistance;
                if (debug)
                {
                    // Hit a la Izquierdo
                    Debug.DrawLine(miAgentePosition, leftHit.point, Color.red);
                    Debug.DrawLine(leftHit.point, newTargetPoint, Color.green);

                }

                break;
            // Lo haze el derecho
            default:
                newTargetPoint = rightHit.point + rightHit.normal * avoidDistance;
                if (debug)
                {
                    // Hit a la Izquierdo
                    Debug.DrawLine(miAgentePosition, rightHit.point, Color.red);
                    Debug.DrawLine(rightHit.point, newTargetPoint, Color.green);

                }
                break;
        }

        UseCustomDirectionAndRotation(newTargetPoint - miAgentePosition);

        steering = base.GetSteering(miAgente);


        return steering;
    }
}