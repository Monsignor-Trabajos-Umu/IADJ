using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding
{
    public class Node
    {
        public readonly int gridX;
        public readonly int gridY;

        public float gCost;
        public float hCost;

        public bool pared;
        public Node parent;
        public Vector3 worldPosition;

        public Node(bool pared, Vector3 worldPosition, int gridX, int gridY)
        {
            this.pared = pared;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public float fCost => gCost + hCost;

        public override bool Equals(object obj)
        {
            // If the passed object is null
            if (!(obj is Node node)) return false;
            return gridX == node.gridX
                   && gridY == node.gridY;
        }

        public override int GetHashCode() => gridX.GetHashCode() ^ gridY.GetHashCode();

        public override string ToString() =>
            $"{nameof(pared)}: {pared}, {nameof(worldPosition)}: {worldPosition}, {nameof(gridX)}: {gridX}, {nameof(gridY)}: {gridY}, {nameof(gCost)}: {gCost}, {nameof(hCost)}: {hCost}, {nameof(fCost)}: {fCost}";
    }
}