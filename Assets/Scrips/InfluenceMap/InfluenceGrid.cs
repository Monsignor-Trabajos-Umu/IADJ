using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InfluenceGrid : MonoBehaviour
{
    private int gridSizeX, gridSizeZ;
    [SerializeField] public float gridWorldSizeX;
    [SerializeField] public float gridWorldSizeZ;
    [SerializeField] public bool mostrarMapa = false;

    private float nodeDiameter;
    [SerializeField] private float nodeRaidus;

    public List<NodoI> path = new List<NodoI>();
    [field: SerializeField] public NodoI[,] getGrid { get; set; }


    public void Awake()
    {

        nodeDiameter = nodeRaidus * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSizeX / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSizeZ / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        // Creamos el array de Nodes
        this.getGrid = new NodoI[gridSizeX, gridSizeZ];
        var pInicial = transform.position - Vector3.right * gridWorldSizeX / 2 -
                       Vector3.forward * gridWorldSizeZ / 2;

        for (var x = 0; x < gridSizeX; x++)
            for (var z = 0; z < gridSizeZ; z++)
            {
                var worldPoint = pInicial + Vector3.right * (x * nodeDiameter + nodeRaidus) +
                                 Vector3.forward * (z * nodeDiameter + nodeRaidus);
                worldPoint.y = transform.position.y;

                this.getGrid[x, z] = new NodoI(worldPoint, x, z);
            }
    }


    public NodoI GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        // Le sumo la mitad por si es negativo
        var pX = (worldPosition.x) / gridWorldSizeX;
        var pZ = (worldPosition.z) / gridWorldSizeZ;
        pX = Mathf.Clamp01(pX);
        pZ = Mathf.Clamp01(pZ);

        var x = Mathf.RoundToInt((gridSizeX) * pX);
        var z = Mathf.RoundToInt((gridSizeZ) * pZ);
        return getGrid[x, z];
    }


    public List<NodoI> GetNeighbors(NodoI node,int radio)
    {
        // Calculamos el radio en casillas al rededor del punto

         
        var neighbors = new List<NodoI>();

        for (var x = -radio; x <= radio; x++)
            for (var z = -radio; z <= radio; z++)
            {
                if (x == 0 && z == 0) continue;
                // pNeigboursX/Z son las posiciones en el grid de los nodos
                var pNeighborsX = node.x + x;
                var pNeighborsY = node.y + z;
                // Ahora hay que comprobar que esos nodos existen
                // Es decir que estan dentro del grid
                if (pNeighborsX >= 0 && pNeighborsX < gridSizeX &&
                    pNeighborsY >= 0 && pNeighborsY < gridSizeZ)
                {
                    var nodo = getGrid[pNeighborsX, pNeighborsY];
                    neighbors.Add(nodo);
                }
            }

        return neighbors;
    }

    //Obtiene el NodoI de la esquina inferior derecha
    public NodoI getBottomRight()
    {
        return getGrid[gridSizeX-1,gridSizeZ-1];
    }

    private void OnDrawGizmos()
    {
        if (mostrarMapa)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSizeX, 1, gridWorldSizeZ));
            if (getGrid != null)
                foreach (var n in getGrid)
                {
                    n.calcularColor();
                    Gizmos.color = n.color;
                    if (path != null)
                        if (path.Contains(n))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));

                }
        }
    }
}
