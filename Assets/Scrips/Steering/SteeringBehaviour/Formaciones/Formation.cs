using System;
using System.Collections.Generic;
using System.Linq;

public class Formation
{
    private bool deletingFormation;
    private Dictionary<AgentNPC, bool> formationDone;
    public bool formationReady;
    public AgentNPC leader;
    public Dictionary<AgentNPC, Steering> soldiers;

    public Formation(AgentNPC leader)
    {
        this.leader = leader;
        soldiers = new Dictionary<AgentNPC, Steering>();
        formationDone = new Dictionary<AgentNPC, bool>();
    }

    public void MakeFormation()
    {
        leader.BecameLeader(this);
        foreach (var agent in soldiers.Keys)
        {
            agent.BecameSoldier(this);
            formationDone.Add(agent, false);
        }


        formationReady = true;
    }

    public void DissolveFormation()
    {
        if (deletingFormation) return;
        // Yo soy quien borra la formacion
        formationReady = false;
        deletingFormation = true;

        // C# Funcioan con Recogedor de basura
        // Es decir que si no hay puntero apuntando a esta clase se borra
        leader.ResetStateAction();

        foreach (var agent in soldiers.Keys) agent.ResetStateAction();
    }

    public Steering GetGlobalPosition(AgentNPC agente)
    {
        var relativePosition = soldiers[agente];
        relativePosition.lineal = leader.transform.TransformPoint(relativePosition.lineal);
        return relativePosition;
    }

    private FormationRank GetRank(AgentNPC agente)
    {
        if (agente == leader) return FormationRank.Leader;
        if (soldiers.Keys.Contains(agente)) return FormationRank.Soldier;
        throw new Exception("No hay ese agente en mi rango");
    }


    public bool ImLeader(AgentNPC agentNpc)
    {
        if (agentNpc == null) return false;
        return GetRank(agentNpc) == FormationRank.Leader;
    }

    public bool ImSoldier(AgentNPC agentNpc)
    {
        if (agentNpc == null) return false;
        return GetRank(agentNpc) == FormationRank.Soldier;
    }

    // Cuando todos estén en posición le decimos al líder que se espere para moverse.
    public void ImInPosition(AgentNPC agentNpc)
    {
        formationDone[agentNpc] = true;

        if (IsFormationDone())
        {
            leader.AllInPositionWaitABitMore();
        }
    }

    // El soldado no esta en posición

    public void ImNotInPosition(AgentNPC agentNpc) =>formationDone[agentNpc] = false;


    // Los solados ya no están en posición porque el líder se ha movido
    public void WaitForSoldiers()
    {
        foreach (var key in formationDone.Keys.ToList())
            formationDone[key] = false;

        leader.MoveABitBeforeWaiting(); // Le decimos al líder que espere a los soldados en un futuro
    }

    public bool IsFormationDone() => !formationDone.Values.Contains(false);

    public List<AgentNPC> GetRest(AgentNPC agentNpc)
    {
        // Se supone que esta en la formacion

        if (ImLeader(agentNpc)) return soldiers.Keys.ToList();
        var resto = new List<AgentNPC> {leader};
        resto.AddRange(soldiers.Keys.Where(npc => npc != agentNpc));

        return resto;
    }
}