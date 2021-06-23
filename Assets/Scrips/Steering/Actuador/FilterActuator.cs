using UnityEngine;

public class FilterActuator : BaseActuator
{
    public override Steering Act(Steering steering)
    {
        steering.lineal.y = 0;
        return steering;
    }

    public override Steering Act(Steering steering, Vector3 angulo) => Act(steering);
}