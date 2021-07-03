using System.Collections;
using UniBT;
using UniBT.Examples.Scripts;
using UnityEngine;

namespace Assets.Scrips.Actions
{
    public class DefenseMode: Conditional
    {

        private AgentNpc agente;

        protected override void OnAwake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
            Debug.Log($"Mi base {agente.name}");
        }

        protected override bool IsUpdatable() => agente != null && agente.IsDefending();
    }
}