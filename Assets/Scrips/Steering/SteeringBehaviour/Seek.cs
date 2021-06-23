using UnityEngine;

public class Seek : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {
        steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration Seek a tope


        steering.lineal = useCustom
            ? customDirection
            : target.transform.position - miAgente.transform.position;
        steering.lineal = RemoveY(steering.lineal); // Filtramos la y
        steering.lineal.Normalize();
        steering.lineal *= miAgente.mAcceleration;

        steering.angular = 0;
        return steering;
    }

    // Start is called before the first frame update
    private void Start()
    {
        steeringGroup = SteeringGroup.Pursuit;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}