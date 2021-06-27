using UniBT;
using UnityEngine;
namespace Assets.Scrips.Actions
{

    //Se mueve hacia el punto de interes m�s cercano no conquistado.
    //Si todos est�n conquistados, se mueve hacia la base enemiga.
    public class FindEnemy : UniBT.Action
    {
        [SerializeField] private AgentNpc agente;

        public override void Awake()
        {
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.GoingToEnemy) return Status.Running;

            if (agente.state != State.Normal || agente.cAction != CAction.None) return Status.Failure;

            Debug.Log($"{agente.name} Buscando Enemigos");
            agente.FindEnemy();

            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}
