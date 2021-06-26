using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FollowPathPredictive : Arrive
{

    [Range(0.0f, 10.0f)]
    public float radio;
    public int currentNode = 0;
    public double predictTime = 0.1;
    // Se presupone que target tiene un path
    public override Steering GetSteering(AgentNpc agent)
    {

        Patheable patheable = target.GetComponent<Patheable>() as Patheable;
        if (patheable == null)
        {
            Debug.LogError("El target no es patheable");
            return steering;
        }
        var futurePos = agent.transform.position + agent.vVelocidad * (float) predictTime;
        List<Vector3> nodes = patheable.path.nodes;
        // Estamos en el ultimo nodo
        if (currentNode + 1 > nodes.Count())
            return steering;
        Vector3 targetPosition = nodes[currentNode];

        // Si esta los suficientemente lejos nos acercamos
        if (Vector3.Distance(futurePos, targetPosition) >= radio)
        {
            // Estamos en el penultimo vamos a hacer un Arrive al ultimo
            if (currentNode + 1 == nodes.Count())
            {
                this.useCustom = true;
                this.customDirection = targetPosition - agent.transform.position;
                steering = base.GetSteering(agent);
            }
            else
            {
                // Si no hacemos un seek
                steering.lineal = targetPosition - agent.transform.position;
                steering.lineal.Normalize();
                steering.lineal *= agent.mAcceleration;

            }
        }
        // Si estamos lo suficientemente cerca vamos al siguiente nodo 
        if (Vector3.Distance(futurePos, targetPosition) <= radio)
            currentNode++;

        return steering;
    }
    private void Start()
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));
    }
}
