using UniBT;
using UnityEngine;
namespace Assets.Scrips.Actions
{

    //Se mueve hacia el punto de interes más cercano no conquistado.
    //Si todos están conquistados, se mueve hacia la base enemiga.
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

            var puntos = GameObject.FindGameObjectsWithTag("puntoInteres");
            GameObject target = null;
            foreach (var p in puntos)
            {
                int valor = agente.controlador.getInfluencia(p.transform.position);
                if ((valor <= 0 && agente.tag == "equipoAzul") || (valor >= 0 && agente.tag == "equipoRojo"))
                {
                    target = p;
                    break;
                }
            }
            //Si no hay puntos que conquistar no hacemos nada
            if (target == null)
            {
                Debug.LogWarning("Todo conquistado");
                return Status.Failure;
            }
            agente.GoTo(target);
            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}
