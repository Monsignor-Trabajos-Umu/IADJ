using System.Linq;
using UniBT;
using UnityEngine;
namespace Assets.Scrips.Actions
{

    //"Busca" un enemigos al rededor si los hay los ataca
    public class AttackEnemy : UniBT.Action
    { 
        private AgentNpc agente;

        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.AttackEnemy) return Status.Running;

            if (agente.state != State.Waiting || agente.cAction != CAction.None)  return Status.Failure;

            var enemigos = agente.enemigos;

            var enemigo = enemigos.First();

            //Siempre hay un objetivo

            agente.Atacar(enemigo);
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}