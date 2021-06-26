using UnityEngine;
 
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase
 
[Condition("Nuevas/CheckMode")]
[Help("Checks whether it is night. It searches for the first light labeled with " +
      "the 'MainLight' tag, and looks for its DayNightCycle script, returning the" +
      "informed state. If no light is found, false is returned.")]
public class CheckMode : ConditionBase
{
    public override bool Check()
    {
        GameObject light = GameObject.FindGameObjectWithTag("MainLight");
        if (light != null)
        {
            //DayNightCycle dnc = light.GetComponent<DayNightCycle>();
            //if (dnc != null)
            //    return dnc.isNight;
        }
 
        return false;
    }
} // class IsNightCondition