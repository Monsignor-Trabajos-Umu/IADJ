using System.Collections.Generic;
using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{

    //Se mueve hacia la base enemiga
    public class Patrullar : UniBT.Action
    {
        private AgentNpc agente;
        public GameObject[] points;
        public int index;

        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
            points = GameObject.FindGameObjectsWithTag("Patrulla");
        }

        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.GoToTarget) return Status.Running;

            if (agente.state != State.Waiting || agente.cAction != CAction.None) return Status.Failure;

            
            if (points.Length <= 0)
                return Status.Failure;
            var p = points[index];
            Debug.Log($"{agente.name} Patrullando");
            agente.Patrullar(p.transform);
            index = (index + 1) % points.Length;
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}
