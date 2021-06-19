using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FormationState
{
    Waiting,
    Selected,
    MakingFormation,
}
public class Formation
{
    public AgentNPC leader;
    public FormationState state;
    public Dictionary<AgentNPC,Steering> soldier;

    public Formation(AgentNPC leader)
    {
        this.leader = leader;
        this.soldier = new Dictionary<AgentNPC, Steering>();
    }

    public void MakeFormation()
    {
        state = FormationState.MakingFormation;
    }

    public Steering GetGlobalPosition(AgentNPC agente)
    {
        var relativePosition = soldier[agente];
         relativePosition.lineal = leader.transform.TransformPoint(relativePosition.lineal);
         return relativePosition;
    }
}
