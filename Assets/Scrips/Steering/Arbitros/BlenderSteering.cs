using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;



public class BlenderSteering : ArbitroSteering
{

    [SerializeField]
    List<SteeringBehaviour> listSteerings;
    List<BehaviorAndWeight> behaviors;
    AgentNPC agente;
    [SerializeField]
    Steering debugSteering;


    private void Awake()
    {
        listSteerings = new List<SteeringBehaviour>();
        behaviors = new List<BehaviorAndWeight>();
        agente = GetComponent<AgentNPC>();
        //usar GetComponents<>() para cargar los SteeringBehavior del personaje
        listSteerings = GetComponents<SteeringBehaviour>().Where(ste => ste.useReferee)
            .ToList();
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



    override public Steering GetSteering()
    {
        Steering steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration Seek a tope

        foreach (BehaviorAndWeight behavior in behaviors)
        {
            steering.angular += behavior.weight * behavior.behavior.angular;
            steering.lineal += behavior.weight * behavior.behavior.lineal;
        }


        steering = filtroYSteering(steering);

        steering.lineal = (steering.lineal.magnitude > agente.mAcceleration) ? steering.lineal.normalized * agente.mAcceleration : steering.lineal;
        steering.angular = (steering.angular > agente.mAngularAcceleration) ? (steering.angular * agente.mAngularAcceleration) / Math.Abs(steering.angular) : steering.angular;

        steering = RoundSteering(steering);
        this.debugSteering = steering;

        if (debugGreen)
        {
            Debug.DrawRay(agente.transform.position, steering.lineal, Color.green);
           
        }


        return steering;
    }


    private Steering filtroYSteering(Steering steering)
    {
        steering.lineal.y = 0;
        return steering;
    }


    private Steering RoundSteering(Steering steering)
    {
        Debug.Log($"PreFiltre {steering.lineal} {steering.angular}");
        // No tiene sentido 0,000001 de aceleracion
        steering.lineal.x  = (float)(Math.Round(steering.lineal.x, 6));
        steering.lineal.z  = (float)(Math.Round(steering.lineal.z, 6));

        // Para los angulos con dos decimales es mas que suficiente
        steering.angular  = (float)(Math.Round(steering.angular, 2));
        Debug.Log($"Post {steering.lineal} {steering.angular}");
        return steering;
    }



}
