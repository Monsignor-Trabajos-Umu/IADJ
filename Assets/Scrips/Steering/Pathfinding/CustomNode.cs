using UnityEngine;

public class CustomNode
{
    public readonly int gridX;
    public readonly int gridY;

    public float gCost;
    public float hCost;

    public bool pared;
    public Vector3 worldPosition;
    public CustomNode parent;

    public CustomNode(bool pared, Vector3 worldPosition, int gridX, int gridY)
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
        if (!(obj is CustomNode node)) return false;
        return gridX == node.gridX
               && gridY == node.gridY;
    }

    public override int GetHashCode()
    {
        return gridX.GetHashCode() ^ gridY.GetHashCode();
    }

    public override string ToString()
    {
        return
            $"{nameof(pared)}: {pared}, {nameof(worldPosition)}: {worldPosition}, {nameof(gridX)}: {gridX}, {nameof(gridY)}: {gridY}, {nameof(gCost)}: {gCost}, {nameof(hCost)}: {hCost}, {nameof(fCost)}: {fCost}";
    }
}