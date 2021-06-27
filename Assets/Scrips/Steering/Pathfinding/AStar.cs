using System;
using System.Collections.Generic;
using Assets.Scrips.Steering.Pathfinding;
using UnityEngine;

public class AStar : MonoBehaviour
{


    [SerializeField] private GameObject gridChungoPrefab; 
    [SerializeField] private GridChungo grid; //Lleva los costes generales
    [SerializeField] private InfluenceMapControl influeceMap;
    [SerializeField] private Heuristic heuristic;
    //Si soy las casillas negativas en mi mapa de influencia
    [SerializeField] private bool bandoPositivo;
    public Transform seeker, target;
    [SerializeField] private bool debug=false;



    // Tenemos que esperar que el GridChungo se incialize
    private void Start()
    {

        var agenteNpc = gameObject.GetComponent<AgentNpc>();

        // Clonamos el GridChungo base
        var parent = GameObject.Find("Inst").transform;

        grid = Instantiate(gridChungoPrefab, parent).GetComponent<GridChungo>();

        grid.name = $"{gridChungoPrefab} - {agenteNpc.name}";

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

    public List<Node> GetPath(Vector3 startPos, Vector3 targetPos)
    {
        // Calculamos el path
        FindPath(startPos, targetPos);


        return grid.path;
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Borramos el path anterior
        grid.path.Clear();


        var startNode = grid.GetNodeFromWorldPoint(startPos);
        var targetNode = grid.GetNodeFromWorldPoint(targetPos);

        //Debug.Log(startNode);
        //Debug.Log(targetNode);


        var openSet = new List<Node>();
        var closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            var actualNode = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
                if (openSet[i].fCost < actualNode.fCost ||
                    Mathf.Approximately(openSet[i].fCost, actualNode.fCost))
                    if (openSet[i].hCost < actualNode.hCost)
                        actualNode = openSet[i];

            openSet.Remove(actualNode);
            closedSet.Add(actualNode);

            if (Equals(actualNode, targetNode))
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (var neighbor in grid.GetNeigbours(actualNode))
            {
                if (neighbor.pared || closedSet.Contains(neighbor)) continue;

               
             
                
                var newCostToNeighbor = actualNode.gCost + GetDistance(actualNode, neighbor);

                // Si es negativo ponemos 0
                newCostToNeighbor = (newCostToNeighbor < 0) ? 0 : newCostToNeighbor;

                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = actualNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
    }
    // 1 aliado
    // 0 nada
    // -1 enemigo
    private int AliadoNadaEnemigo(Node node)
    {
        var value = influeceMap.GetInfluence(node.gridX, node.gridY);

        if (value == 0) return 0;
        return bandoPositivo switch
        {
            // Bando positivo
            true when value > 0 => 1,  //Mismo bando
            true when value < 0 => -1, //Bando enemigo
            // Bando negativo
            false when value > 0 => -1, //Mismo bando
            false when value < 0 => 1, //Bando enemigo
            _ => 0
        };
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        var currentNode = endNode;

        while (!Equals(currentNode, startNode))
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    private float GetDistance(Node nodeA, Node nodeB)
    {

        var h =  heuristic.GetH(nodeA, nodeB);
         
        // Si el nodo es alidado restamos uno
        // 1 aliado
      
        // -1 enemigo
        // h(n) debe ser menor que h*(n) 
        // Es decir la heuristic no puede sobrepasar a la real
        var extra = AliadoNadaEnemigo(nodeB) switch  
        {  
            -1 => 0, // Enemigo
            0 => -1, // 0 nada
            1 => -2, // Aliado

            _ => throw new ArgumentOutOfRangeException()
        };

        if(debug) Debug.Log(h);
        return h ;

    } 
}