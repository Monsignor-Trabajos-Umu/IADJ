using UnityEngine;

public class Chebychev : Heuristic
{
    public override float GetH(CustomNode customNode,CustomNode objetivo)
    {
        var dx = Mathf.Abs(customNode.gridX - objetivo.gridX);
        var dy = Mathf.Abs(customNode.gridY - objetivo.gridY);
        return (float) Mathf.Max(dx, dy);
    }
}
