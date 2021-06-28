using System.Collections;
using UnityEngine;

namespace Assets.Scrips.Arboles.Conditional
{
    public class IsFarFromLandPoint: UniBT.Conditional
    {

        private AgentNpc agente;

        protected override void OnAwake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
            Debug.Log($"Mi IsFarFromLandPoint {agente.name}");
        }

        protected override bool IsUpdatable() => agente != null && agente.CanGoToLandPoint();
    }
}