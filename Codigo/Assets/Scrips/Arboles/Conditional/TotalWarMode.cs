using UniBT;
using UnityEngine;

public class TotalWarMode : Conditional
{

    private AgentNpc agente;

    protected override void OnAwake()
    {
        agente = gameObject.GetComponent<AgentNpc>();
        Debug.Log($"Mi base {agente.name}");
    }

    protected override bool IsUpdatable() => agente != null && agente.IsTotalWar();
}
