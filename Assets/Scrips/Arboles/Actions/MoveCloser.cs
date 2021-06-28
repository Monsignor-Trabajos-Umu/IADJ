using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{

    //Se mueve hacia la base enemiga
    public class MoveCloser : UniBT.Action
    {
       

        private AgentNpc agente;

        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {
            
            if (agente.state == State.Action && agente.cAction == CAction.GoingToEnemy) return Status.Running;

            //if (agente.state != State.Waiting || agente.cAction != CAction.None) return Status.Failure;

            Debug.Log($"{agente.name} Yendo a la base enemiga");
            agente.GoToEnemyBase(); 
           
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
           agente.ResetStateAction();
        }
    }
}