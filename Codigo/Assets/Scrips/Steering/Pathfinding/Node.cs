using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding
{
    public class Node
    {
        public readonly int gridX;
        public readonly int gridZ;

        public float gCost;
        public float hCost;

        public bool pared;
        public Node parent;
        public Vector3 worldPosition;

        public Node(bool pared, Vector3 worldPosition, int gridX, int gridZ)
        {
            this.pared = pared;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridZ = gridZ;
        }


        public float fCost => gCost + hCost;

        public override bool Equals(object obj)
        {
            // If the passed object is null
            if (!(obj is Node node)) return false;
            return gridX == node.gridX
                   && gridZ == node.gridZ;
        }

        public override int GetHashCode() => gridX.GetHashCode() ^ gridZ.GetHashCode();

        public override string ToString() =>
            $"{nameof(pared)}: {pared}, {nameof(worldPosition)}: {worldPosition}, {nameof(gridX)}: {gridX}, {nameof(gridZ)}: {gridZ}, {nameof(gCost)}: {gCost}, {nameof(hCost)}: {hCost}, {nameof(fCost)}: {fCost}";
    }
}