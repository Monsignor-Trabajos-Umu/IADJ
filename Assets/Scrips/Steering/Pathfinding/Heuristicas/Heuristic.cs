using Assets.Scrips.Steering.Pathfinding;
using UnityEngine;

public abstract class Heuristic: MonoBehaviour
{
    public abstract float GetH(Node actual,Node objetivo);
}