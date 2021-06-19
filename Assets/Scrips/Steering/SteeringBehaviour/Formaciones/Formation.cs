using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation
{
    public AgentNPC leader;
    public Dictionary<AgentNPC,Steering> soldier;

    public Formation(AgentNPC leader)
    {
        this.leader = leader;
        this.soldier = new Dictionary<AgentNPC, Steering>();
    }
}
