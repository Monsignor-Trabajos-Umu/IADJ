using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationOffset : SteeringBehaviour
{
    [SerializeField]
    private Formation formation;
    private Align align;
    private Arrive arrive;
    public override Steering GetSteering(AgentNpc miAgente)
    {

        this.steering = new Steering(0, new Vector3(0, 0, 0));
        if (formation == null || !formation.formationReady)
        {
            return steering;
        }

        var myPosition = miAgente.transform.position;
        var leader = formation.leader;
        var newOffset = formation.GetGlobalPosition(miAgente);
        var newDirection = newOffset.lineal - myPosition;

        
        var rotation = leader.orientacion - miAgente.orientacion + newOffset.angular;
        // Hago que este entre -180 y 180
        rotation = align.MapToRange(rotation);

        align.UseCustomRotation(rotation);
        steering.angular = align.GetSteering(miAgente).angular;

        // Arrive ya se encarga de parrar si estamos lo suficientemente cerca
        arrive.UseCustomDirectionAndRotation(newDirection); 
        steering.lineal = arrive.GetSteering(miAgente).lineal;

        // Si ya he llegado a mi sitio se lo hago saver a la formacion
        if (steering.lineal.Equals(new Vector3(0, 0, 0)))
        {
            //Debug.Log($"{miAgente.name} esta en posicion");
            formation.ImInPosition(miAgente);
        }
        else
        {
            formation.ImNotInPosition(miAgente);
            //Debug.Log($"{miAgente.name} NO esta en posicion");
        }

        return this.steering;
    }

    void Start()
    {
        align = gameObject.AddComponent<Align>();
        arrive = gameObject.AddComponent<Arrive>();
        steeringGroup = SteeringGroup.Formation;

    }

    public void SetFormation(Formation newFormation) 
    { 
        formation = newFormation;
    }


}
