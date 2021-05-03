using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;
public class Pathfinding : MonoBehaviour
{
    private Transform actual;
    [SerializeField] private GridChungo grid;

    [SerializeField] private Heuristic heuristic;
    private Node startNode;

    [SerializeField] private Transform target;
    private Node targetNode;

    private readonly List<Node> todosLosNodos = new List<Node>();

    private void Awake()
    {
        startNode = grid.GetNodeFromWorldPoint(transform.position);
        targetNode = grid.GetNodeFromWorldPoint(target.position);
        Debug.Log("Transfrom pathfinding" + transform.position);
    }


    private void Start()
    {
        foreach (var node in grid.getGrid)
            if (!node.pared)
            {
                node.hCost = heuristic.GetH(node, targetNode);
                todosLosNodos.Add(node);
            }

        Debug.Log("Numero nodos ->" + todosLosNodos.Count);
        Debug.Log("Nodo Entrada ->" + startNode);
        Debug.Log("Nodo salida ->" + targetNode);
        CalculatePath();
    }


    private void CalculatePath()
    {
        //Avanzo al primer nodo
        var nodoActual = startNode;
        Debug.Log("Avanzo a  ->" + startNode);
        grid.path.Add(nodoActual);
        todosLosNodos.Remove(startNode);


        // Creamos un set con los open
        var neigbours = new HashSet<Node>();

        // Flag por si tarda mucho
        var count = 0;
        //Mientras que queden nodos en el open
        while (count < 400 && nodoActual != targetNode || todosLosNodos.Count == 0)
        {
            count++;
            // Cogemos los vecinos
            grid.GetNeigbours(nodoActual).ForEach(n => neigbours.Add(n));
            //Debug.LogError("N vecinos " + neigbours.Count);
            if (neigbours.Count == 0)
            {
                Debug.LogError("No hay vecinos :(");
                return;
            }

            // Actualizamos el gCost de los vecinos
            neigbours.ForEach(n => n.gCost = GetDistance(nodoActual, n));
            // Cogemos el que tenga menor valor
            var minimo = neigbours.OrderByDescending(v => v.fCost).Last();
            // Actualizamos h a fMinimo si fMinimo es superior a h
            if (minimo.fCost > nodoActual.hCost)
                nodoActual.hCost = minimo.fCost;
            //nodoActual.hCost = minimo.fCost > nodoActual.hCost ? minimo.fCost : nodoActual.hCost;
            // Vamos al nodo
            //Debug.Log("Yendo a =>" + nodoActual);
            nodoActual = minimo;
            grid.path.Add(nodoActual);
            todosLosNodos.Remove(nodoActual);
        }
    }

    /*
     * Un nodo es un cuadrado 1x1
     * Su longitud diagonal es ~1,4
     * Para que el coste nos salga mas menos redondo multiplacamos por 10
     * Ergo su coste en diagonal es 14 y en horizontal 10
     * --------------------------------------------------
     * Tambien podemos obtener su transfor y usar vector3Distance
     */
    private float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector3.Distance(nodeA.worldPosition, nodeB.worldPosition);
    }
}