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

        grid.EnableDebug();
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        var startNode = grid.GetNodeFromWorldPoint(startPos);
        var targetNode = grid.GetNodeFromWorldPoint(targetPos);

        var openSet = new List<Node>();
        var closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            var node = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
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

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        var dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        var dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}