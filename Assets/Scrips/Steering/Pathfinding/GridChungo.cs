using System;
using System.Collections.Generic;
using Assets.Scrips.Steering.Pathfinding;
using UnityEngine;

public class GridChungo : MonoBehaviour
{
    private int gridSizeX, gridSizeZ;
    [SerializeField] private float gridWorldSizeX;
    [SerializeField] private float gridWorldSizeZ;
    //Booleano para saber si el terreno es el de Unity o no.
    [SerializeField] public Terrain terreno;

    private float nodeDiameter;
    [SerializeField] private float nodeRaidus;
    [SerializeField] private LayerMask paredesLayerMask;


    public List<Node> path = new List<Node>();

    //Uso un array en vez de una lista porque es mas rapido buscar
    [field: SerializeField] public Node[,] getGrid { get; set; }

    [SerializeField] private bool debug;

    public void Awake()
    {
        nodeDiameter = nodeRaidus * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSizeX / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSizeZ / nodeDiameter);
        CreateGrid();
    }

    //array de x por x donde cada casilla es un nodo
    // si el nodo hace colision con una pared o montañas es un terreno por donde no podemos pasar
    // Nodo tiene la propiedad de que no se puede atravesar
    private void CreateGrid()
    {
        // Creamos el array de Nodes
        this.getGrid = new Node[gridSizeX, gridSizeZ];
        var pInicial = transform.position - Vector3.right * gridWorldSizeX / 2 -
                       Vector3.forward * gridWorldSizeZ / 2;

        for (var x = 0; x < gridSizeX; x++)
        for (var z = 0; z < gridSizeZ; z++)
        {
            var worldPoint = pInicial + Vector3.right * (x * nodeDiameter + nodeRaidus) +
                             Vector3.forward * (z * nodeDiameter + nodeRaidus);
            worldPoint.y = transform.position.y;
            bool pared = false;
            if (terreno != null && terreno.gameObject.activeSelf)
            {
                var control = FindObjectOfType<Controlador>();
                int i = control.GetTerrainLayer(worldPoint, terreno);
                    Debug.Log(i);
                //Se comprueba si es el valor del layer de las montañas
                if (i == 1)
                    pared = true;
            }
            else
            {
                pared = Physics.CheckSphere(worldPoint, nodeRaidus, paredesLayerMask);
            }
            this.getGrid[x, z] = new Node(pared, worldPoint, x, z);
        }
    }

    //Obtenemos nodo a partir de un vector posicion
    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        // Le sumo la mitad por si es negativo
        var pX = (worldPosition.x + gridWorldSizeX / 2) / gridWorldSizeX;
        var pZ = (worldPosition.z + gridWorldSizeZ / 2) / gridWorldSizeZ;
        pX = Mathf.Clamp01(pX);
        pZ = Mathf.Clamp01(pZ);

        var x = Mathf.RoundToInt((gridSizeX - 1) * pX);
        var z = Mathf.RoundToInt((gridSizeZ - 1) * pZ);
        return getGrid[x, z];
    }

    public Vector3 GetWorldPointFromNode(Node nodo)
    {
        return nodo.worldPosition;
    }

    public List<Node> GetNeigbours(Node node)
    {
        var neightBours = new List<Node>();
        for (var x = -1; x <= 1; x++)
            for (var z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                // pNeigboursX/Z son las posiciones en el grid de los nodos
                var pNeigboursX = node.gridX + x;
                var pNeigboursZ = node.gridY + z;
                // Ahora hay que comprobar que esos nodos existen
                // Es decir que estan dentro del grid
                if (pNeigboursX >= 0 && pNeigboursX < gridSizeX &&
                    pNeigboursZ >= 0 && pNeigboursZ < gridSizeZ)
                {
                    var nodo = getGrid[pNeigboursX, pNeigboursZ];
                    if (!nodo.pared) neightBours.Add(nodo);
                }
            }

        return neightBours;
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSizeX, 1, gridWorldSizeZ));
        //Debug.Log("Path size " + path.Count);
        if (getGrid != null)
            foreach (var n in getGrid)
            {
                Gizmos.color = n.pared ? Color.red : Color.white;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
    }
}