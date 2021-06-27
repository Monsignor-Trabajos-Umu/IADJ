using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{

    //Se mueve hacia la base enemiga
    public class AvanzoBase : UniBT.Action
    {
       

        [SerializeField] private AgentNpc agente;

        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {
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