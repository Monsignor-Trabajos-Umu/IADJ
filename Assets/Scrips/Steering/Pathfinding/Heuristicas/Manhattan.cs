using Assets.Scrips.Steering.Pathfinding;
using UnityEngine;

public class Manhattan : Heuristic
{
    public override float GetH(Node node,Node objetivo)
    {
        var dx = Mathf.Abs(node.gridX - objetivo.gridX);
        var dy = Mathf.Abs(node.gridZ - objetivo.gridZ);

        return (float) 1 * (dx + dy);
    }
}