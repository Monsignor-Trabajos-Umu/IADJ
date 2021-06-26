using UnityEngine;

public abstract class Heuristic
{
    public abstract float GetH(CustomNode actual,CustomNode objetivo);
}