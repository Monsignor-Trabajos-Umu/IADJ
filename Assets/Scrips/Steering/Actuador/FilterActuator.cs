using UnityEngine;

// El mismo filtro de ArbitroSteering
// El actuador elimina la componente Y del steering
public class FilterActuator : BaseActuator
{
    public override Steering Act(Steering steering)
    {
        steering.lineal.y = 0;
        return steering;
    }

    public override Steering Act(Steering steering, AgentNPC agenteNPC) => Act(steering);
}