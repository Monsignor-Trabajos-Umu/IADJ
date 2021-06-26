using System.Collections;
using UniBT;
using UniBT.Examples.Scripts;
using UnityEngine;

namespace Assets.Scrips.Actions
{
    public class AttackMode: Conditional
    {

        private AgentNPC agente;

        protected override void OnAwake()
        {
            agente = gameObject.GetComponent<AgentNPC>();
            Debug.Log($"Mi base {agente.name}");
        }

        protected override bool IsUpdatable() => agente != null && agente.IsAttacking();
    }
}