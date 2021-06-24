using UnityEngine;

public abstract class BaseActuator : MonoBehaviour
{
    public abstract Steering Act(Steering steering);

    public abstract Steering Act(Steering steering, Vector3 direccion);
}