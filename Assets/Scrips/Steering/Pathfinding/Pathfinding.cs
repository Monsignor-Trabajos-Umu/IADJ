using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Pathfinding : MonoBehaviour
{
    private Transform actual;
    [SerializeField] private GridChungo grid;

    [SerializeField] private Heuristic heuristic;
    private CustomNode startCustomNode;

    [SerializeField] private Transform target;
    private CustomNode targetCustomNode;

    private List<CustomNode> todosLosNodos;

    private void Awake()
    {
        startCustomNode = grid.GetNodeFromWorldPoint(transform.position);
        targetCustomNode = grid.GetNodeFromWorldPoint(target.position);
        Debug.Log("Transfrom pathfinding" + transform.position);
        todosLosNodos = new List<CustomNode>();
    }


    private void Start()
    {
        foreach (var node in grid.getGrid)
            if (!node.pared)
            {
                node.hCost = heuristic.GetH(node, targetCustomNode);
                todosLosNodos.Add(node);
            }

        Debug.Log("Numero nodos ->" + todosLosNodos.Count);
        Debug.Log("Nodo Entrada ->" + startCustomNode);
        Debug.Log("Nodo salida ->" + targetCustomNode);
        CalculatePath();
    }


    private void CalculatePath()
    {
        //Avanzo al primer nodo
        var nodoActual = startCustomNode;
        Debug.Log("Avanzo a  ->" + startCustomNode);
        grid.path.Add(nodoActual);
        todosLosNodos.Remove(startCustomNode);

        // Flag por si tarda mucho
        var count = 0;
        //Mientras que queden nodos en el open
        while (nodoActual != targetCustomNode )
        {
            count++;
            // Cogemos los vecinos
            var neigbours = grid.GetNeigbours(nodoActual);
            // Actualizamos el gCost de los vecinos
            neigbours.ForEach(n => n.gCost = GetDistance(nodoActual, n));
            // Cogemos el que tenga menor valor
            var minimo = neigbours.OrderByDescending(v => v.fCost).Last();
            // Actualizamos h a fMinimo si fMinimo es superior a h
            nodoActual.hCost = minimo.fCost > nodoActual.hCost ? minimo.fCost : nodoActual.hCost;
            // Vamos al nodo
            Debug.Log("Yendo a =>" + nodoActual);
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
    private float GetDistance(CustomNode customNodeA, CustomNode customNodeB)
    {
        return Vector3.Distance(customNodeA.worldPosition, customNodeB.worldPosition);
    }
}