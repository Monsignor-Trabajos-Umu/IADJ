using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Face : Align
{
    // Quiero que mi agente mire hacia mi target
    public override Steering GetSteering(AgentNpc miAgente)
    {
        this.customRotation = (float)miAgente.MinAngleToRotate(target.gameObject);
        this.useCustom = true;
        return base.GetSteering(miAgente);

    }

}
