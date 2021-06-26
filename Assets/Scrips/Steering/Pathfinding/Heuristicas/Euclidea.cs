using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Euclidea : Heuristic
{
    public override float GetH(CustomNode customNode,CustomNode objetivo)
    {
        return Vector3.Distance(customNode.worldPosition, objetivo.worldPosition);
    }
}
