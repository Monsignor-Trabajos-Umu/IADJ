using System.Linq;
using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    public class ASteering : SteeringBehaviour
    {
        // Steering para movernos y girar
        [SerializeField] private Arrive arrive;
        [SerializeField] private Vector3 currentTarget;


        // [SerializeField] private int nodesCovered;
        [SerializeField] private Vector3 enemyBasePosition;
        [SerializeField] private Heuristic heuristic;
        [SerializeField] private LookWhereYouGoing lookWhereYouGoing;


        // Valores para el GetSteering
        [SerializeField] public bool moving;
        [SerializeField] private int nodesToCover;


        // Debug can be deleted
        [SerializeField] private Agent origen;

        //Lista con los puntos donde ir
        [SerializeField] private Vector3[] path;
        [SerializeField] private int targetIndex;

        // Area para saber si hemos llegado
        [SerializeField] private double radioTarget;
        [SerializeField] private Seek seek;


        // Use this for initialization
        private void Start()
        {
            arrive = gameObject.AddComponent<Arrive>();
            seek = gameObject.AddComponent<Seek>();
            lookWhereYouGoing = gameObject.AddComponent<LookWhereYouGoing>();
        }

        public override global::Steering GetSteering(AgentNpc agent)
        {
            steering = new global::Steering(0, new Vector3(0, 0, 0));
            if (!moving) return steering;
            //Vemos si nos nos quedan nodos o que estamos ya al lado de la base
            if (targetIndex == path.Length || agent.NearBase())
            {
                Debug.Log($"{agent.name} ha llegado al objetivo");
                moving = false;

                agent.GoToEnemyBaseEnded();
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

        public void CancelPath()
        {
            Debug.Log($"Cancelando path");

            if(!moving) return;
            moving = false;
            path = null;
            targetIndex = 0;
        }

        private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            Debug.Log($"Path Calculado pathSuccessful {pathSuccessful}");
            if (!pathSuccessful) return;

            path = newPath.Take(nodesToCover).ToArray();
            moving = true;
            targetIndex = 0;
        }


        protected override void OnDrawGizmos()
        {
            if (!debug || path == null || !moving) return;
            for (var i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                    Gizmos.DrawLine(transform.position, path[i]);
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }
}