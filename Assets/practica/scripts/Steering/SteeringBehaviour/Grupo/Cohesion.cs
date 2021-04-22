using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Cohesion : Seek
{

    [SerializeField]
    List<Agent> targets;
    [SerializeField]
    float threshold;
    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steering = new Steering(0, new Vector3(0, 0, 0));
        var count = 0;
        Vector3 centerOfMas = new Vector3(0, 0, 0);
        foreach (Agent target in targets)
        {
            Vector3 direction = target.transform.position - agent.transform.position;
            float distance = direction.magnitude;
            if (distance > threshold)
            {
                centerOfMas += target.transform.position;
                count++;
            }
        }
        // Convertimos velocidad en aceleracion | limitamos la aceleracion


        if (count > 0)
        {
            centerOfMas /= count;
            usePredicted = true;
            predictedPosition = centerOfMas;
            return base.GetSteering(agent);
        }


        return steering;
    }


    private void Start()
    {
        this.targets = GameObject.FindGameObjectsWithTag("flock")
                        .Select(target => target.GetComponent<Agent>())
                        .ToList(); ;
    }
}

