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

            if (agente.state == State.Action && agente.cAction == CAction.GoToTarget) return Status.Running;

            if (agente.state != State.Normal || agente.cAction != CAction.None)
            {
                Debug.Log($"{agente.state} Fallo");
                return Status.Failure;
            }

            Debug.Log($"{agente.name} Patrullando");
            agente.Patrullar(index);
            Debug.Log($"{agente.name} Llego al punto de patrulla");
            index += 1;
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}
