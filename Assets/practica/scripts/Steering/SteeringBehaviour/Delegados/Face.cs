using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Face : Aling
{
    // Quiero que mi agente mire hacia mi target
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.preditedRotation = (float)miAgente.MinAngleToRotate(target.gameObject);
        this.usePredicted = true;
        return base.GetSteering(miAgente);

    }

}
