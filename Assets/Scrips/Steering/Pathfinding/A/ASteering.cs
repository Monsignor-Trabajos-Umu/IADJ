using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    public class ASteering : SteeringBehaviour
    {

        // A para calcular
        [SerializeField] private AStar aStar;

        // Steering para movernos y girar
        [SerializeField] private Arrive arrive;
        [SerializeField] private Seek seek;
        [SerializeField] private LookWhereYouGoing lookWhereYouGoing;


        // Valores para el GetSteering
        [SerializeField] private bool moving=false;
        [SerializeField] private int nodesToCover;
       // [SerializeField] private int nodesCovered;
        [SerializeField] private Vector3 enemyBasePosition;
        
        [SerializeField] private Queue<Node> nodes; // Cola con los nodos 
        [SerializeField] private Node currentTarget;

        // Use this for initialization
        void Start()
        {
            arrive = gameObject.AddComponent<Arrive>();
            seek = gameObject.AddComponent<Seek>();
            aStar = gameObject.GetComponent<AStar>();

        }

        public override global::Steering GetSteering(AgentNpc agent)
        {
            steering = new global::Steering(0, new Vector3(0, 0, 0));

            if (!moving) return steering;


            // Nos estamos moviendo
            // Queremos ir hacia la base enemiga y movernos x casillas
            //Optimo no es
            nodes = aStar.GetPath(agent.transform.position, enemyBasePosition,nodesToCover);
            var target = nodes.Dequeue();

            if (nodes.Count == 0)
            {
                // Es el ultimo
                arrive.UseCustomDirectionAndRotation(target.worldPosition);

            }

            return steering;

        }



        public void StartMoving(int nodes,Vector3 targetPosition)
        {
            moving = true;
            this.nodesToCover = nodes;
            //nodesCovered = 0;
            this.enemyBasePosition = targetPosition;


        }
    }
}