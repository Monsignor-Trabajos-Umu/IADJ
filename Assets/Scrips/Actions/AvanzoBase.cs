using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{
    public class AvanzoBase : UniBT.Action
    {
       

        [SerializeField] private AgentNpc agente;

        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {
            
            //if (elapsedTime < waitTime) return Status.Running;
            //agente.

            //elapsedTime = 0.0f;
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
           
        }
    }
}