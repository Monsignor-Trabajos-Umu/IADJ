using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Face : Align
{
    // Quiero que mi agente mire hacia mi target
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.predictedRotation = (float)miAgente.MinAngleToRotate(target.gameObject);
        this.usePredicted = true;
        return base.GetSteering(miAgente);

    }

}
