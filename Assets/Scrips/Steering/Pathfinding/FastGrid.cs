using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding
{
    // Es un gridchungo pero sin el awake
    public class FastGrid : GridChungo
    {
        [SerializeField] private GridChungo grid; //Grid que vamos a clonar


        private void Awake()
        {
            Debug.Log("Loading  FastGrid");
            nodeDiameter = nodeRaidus * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSizeX / nodeDiameter);
            gridSizeZ = Mathf.RoundToInt(gridWorldSizeZ / nodeDiameter);
            GetGrid = grid.GetGrid.Clone() as Node[,];
        }
        protected override void OnDrawGizmos()
        {
            if (!debug) return;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSizeX, 1, gridWorldSizeZ));
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