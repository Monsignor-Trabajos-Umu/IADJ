using System.Linq;
using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    public class ASteering : SteeringBehaviour
    {
        // Steering para movernos y girar
        [SerializeField] private Arrive arrive;
        [SerializeField] private Vector3 currentTarget;
        [SerializeField] private Vector3[] dPath;


        // [SerializeField] private int nodesCovered;
        [SerializeField] private Vector3 enemyBasePosition;
        [SerializeField] private Heuristic heuristic;
        [SerializeField] private LookWhereYouGoing lookWhereYouGoing;


        // Valores para el GetSteering
        [SerializeField] private bool moving;
        [SerializeField] private int nodesToCover;


        // Debug can be deleted
        [SerializeField] private Agent origen;

        //Lista con los puntos donde ir
        [SerializeField] private Vector3[] path;

        // Area para saber si hemos llegado\
        [SerializeField] private double radioTarget;
        [SerializeField] private Seek seek;


        //Debug
        //[SerializeField] protected bool debug;
        [SerializeField] private int targetIndex;
        [SerializeField] private Agent targetV;

        // Use this for initialization
        private void Start()
        {
            arrive = gameObject.AddComponent<Arrive>();
            seek = gameObject.AddComponent<Seek>();
            lookWhereYouGoing = gameObject.AddComponent<LookWhereYouGoing>();
            StartMoving(nodesToCover, origen.transform.position,
                targetV.transform.position, 40, heuristic);
        }

        public override global::Steering GetSteering(AgentNpc agent)
        {
            steering = new global::Steering(0, new Vector3(0, 0, 0));
            if (!moving) return steering;
            //Vemos si nos nos quedan nodos o que estamos ya al lado
            if (targetIndex == path.Length-1 ||
                Vector3.Distance(agent.transform.position, path[path.Length-1]) <
                radioTarget/2)
            {
                Debug.Log($"{agent.name} ha llegado al objetivo");
                moving = false;
                return steering;
            }

            //Nos quedan nodos vemos ahora cual es nuestro objetivo

            var currentPosition = agent.transform.position;
            var radio = agent.rInterior;

            var targetPosition = path[targetIndex];


            // Si esta los suficientemente lejos nos acercamos
            if (Vector3.Distance(agent.transform.position, targetPosition) >= radio)
            {
                // Estamos en el penultimo vamos a hacer un Arrive al ultimo
                if (targetIndex + 1 == path.Length)
                {
                    arrive.UseCustomDirectionAndRotation(targetPosition -
                                                         currentPosition);
                    steering = arrive.GetSteering(agent);
                }
                else
                {
                    // Si no hacemos un seek
                    seek.UseCustomDirectionAndRotation(targetPosition - currentPosition);
                    steering = seek.GetSteering(agent);
                }
            }

            // Si estamos lo suficientemente cerca vamos al siguiente nodo 
            if (Vector3.Distance(currentPosition, targetPosition) <= radio)
                targetIndex++;

            steering.angular = lookWhereYouGoing.GetSteering(agent).angular;
            return steering;
        }

        public void StartMoving(int _nodes, Vector3 origen, Vector3 target,
            double _radioTarget,
            Heuristic heuristic)
        {
            radioTarget = _radioTarget;
            nodesToCover = _nodes;
            PathRequestManagerA.RequestPath(origen, target, heuristic, OnPathFound);
        }

        private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (!pathSuccessful) return;

            dPath = newPath;
            path = newPath.Take(nodesToCover).ToArray();
            moving = true;
            targetIndex = 0;
        }


        protected override void OnDrawGizmos()
        {
            if (!debug || dPath == null) return;
            for (var i = targetIndex; i < dPath.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(dPath[i], Vector3.one);

                if (i == targetIndex)
                    Gizmos.DrawLine(transform.position, dPath[i]);
                else
                    Gizmos.DrawLine(dPath[i - 1], dPath[i]);
            }
        }
    }
}