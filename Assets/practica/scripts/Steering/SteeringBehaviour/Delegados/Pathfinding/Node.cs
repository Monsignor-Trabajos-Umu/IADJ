using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    
    public bool pared;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;
    public Node(bool pared, Vector3 worldPosition, int gridX, int gridY)
    {
        this.pared = pared;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public override string ToString()
    {
        return $"{nameof(pared)}: {pared}, {nameof(worldPosition)}: {worldPosition}, {nameof(gridX)}: {gridX}, {nameof(gridY)}: {gridY}, {nameof(gCost)}: {gCost}, {nameof(hCost)}: {hCost}, {nameof(fCost)}: {fCost}";
    }
}
