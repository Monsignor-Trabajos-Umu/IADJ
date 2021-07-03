using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Actions;
using UnityEngine;
using UniBT;
public class GuerraTotal : MonoBehaviour
{
    public void ActivateTotalWar()
    {
        
            var bases = FindObjectsOfType<AgentBase>();
            foreach(var b in bases)
            {
                b.modo = Modo.TotalWar;
            }

            var agents = FindObjectsOfType<AgentNpc>();
            foreach(var a in agents)
            {
                a.gameObject.GetComponent<BehaviorTree>().enabled = true;
                a.controladoMaquina = true;
                a.SetColorTotalWar();
            }
        
    }
}
