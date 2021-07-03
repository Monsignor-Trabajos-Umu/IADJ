using System.Collections;
using UniBT;
using UniBT.Examples.Scripts;
using UnityEngine;

//Comprueba si el agenteNPC se encuentra por debajo del 50% de su vida máxima
namespace Assets.Scrips.Arboles.Conditional
{
    public class IsNotDead : UniBT.Conditional
    {
        private AgentNpc agente;

        protected override void OnAwake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }

        protected override bool IsUpdatable() => agente != null && agente.IsNotDead();
    }
}