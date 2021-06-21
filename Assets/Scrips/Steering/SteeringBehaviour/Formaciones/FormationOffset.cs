using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationOffset : SteeringBehaviour
{
    
    private Formation formation;
    private Align align;
    private Arrive arrive;
    public override Steering GetSteering(AgentNPC miAgente)
    {

        this.steering = new Steering(0, new Vector3(0, 0, 0));
        if (!formation.formationReady) return steering;

        Vector3 myPosition = miAgente.transform.position;
        var leader = formation.leader;
        var newOffset = formation.GetGlobalPosition(miAgente);
        var newDirection = newOffset.lineal - myPosition;

        
        var rotation = leader.orientacion - miAgente.orientacion + newOffset.angular;
        // Hago que este entre -180 y 180
        rotation = align.MapToRange(rotation);

        align.UsePredicted(rotation);
        steering.angular = align.GetSteering(miAgente).angular;

        // Arrive ya se encarga de parrar si estamos lo suficientemente cerca
        arrive.UsePredicted(newDirection); 
        steering.lineal = arrive.GetSteering(miAgente).lineal;

        // Si ya he llegado a mi sitio se lo hago saver a la formacion
        if (steering.lineal.Equals(new Vector3(0, 0, 0)))
        {
            formation.ImInPosition(miAgente);
        }

        return this.steering;
    }

    void Start()
    {
        align = gameObject.AddComponent<Align>();
        arrive = gameObject.AddComponent<Arrive>();
    }

    public void SetFormation(Formation formation)
    {
        this.formation = formation;
    }


}
