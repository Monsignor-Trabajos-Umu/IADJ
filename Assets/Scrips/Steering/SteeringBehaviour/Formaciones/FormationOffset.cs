using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationOffset : SteeringBehaviour
{
    
    private Formation formation;
    private bool makeFormation;
    private Align align;
    private Arrive arrive;
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        if (!makeFormation)
            return this.steering;

        Vector3 myPosition = miAgente.transform.position;
        var leader = formation.leader;
        var offSet = formation.soldier[miAgente];
        var newPosition = leader.transform.position + offSet.lineal;

        arrive.UsePredicted(newPosition);
        steering.lineal = arrive.GetSteering(miAgente).lineal;



        var rotation = leader.orientacion - miAgente.orientacion + offSet.angular;
        // Hago que este entre -180 y 180
        rotation = align.MapToRange(rotation);

        align.UsePredicted(rotation);
        steering.angular = align.GetSteering(miAgente).angular;


        // Comprobamos si estamos en la posicion +-
        if (direction.magnitude > miAgente.rExterior)
        {
            base.usePredicted = true;
            base.predictedDirection = direction;
            return base.GetSteering(miAgente);
        }
        else
        {
            // Si estamos en la posicion +- ponemos a false el flag
            makeFormation = false;
            miAgente.ArrivedToTarget();

        }
        double angle = miAgente.MinAngleToRotate(targetPosition);
        if (Math.Abs(angle) >= Math.Abs(miAgente.aExterior))
        {
            this.steering.rotacion = (float)angle;
        }
        return base.steering;
    }

    void Start()
    {
        align = gameObject.AddComponent<Align>();
        arrive = gameObject.AddComponent<Arrive>();
    }

    public void NewTarget(Vector3 newTarget)
    {
        this.targetPosition = newTarget;
        this.makeFormation = true;
    }

}
