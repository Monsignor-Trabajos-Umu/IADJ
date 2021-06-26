using System.Collections;
using UniBT;
using UniBT.Examples.Scripts;
using UnityEngine;

//Comprueba si el agenteNPC se encuentra por debajo del 50% de su vida máxima
public class IsInjured : Conditional
{
    private AgentNpc agente;

    protected override void OnAwake()
    {
        agente = gameObject.GetComponent<AgentNpc>();
        Debug.Log($"Mi base {agente.name}");
    }

    protected override bool IsUpdatable() => agente != null && agente.IsInjured();
}
