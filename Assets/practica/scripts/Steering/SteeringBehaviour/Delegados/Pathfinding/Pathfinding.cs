using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform actual, target;
    [SerializeField] private GridChungo grid;
    private readonly List<Node> path = new List<Node>();

    private void Awake()
    {
        grid = GetComponent<GridChungo>();
    }

    private void Update()
    {
    }

    public void CalculatePath(Vector3 currentPosition, Vector3 targetPos)
    {
        var startNode = grid.GetNodeFromWorldPoint(actual.position);
        var targetNode = grid.GetNodeFromWorldPoint(target.position);

        // Creamos los open y closet set;
        var openSet = new List<Node>();
        var closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        //Mientras que queden nodos en el open
        while (openSet.Count > 0)
        {
            // Cogemos el que tenga menor valor
            var cNodo = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
            {
                var iNodo = openSet[i];
                if (cNodo.fCost < iNodo.fCost ||
                    cNodo.fCost == iNodo.fCost &&
                    iNodo.hCost < cNodo.hCost)
                    cNodo = iNodo;
            }

            // Una vez que lo tenemos lo movemos al closeSet
            openSet.Remove(cNodo);
            closedSet.Add(cNodo);
            //Vemos si llegado
            if (cNodo == targetNode)
            {
                getPath(startNode, targetNode);
                return;
            }

            ;

            //A que vecino vamos ?

            foreach (var neigbour in grid.GetNeigbours(cNodo))
            {
                // Si el vecino es pared pasamos
                if (neigbour.pared || closedSet.Contains(neigbour)) continue;
                //Vemos cual es el camino mas corto
                var newCostToNeighbour = cNodo.gCost + getDistance(cNodo, neigbour);
                if (newCostToNeighbour < neigbour.gCost || !openSet.Contains(neigbour))
                {
                    neigbour.gCost = newCostToNeighbour;
                    neigbour.hCost = getDistance(neigbour, targetNode);
                    neigbour.parent = cNodo;
                    if (!openSet.Contains(cNodo))
                        openSet.Add(neigbour);
                }
            }
        }
    }

    /*
     * Calculamos nuestro path
     */
    public void getPath(Node starNode, Node endNode)
    {
        var currentNode = endNode;
        while (currentNode != starNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
    }


    /*
     * Un nodo es un cuadrado 1x1
     * Su longitud diagonal es ~1,4
     * Para que el coste nos salga mas menos redondo multiplacamos por 10
     * Ergo su coste en diagonal es 14 y en horizontal 10
     * --------------------------------------------------
     * Tambien podemos obtener su transfor y usar vector3Distance
     */
    private float getDistance(Node current, Node objetivo)
    {
        return Vector3.Distance(current.worldPosition, objetivo.worldPosition);
    }
}