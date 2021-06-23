using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankActuator : BaseActuator
{
    public override Steering Act(Steering steering) => throw new System.NotImplementedException();

    public override Steering Act(Steering steering, Vector3 angulo)
    {
        Steering acted = new Steering(0, new Vector3(0, 0, 0));
        //if (rotared)
        //{
        //    acted.angular
        //}
        return acted;

    }
}
