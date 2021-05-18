using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LRTA : SteeringBehaviour
{
    private Transform actual;
    [SerializeField] private GridChungo grid;

    [SerializeField] private Heuristic heuristic;
    private Node startNode;

    [SerializeField] private Transform objetivo;
    private Node targetNode;

    private List<Node> todosLosNodos;

    public bool targetExists;

    private void Awake()
    {
        startNode = grid.GetNodeFromWorldPoint(transform.position);
        targetNode = grid.GetNodeFromWorldPoint(objetivo.position);
        Debug.Log("Inicio LRTA:" + transform.position);
        todosLosNodos = new List<Node>();
    }


    private void Start()
    {
        startNode = grid.GetNodeFromWorldPoint(transform.position);
        targetNode = grid.GetNodeFromWorldPoint(objetivo.position);
        todosLosNodos = new List<Node>();
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

    private Node CalculatePath()
    {
        //Avanzo al primer nodo
        var nodoActual = startNode;
        Debug.Log("Avanzo a  ->" + startNode);
        grid.path.Add(nodoActual);
        todosLosNodos.Remove(startNode);
        //Mientras que queden nodos en el open
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
        return nodoActual;
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

    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));
        if (!targetExists)
            return returnDebuged(Color.red);

        Node movimiento = CalculatePath();
        float distancia = Vector3.Distance(miAgente.transform.position, movimiento.worldPosition);
        // Comprobamos si estamos en la posicion +-
        if (distancia > miAgente.rInterior)
        {
            this.steering.velocidad = Vector3.ClampMagnitude(movimiento.worldPosition - miAgente.transform.position,
                miAgente.mVelocidad);
        }
        else
        {
            // Si estamos en la posicion +- ponemos a false el flag
            targetExists = false;
            miAgente.ArrivedToTarget();
        }
        double angle = miAgente.MinAngleToRotate(movimiento.worldPosition);
        if (Math.Abs(angle) >= Math.Abs(miAgente.aExterior))
        {
            this.steering.rotacion = (float)angle;
        }
        return steering;
    }
}
