using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LRTA : Arrive
{
    private bool atFinalTarget;

    [SerializeField] private GridChungo grid;

    [SerializeField] private Heuristic heuristic;

    [SerializeField] private Transform objetivo;

    private CustomNode startCustomNode;
    private CustomNode targetCustomNode;


    private CustomNode tempObjetive;

    private List<CustomNode> todosLosNodos;

    private void Awake()
    {
        startCustomNode = grid.GetNodeFromWorldPoint(transform.position);
        targetCustomNode = grid.GetNodeFromWorldPoint(objetivo.position);
        Debug.Log("Inicio LRTA:" + transform.position);
        todosLosNodos = new List<CustomNode>();
    }


    private void Start()
    {
        startCustomNode = grid.GetNodeFromWorldPoint(transform.position);
        targetCustomNode = grid.GetNodeFromWorldPoint(objetivo.position);
        todosLosNodos = new List<CustomNode>();
        foreach (var node in grid.getGrid)
            if (!node.pared)
            {
                node.hCost = heuristic.GetH(node, targetCustomNode);
                todosLosNodos.Add(node);
            }

        Debug.Log("Numero nodos ->" + todosLosNodos.Count);
        Debug.Log("Nodo Entrada ->" + startCustomNode);
        Debug.Log("Nodo salida ->" + targetCustomNode);

        // Start values
        useCustom = true;
        atFinalTarget = false;
    }

    private bool AtNode(Vector3 aPosition, CustomNode oCustomNode, double error)
    {
        var aNode = grid.GetNodeFromWorldPoint(aPosition);
        // Si el nodo es el mismo d
        if (aNode.Equals(oCustomNode)) return true;
        // Si estamos en el margen error
        return (oCustomNode.worldPosition - aPosition).magnitude < error;
    }


    private bool AtFinal(Vector3 aPosition, double error)
    {
        // Speed up
        if (atFinalTarget) return true;
        
        // Si mi actual es mi objetivo ya hemos llegado
        atFinalTarget = AtNode(aPosition, targetCustomNode, error);
        return atFinalTarget;
    }


    private CustomNode CalculatePathToTargetNode(CustomNode actual)
    {
        //Avanzo al primer nodo


        grid.path.Add(actual);
        // Pudo revisitar un nodo ya visitado todosLosNodos.Remove(startCustomNode);
        //Mientras que queden nodos en el open
        var neigbours = grid.GetNeigbours(actual);
        // Actualizamos el gCost de los vecinos
        neigbours.ForEach(n => n.gCost = GetDistance(actual, n));
        // Cogemos el que tenga menor valor
        var minimo = neigbours.OrderByDescending(v => v.fCost).Last();
        // Actualizamos h a fMinimo si fMinimo es superior a h
        actual.hCost = minimo.fCost > actual.hCost ? minimo.fCost : actual.hCost;
        // Vamos al nodo objetivo
        var objetivo = minimo;
        Debug.Log($"Voy de [{actual.gridX},{actual.gridY}] a [{objetivo.gridX},{objetivo.gridY}]");
        return objetivo;
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

    public override Steering GetSteering(AgentNPC miAgente)
    {
        var currentPosition = miAgente.transform.position;
        var currentNode = grid.GetNodeFromWorldPoint(currentPosition);
        var error = miAgente.RExterior;
        steering = new Steering(0, new Vector3(0, 0, 0));
        if (AtFinal(currentPosition, error)) return steering;

        // Si existe ya un objetivo temporal vamos a el.
        if (tempObjetive != null)
        {
            //Debug.Log($"Distancia {Vector3.Distance(currentPosition, tempObjetive.worldPosition)} Error {error}");
            // Si estamos lo suficientemente cerca vamos al siguiente nodo 
            //Vector3.Distance(currentPosition, tempObjetive.worldPosition) <= error
            Debug.Log(
                $"Estoy\t[{currentNode.gridX},{currentNode.gridY}]");
            if (AtNode(currentPosition,tempObjetive,error))
            {
               
                // Como tenemos un error tenemos que suponer que nuestro nodo actual no es la
                // posicion sino nuestro objetivo
                Debug.Log($"He yegado al objetivo a[{tempObjetive.gridX},{tempObjetive.gridY}]");
                tempObjetive = CalculatePathToTargetNode(tempObjetive);
            }
            Debug.Log($"Voy a  [{tempObjetive.gridX},{tempObjetive.gridY}]");
            // Sino Vamos al nodo objetivo
            customDirection = tempObjetive.worldPosition - currentPosition;
            return base.GetSteering(miAgente);
        }

        // Solo se ejecuta la primera vez.
        tempObjetive = CalculatePathToTargetNode(grid.GetNodeFromWorldPoint(currentPosition));


        return steering;
    }
}