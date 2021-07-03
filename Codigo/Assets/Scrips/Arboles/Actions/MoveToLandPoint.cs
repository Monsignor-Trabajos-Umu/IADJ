using System;
using System.Linq;
using UniBT;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scrips.Actions
{

    //Busca un punto de interes priorizando los que tienen el control enemigo
    public class MoveToLandPoint: UniBT.Action
    {
        private AgentNpc agente;


        public override void Awake() => agente = gameObject.GetComponent<AgentNpc>();


        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.GoingToLandPoint) return Status.Running;

    

             var objetivo = agente.landPoint;
            
            agente.GoToEpicPoint(objetivo);
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}
