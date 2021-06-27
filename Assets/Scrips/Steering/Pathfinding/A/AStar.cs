using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    public class AStar : MonoBehaviour
    {
        //Si soy las casillas negativas en mi mapa de influencia
        [SerializeField] private bool bandoPositivo;
        [SerializeField] private readonly bool debug = false;

        // Path en si
        [SerializeField] private FastGrid grid; //Lleva los costes generales


        [SerializeField] private GameObject gridChungoPrefab;
        [SerializeField] private Heuristic heuristic;
        [SerializeField] private InfluenceMapControl influeceMap;
        public Transform seeker, target;


        // Tenemos que esperar que el GridChungo se incialize
        private void Start()
        {
            var agenteNpc = gameObject.GetComponent<AgentNpc>();

            // Clonamos el GridChungo base
            var parent = GameObject.Find("Inst").transform;

            grid = Instantiate(gridChungoPrefab, parent).GetComponent<FastGrid>();

            grid.name = $"GridMap - {agenteNpc.name}";

            influeceMap = GameObject.Find("InfluenceController")
                .GetComponent<InfluenceMapControl>();


            heuristic = agenteNpc.GetHeuristic();


            bandoPositivo = gameObject.GetComponent<Propagador>().Positive();
        }


        private void Update()
        {
            var current = transform.position;
            var ta = target.position;
            GetPath(current, ta);
        }

        public Queue<Node> GetPath(Vector3 startPos, Vector3 targetPos)
        {
            // Calculamos el path
            FindPath(startPos, targetPos);


            return new Queue<Node>(grid.path);
        }

        public Queue<Node> GetPath(Vector3 startPos, Vector3 targetPos, int nodos)
        {
            // Calculamos el path
            FindPath(startPos, targetPos);

            var partedList = grid.path.Count < nodos
                ? grid.path.GetRange(0, nodos)
                : grid.path;

            return new Queue<Node>(partedList);
        }

        private void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            var startNode = grid.GetNodeFromWorldPoint(startPos);
            var targetNode = grid.GetNodeFromWorldPoint(targetPos);

            var openSet = new Heap<NodeHeaped>(grid.MaxSize);
            var closedSet = new HashSet<NodeHeaped>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (var neighbor in grid.GetNeighbors(currentNode))
                {
                    if (neighbor.pared || closedSet.Contains(neighbor)) continue;

                    var newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                    // Si es negativo ponemos 0
                    newCostToNeighbor = (newCostToNeighbor < 0) ? 0 : newCostToNeighbor;

                    if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
        }

        private void RetracePath(NodeHeaped startNode, NodeHeaped endNode)
        {
            var path = new List<NodeHeaped>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = (NodeHeaped) currentNode.parent;
            }

            path.Reverse();

            grid.path = path;
        }

        // 1 aliado
        // 0 nada
        // -1 enemigo
        private int AliadoNadaEnemigo(Node node)
        {
            var value = influeceMap.GetInfluence(node.gridX, node.gridZ);

            if (value == 0) return 0;
            return bandoPositivo switch
            {
                // Bando positivo
                true when value > 0 => 1, //Mismo bando
                true when value < 0 => -1, //Bando enemigo
                // Bando negativo
                false when value > 0 => -1, //Mismo bando
                false when value < 0 => 1, //Bando enemigo
                _ => 0
            };
        }


        private float GetDistance(Node nodeA, Node nodeB)
        {
            var h = heuristic.GetH(nodeA, nodeB);

            // Si el nodo es alidado restamos uno
            // 1 aliado

            // -1 enemigo
            // h(n) debe ser menor que h*(n) 
            // Es decir la heuristic no puede sobrepasar a la real
            var extra = AliadoNadaEnemigo(nodeB) switch
            {
                -1 => 1, // Enemigo coste se mantiene
                0 => 0.9, // No hay nada es preferible ir por aqui
                1 => 0.7, // Aliado muy preferible ir por aqui

                _ => throw new ArgumentOutOfRangeException()
            };

            if (debug) Debug.Log(h);
            return (float) (extra * h);
        }
    }
}