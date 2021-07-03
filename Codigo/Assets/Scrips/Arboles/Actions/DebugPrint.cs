using System.Collections;
using UnityEngine;
using UniBT;
namespace Assets.Scrips.Actions
{
   public class DebugPrint : UniBT.Action
{
    [SerializeField] 
    private float waitTime;

    private float elapsedTime = 0.0f;

    protected override Status OnUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < waitTime)
        {
            return Status.Running;
        }

        elapsedTime = 0.0f;
        Debug.Log("Running bro");
        return Status.Success;
    }

    // abort when the parent conditional changed on previous status is running.
    public override void Abort()
    { 
        elapsedTime = 0.0f;
    }
}
}