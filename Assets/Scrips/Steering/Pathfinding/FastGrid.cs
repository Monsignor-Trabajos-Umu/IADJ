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
    }
}