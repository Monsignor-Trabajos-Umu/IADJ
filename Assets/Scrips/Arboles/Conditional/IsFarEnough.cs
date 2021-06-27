using System.Collections;
using UnityEngine;

namespace Assets.Scrips.Arboles.Conditional
{
    public class IsFarEnough: UniBT.Conditional
    {

        private AgentNpc agente;

        protected override void OnAwake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
            Debug.Log($"Mi base {agente.name}");
        }

        protected override bool IsUpdatable() => agente != null && agente.CanGoToBase();
    }
}