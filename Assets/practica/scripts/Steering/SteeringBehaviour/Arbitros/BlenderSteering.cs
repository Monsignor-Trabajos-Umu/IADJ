using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[System.Serializable]
public struct BehaviorAndWeight
{
    public Steering behavior;
    public float weight;

    public BehaviorAndWeight(Steering behavior, float weight)
    {
        this.behavior = behavior;
        this.weight = weight;
    }
}

public class BlenderSteering : MonoBehaviour
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
        listSteerings = GetComponents<SteeringBehaviour>().ToList<SteeringBehaviour>();
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

        // Full Aceleration Seek a tope

        foreach (BehaviorAndWeight behavior in behaviors)
        {
            steering.angular += behavior.weight * behavior.behavior.angular;
            steering.lineal += behavior.weight * behavior.behavior.lineal;
        }
        steering.lineal = (steering.lineal.magnitude > agente.mAceleracion) ? steering.lineal.normalized * agente.mAceleracion : steering.lineal;
        steering.angular = (steering.angular > agente.mAngularAceleracion) ? (steering.angular * agente.mAngularAceleracion) / Math.Abs(steering.angular) : steering.angular;


        //Debug.DrawRay(agente.transform.position, steering.lineal, Color.white);
        this.debugSteering = steering;
        return steering;
    }






}
