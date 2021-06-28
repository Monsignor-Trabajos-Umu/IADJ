using System.Collections.Generic;
using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{

    //Se mueve hacia la base enemiga
    public class Patrullar : UniBT.Action
    {
        private AgentNpc agente;
        private int index;

        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
            index = 0;
        }

        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.GoingToLandPoint) return Status.Running;

            var target = agente.puntosPatrulla[index];
            Debug.Log($"{agente.name} Patrullando");
            agente.GoToEpicPoint(target);
            Debug.Log($"{agente.name} Llego al punto de patrulla");
            index = (index + 1)% agente.puntosPatrulla.Length;
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}
