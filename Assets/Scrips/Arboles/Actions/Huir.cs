using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{

    //Se mueve hacia la base enemiga
    public class Huir : UniBT.Action
    {


       private GameObject[] fuentes;
       private AgentNpc agente;

        public override void Awake()
        {
            fuentes = GameObject.FindGameObjectsWithTag("puntoCurativo");
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.Retreat) return Status.Running;

            if (agente.state != State.Waiting || agente.cAction != CAction.None) return Status.Failure;


            if (agente.AlreadyHealing()) return Status.Success;


            GameObject target = null;
            float aux = Mathf.Infinity;
            foreach(var p in fuentes)
            {
                var distance = Vector3.Distance(p.transform.position, gameObject.transform.position);
                if (aux > distance)
                {
                    aux = distance;
                    target = p;
                }
            }

            if(target == null)
            {
                Debug.Log($"{agente.name} No se encontró ninguna fuente");
                return Status.Failure;
            }



            if (Vector3.Distance(target.transform.position, agente.transform.position) <= agente.RExterior)
            {
                Debug.Log($"{agente.name} Estamos al lado de la fuente");
                return Status.Success;
            }
            Debug.Log($"{agente.name} Huyendo a la fuente más cercana");
            agente.Huir(target);

            return Status.Success;
        }

        // abort when the parent conditional changed on previous status is running.
        public override void Abort()
        {
            agente.ResetStateAction();
        }
    }
}