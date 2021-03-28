using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewTarget : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));

        Vector3 myPosition = miAgente.transform.position;
        Vector3 targetPosition = this.target.transform.position;

        float distancia = Vector3.Distance(myPosition, targetPosition);

        if (distancia > miAgente.rExterior)
            this.steering.velocidad = Vector3.ClampMagnitude(targetPosition - myPosition,
                miAgente.vMaxima);
        double angle = miAgente.MinAngleToRotate(target.gameObject);


        if (Math.Abs(angle) >= Math.Abs(miAgente.AExterior))
        {
            this.steering.angulo = (float)angle;
        }
        return this.steering;
    }

    
    public void newTarget(Vector3 posicion)
    {
        Vector3 myPosition = miAgente.transform.position;
        Vector3 targetPosition = this.target.transform.position;

        float distancia = Vector3.Distance(myPosition, targetPosition);

        if (distancia > miAgente.rExterior)
            this.steering.velocidad = Vector3.ClampMagnitude(targetPosition - myPosition,
                miAgente.vMaxima);
        double angle = miAgente.MinAngleToRotate(target.gameObject);


        if (Math.Abs(angle) >= Math.Abs(miAgente.AExterior))
        {
            this.steering.angulo = (float)angle;
        }
        return this.steering;
    }
}
