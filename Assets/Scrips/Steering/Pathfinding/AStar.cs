using System.Collections.Generic;
using Assets.Scrips.Steering.Pathfinding;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] private GridChungo grid;
    public Transform seeker, target;

    // Tenemos que esperar que el GridChungo se incialize
    private void Start()
    {
        // Clonamos el GridChungo base
        var gridPadre = GameObject.Find("GridMap").GetComponent<FastGrid>();
        var position = gridPadre.GetComponentInParent<Transform>();
        grid = Instantiate(gridPadre,position);

        // Activamos el debug

        //grid.EnableDebug();

        var cp = transform.position;
        var co = cp + Vector3.right * 50;

        GetPath(cp, co);
    }

    public List<Node> GetPath(Vector3 startPos, Vector3 targetPos)
    {
        // Calculamos el path
        FindPath(startPos,targetPos);



        return grid.path;
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Borramos el path anterior
        grid.path.Clear(); 


        var startNode = grid.GetNodeFromWorldPoint(startPos);
        var targetNode = grid.GetNodeFromWorldPoint(targetPos);

        Debug.Log(startNode);
        Debug.Log(targetNode);


        var openSet = new List<Node>();
        var closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            var node = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
                if (openSet[i].fCost < node.fCost || Mathf.Approximately(openSet[i].fCost , node.fCost))
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];

            openSet.Remove(node);
            closedSet.Add(node);

            if (Equals(node, targetNode))
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (var neighbor in grid.GetNeigbours(node))
            {
                if (neighbor.pared || closedSet.Contains(neighbor)) continue;

                var newCostToNeighbor = node.gCost + GetDistance(node, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = node;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
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

    private static float GetDistance(Node nodeA, Node nodeB)
    {
        var pA = nodeA.worldPosition;
        var pB = nodeB.worldPosition;
        return Vector3.Distance(pA, pB);
    }
}