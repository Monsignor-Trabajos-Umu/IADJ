using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    public class FastGrid : MonoBehaviour
    {
        [SerializeField] private bool debug = false;
        private int gridSizeX, gridSizeZ;
        [SerializeField] private float gridWorldSizeX;

        [SerializeField] private float gridWorldSizeZ;

        private float nodeDiameter;
        [SerializeField] private float nodeRaidus;
        [SerializeField] private LayerMask paredesLayerMask;


         private List<NodeHeaped> path = new List<NodeHeaped>();

        //Booleano para saber si el terreno es el de Unity o no.
        [SerializeField] private Terrain terreno;

        //Uso un array en vez de una lista porque es mas rapido buscar
        [field: SerializeField] private NodeHeaped[,] GetGrid { get; set; }


        public int MaxSize => gridSizeX * gridSizeZ;


        private void Awake()
        {
           if(debug) Debug.Log("Loading  GridChungo");
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
            GetGrid = new NodeHeaped[gridSizeX, gridSizeZ];
            var pInicial = transform.position - Vector3.right * gridWorldSizeX / 2 -
                           Vector3.forward * gridWorldSizeZ / 2;

            for (var x = 0; x < gridSizeX; x++)
            for (var z = 0; z < gridSizeZ; z++)
            {
                var worldPoint = pInicial +
                                 Vector3.right * (x * nodeDiameter + nodeRaidus) +
                                 Vector3.forward * (z * nodeDiameter + nodeRaidus);
                worldPoint.y = transform.position.y;
                var pared = false;
                if (terreno != null && terreno.gameObject.activeSelf)
                {
                    var control = FindObjectOfType<Controlador>();
                    var i = control.GetTerrainLayer(worldPoint, terreno);
                    //Debug.Log(i);
                    //Se comprueba si es el valor del layer de las montañas
                    if (i == 1)
                        pared = true;
                }
                else
                {
                    pared = Physics.CheckSphere(worldPoint, nodeRaidus, paredesLayerMask);
                }

                GetGrid[x, z] = new NodeHeaped(pared, worldPoint, x, z);
            }
        }


        //Obtenemos nodo a partir de un vector posicion
        public NodeHeaped GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            // Le sumo la mitad por si es negativo
            var pX = (worldPosition.x + gridWorldSizeX / 2) / gridWorldSizeX;
            var pZ = (worldPosition.z + gridWorldSizeZ / 2) / gridWorldSizeZ;
            pX = Mathf.Clamp01(pX);
            pZ = Mathf.Clamp01(pZ);

            var x = Mathf.RoundToInt((gridSizeX - 1) * pX);
            var z = Mathf.RoundToInt((gridSizeZ - 1) * pZ);
            return GetGrid[x, z];
        }

        public Vector3 GetWorldPointFromNode(NodeHeaped nodo) => nodo.worldPosition;

        public List<NodeHeaped> GetNeighbors(NodeHeaped node)
        {
            var neighbors = new List<NodeHeaped>();
            for (var x = -1; x <= 1; x++)
            for (var z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                // pNeigboursX/Z son las posiciones en el grid de los nodos
                var pNeighBorX = node.gridX + x;
                var pNeighBorZ = node.gridZ + z;
                // Ahora hay que comprobar que esos nodos existen
                // Es decir que estan dentro del grid
                if (pNeighBorX >= 0 && pNeighBorX < gridSizeX &&
                    pNeighBorZ >= 0 && pNeighBorZ < gridSizeZ)
                {
                    var nodo = GetGrid[pNeighBorX, pNeighBorZ];
                    if (!nodo.pared) neighbors.Add(nodo);
                }
            }

            return neighbors;
        }

        protected virtual void OnDrawGizmos()
        {
            if (!debug) return;
            Gizmos.DrawWireCube(transform.position,
                new Vector3(gridWorldSizeX, 1, gridWorldSizeZ));
            //Debug.Log("Path size " + path.Count);
            if (GetGrid != null)
                foreach (var n in GetGrid)
                {
                    Gizmos.color = n.pared ? Color.red : Color.white;
                    if (path != null)
                        if (path.Contains(n))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
        }
    }
}