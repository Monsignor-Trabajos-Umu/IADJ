using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PrioritySteering : MonoBehaviour
{
    List<SteeringBehaviour> listSteerings;
    List<BehaviorAndWeight> behaviors;

    double epsilon = 0.05;
    AgentNPC agente;
    [SerializeField]
    Steering debugSteering;
    [SerializeField]
    bool debugGreen = false;

    private void Awake()
    {
        //Lista de steerings
        listSteerings = new List<SteeringBehaviour>();
        //Lista de steerings con sus prioridades
        behaviors = new List<BehaviorAndWeight>();
        agente = GetComponent<AgentNPC>();
        //usar GetComponents<>() para cargar los SteeringBehavior del personaje
        listSteerings = GetComponents<SteeringBehaviour>().ToList();
        foreach (SteeringBehaviour str in listSteerings)
        {
            str.enabled = true;
        }
    }

    private void LateUpdate()
    {

        //Recorre la lista construida en Awake() y calcula los Steering de los SteeringBehaviour
        behaviors.Clear();
        foreach (SteeringBehaviour str in listSteerings)
        {
            if (str.enabled)
            {
                Steering temp = str.GetSteering(agente);
                behaviors.Add(new BehaviorAndWeight(temp, str.weight));

            }
        }


    }



    public Steering GetSteering()
    {
        Steering steering = new Steering(0, new Vector3(0, 0, 0));

        foreach (BehaviorAndWeight behavior in behaviors)
        {
            steering.angular = behavior.behavior.angular;
            steering.lineal = behavior.behavior.lineal;

            if (debugGreen)
            {
                Debug.DrawRay(agente.transform.position, steering.lineal, Color.green);
                this.debugSteering = steering;
            }
            if (steering.lineal.magnitude > epsilon || Math.Abs(steering.angular) > epsilon)
            {
                return steering;
            }

        }

        return new Steering(0, new Vector3(0, 0, 0));
    }

}
