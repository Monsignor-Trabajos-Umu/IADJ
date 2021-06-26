using Assets.Scrips.Steering.Pathfinding;
using UnityEngine;

public class Euclidea : Heuristic
{
    public override float GetH(Node node,Node objetivo) => Vector3.Distance(node.worldPosition, objetivo.worldPosition);
}
