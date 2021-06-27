using Assets.Scrips.Steering.Pathfinding;
using UnityEngine;

public class Chebychev : Heuristic
{
    public override float GetH(Node node,Node objetivo)
    {
        var dx = Mathf.Abs(node.gridX - objetivo.gridX);
        var dy = Mathf.Abs(node.gridZ - objetivo.gridZ);
        return (float) Mathf.Max(dx, dy);
    }
}
