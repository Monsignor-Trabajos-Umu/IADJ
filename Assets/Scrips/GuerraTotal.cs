using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
public class GuerraTotal : MonoBehaviour
{
    [SerializeField] bool TotalWar = false;

    private void LateUpdate()
    {
        ActivateTotalWar();
    }
    public void ActivateTotalWar()
    {
        if (TotalWar)
        {
            var bases = FindObjectsOfType<AgentBase>();
            foreach(var b in bases)
            {
                b.totalWar = true;
            }

            var agents = FindObjectsOfType<AgentNpc>();
            foreach(var a in agents)
            {
                a.gameObject.GetComponent<BehaviorTree>();

                a.enabled = true;
                a.SetColorTotalWar();
            }
        }
    }
}
