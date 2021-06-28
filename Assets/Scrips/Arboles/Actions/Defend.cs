using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{

    //Se mueve hacia la base aliada
    public class Defend : UniBT.Action
    {


        [SerializeField] private AgentNpc agente;
        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.Defend) return Status.Running;

            //if (agente.state != State.Normal || agente.cAction != CAction.None) return Status.Failure;

            Debug.Log($"{agente.name} Defiendo mi base");
            agente.Defend();

            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}