using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Alignment : SteeringBehaviour
{
    [SerializeField]
    List<Agent> targets;
    [SerializeField]
    float threshold;

    public override Steering GetSteering(AgentNPC agent)
    {
        var count = 0;
        Steering steering = new Steering(0, new Vector3(0, 0, 0));
        Vector3 velocidades = new Vector3(0, 0, 0);
        foreach (Agent target in targets)
        {
            Vector3 direction = target.transform.position - agent.transform.position;
            float distance = direction.magnitude;
            // Si estoy lo suficientemente cerca
            if (distance < threshold)
            {
                // Calcular aceleracion
                velocidades += target.vVelocidad;
                count++;

            }

        }
        if (count > 0)
        {
            // Calculamos la media
            velocidades /= count;
            // Steering Craig Reynolds
            velocidades = velocidades - agent.vVelocidad;

            // Normalizamos y la convertimos en aceleracion

            steering.lineal = velocidades.normalized;
            steering.lineal *= agent.mAceleracion;
        }

        if (debug)
            Debug.DrawRay(transform.position, steering.lineal, Color.blue);
        return steering;

    }

    private void Start()
    {
        this.targets = GameObject.FindGameObjectsWithTag("flock")
                        .Select(target => target.GetComponent<Agent>())
                        .ToList(); ;
        grupo = Grupo.SEPARACION;
    }
}
