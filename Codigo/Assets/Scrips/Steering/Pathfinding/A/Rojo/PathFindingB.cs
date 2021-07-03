using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    public class PathFindingB : MonoBehaviour
    {

        [SerializeField] private readonly bool debug = false;

        //Si soy las casillas negativas en mi mapa de influencia
        [SerializeField] private bool bandoPositivo;
        // Path en si
        [SerializeField] private FastGrid grid; //Lleva los costes generales me lo da el ASteering
        [SerializeField] private InfluenceMapControl influeceMap;

        // Speed up A
        [SerializeField] private PathRequestManagerB requestManagerB;


        [SerializeField] private AgentNpc agente;


        public void StartFindPath(Vector3 startPos, Vector3 targetPos,Heuristic heuristic,AgentNpc _agente)
        {
            this.agente = _agente;
            StartCoroutine(FindPath(startPos, targetPos,heuristic));
        }

        private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos,Heuristic heuristic)
        {

            Debug.Log($"Calculating path from {startPos} {targetPos} using {heuristic.name}");
            var waypoints = new Vector3[0];
            var pathSuccess = false;

            var startNode = grid.GetNodeFromWorldPoint(startPos);
            var targetNode = grid.GetNodeFromWorldPoint(targetPos);


            if (!startNode.pared && !targetNode.pared)
            {
                var openSet = new Heap<NodeHeaped>(grid.MaxSize);
                var closedSet = new HashSet<NodeHeaped>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    var currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (var neighbor in grid.GetNeighbors(currentNode))
                    {
                        if (neighbor.pared || closedSet.Contains(neighbor)) continue;

                        var newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor,heuristic);

                        // Si es negativo ponemos 0
                        newCostToNeighbor = (newCostToNeighbor < 0) ? 0 : newCostToNeighbor;

                        if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = newCostToNeighbor;
                            neighbor.hCost = GetDistance(neighbor, targetNode,heuristic);
                            neighbor.parent = currentNode;

                            if (!openSet.Contains(neighbor))
                                openSet.Add(neighbor);
                            else 
                                openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }

            yield return null;
            if (pathSuccess) waypoints = RetracePath(startNode, targetNode);
            requestManagerB.FinishedProcessingPath(waypoints, pathSuccess);
        }

        private Vector3[] RetracePath(NodeHeaped startNode, NodeHeaped endNode)
        {
            var path = new List<Vector3>();
            NodeHeaped currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.worldPosition);
                currentNode = (NodeHeaped) currentNode.parent;
            }


            path.Reverse();
            return path.ToArray();
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


        // 1 aliado
        // 0 nada
        // -1 enemigo
        private int MejorPeorTerreno(NodeHeaped nodeB)
        {

            var position = nodeB.worldPosition;

            var terreno = agente.controlador.GetTerrainLayer(position,
                FindObjectOfType<Terrain>());

            if (terreno == agente.mejorTerreno)
            {
                return 1;
            }
            if (terreno == agente.peorTerreno)
            {
                return -1;
            }

            return 0;

        }


        private float GetDistance(NodeHeaped nodeA, NodeHeaped nodeB,Heuristic heuristic)
        {
            var h = heuristic.GetH(nodeA, nodeB);

            // Si el nodo es alidado restamos uno
            // 1 aliado

            // -1 enemigo
            // h(n) debe ser menor que h*(n) 
            // Es decir la heuristic no puede sobrepasar a la real
            var extraInf = AliadoNadaEnemigo(nodeB) switch
            {
                -1 => 1, // Enemigo coste se mantiene
                0 => 0.9, // No hay nada es preferible ir por aqui
                1 => 0.7, // Aliado muy preferible ir por aqui

                _ => throw new ArgumentOutOfRangeException()
            };

            var extraTerreno = MejorPeorTerreno(nodeB) switch
            {
                -1 => 1, // Peor terreno se mantiene
                0 => 0.9, // No es el peor mejor por aqui
                1 => 0.7, // Mejor terreno

                _ => throw new ArgumentOutOfRangeException()
            };


            var final = h * extraInf * extraTerreno;

            if (debug) Debug.Log(final);
            return (float) (final);
        }
    }
}