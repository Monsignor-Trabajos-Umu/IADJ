using System.Linq;
using UniBT;
using UnityEngine;
namespace Assets.Scrips.Actions
{

    //"Busca" un enemigos al rededor si los hay los ataca
    public class AttackEnemy : UniBT.Action
    { 
        private AgentNpc agente;
        private Agent enemigo;
        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {
            if (agente.state == State.Action && agente.cAction == CAction.AttackEnemy)
            {
                Debug.Log($"Intento atacar {agente.state} - {agente.cAction}");

                agente.Atacar(enemigo);
                return Status.Success;
            }
            
            var enemigos = agente.enemigos;

            //Siempre hay un objetivo
            enemigo = enemigos.First();
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