using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Euclidea : Heuristic
{
    public override float GetH(Node node,Node objetivo)
    {
        return Vector3.Distance(node.worldPosition, objetivo.worldPosition);
    }
}
