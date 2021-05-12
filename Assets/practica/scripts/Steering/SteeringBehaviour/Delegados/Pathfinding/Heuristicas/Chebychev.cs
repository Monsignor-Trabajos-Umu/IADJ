using UnityEngine;

public class Chebychev : Heuristic
{
    public override float GetH(Node node,Node objetivo)
    {
        var dx = Mathf.Abs(node.gridX - objetivo.gridX);
        var dy = Mathf.Abs(node.gridY - objetivo.gridY);
        return (float) Mathf.Max(dx, dy);
    }
}
