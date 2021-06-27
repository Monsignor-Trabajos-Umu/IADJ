using UniBT;
using UnityEngine;

namespace Assets.Scrips.Actions
{

    //Se mueve hacia la base enemiga
    public class Huir : UniBT.Action
    {


        [SerializeField] private GameObject[] fuentes;
        [SerializeField] private AgentNpc agente;

        public override void Awake()
        {
            fuentes = GameObject.FindGameObjectsWithTag("puntoCurativo");
            agente = gameObject.GetComponent<AgentNpc>();
        }


        protected override Status OnUpdate()
        {

            if (agente.state == State.Action && agente.cAction == CAction.Retreat) return Status.Running;

            if (agente.state != State.Normal || agente.cAction != CAction.None) return Status.Failure;

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