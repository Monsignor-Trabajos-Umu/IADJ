using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoTarget : SteeringBehaviour
{
    Vector3 targetPosition;
    Boolean targetExists;
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        if (!targetExists)
            return this.steering;


        Vector3 myPosition = miAgente.transform.position;

        float distancia = Vector3.Distance(myPosition, targetPosition);
        // Comprobamos si estamos en la posicion +-
        if (distancia > miAgente.rExterior)
        {
            this.steering.velocidad = Vector3.ClampMagnitude(targetPosition - myPosition,
                miAgente.vMaxima);
        }
        else
        {
            // Si estamos en la posicion +- ponemos a false el flag
            targetExists = false;

        }
        double angle = miAgente.MinAngleToRotate(targetPosition);
        if (Math.Abs(angle) >= Math.Abs(miAgente.AExterior))
        {
            this.steering.angulo = (float)angle;
        }
        return this.steering;
    }

    public void NewTarget(Vector3 newTarget)
    {
        this.targetPosition = newTarget;
        this.targetExists = true;
    }
}
