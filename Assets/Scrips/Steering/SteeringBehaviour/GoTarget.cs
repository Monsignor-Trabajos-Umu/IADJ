using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoTarget : SteeringBehaviour
{

    public Vector3 targetPosition;
    public bool targetExists =false;
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        if (!targetExists)
            return this.steering;

        Vector3 myPosition = miAgente.transform.position;

        float distancia = Vector3.Distance(myPosition, targetPosition);
        // Comprobamos si estamos en la posicion +-
        if (distancia > miAgente.RExterior)
        {
            this.steering.velocidad = Vector3.ClampMagnitude(targetPosition - myPosition,
                miAgente.mVelocity);
        }
        else
        {
            // Si estamos en la posicion +- ponemos a false el flag
            targetExists = false;
            miAgente.ArrivedToTarget();

        }
        double angle = miAgente.MinAngleToRotate(targetPosition);
        if (Math.Abs(angle) >= Math.Abs(miAgente.AExterior))
        {
            this.steering.rotacion = (float)angle;
        }
        return base.steering;
    }



    public void NewTarget(Vector3 newTarget)
    {
        this.targetPosition = newTarget;
        this.targetExists = true;
        Debug.Log($"Going to Target {newTarget}");
    }
}
