using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Actions;
using UnityEngine;
using SCT;
using Random = UnityEngine.Random;

public class ScriptableTextHelper : MonoBehaviour
{

    [Header("Bases")] 
    [SerializeField] private GuerraTotal guerraTotal;
    [SerializeField] private AgentBase baseAzul;
    [SerializeField] private AgentBase baseRoja;


    [Header("Randomize On Horizontal Axis")] [SerializeField]
    private Vector2 m_range = new Vector2(-5, 5);
    
    private void OnGUI()
    {
        GUI.Box(new Rect(0, Screen.height - 100, 400, 100), "Helper");


        if (baseAzul.vida <= 0)
        {
            GUI.TextField(
                new Rect(20 + 80 + 50 , Screen.height - 70, 100, 40),"Ha Ganado el rojo");
        }else if (baseRoja.vida <= 0)
        {
            GUI.TextField(
                new Rect(20 + 80 + 50 , Screen.height - 70, 100, 40),"Ha Ganado el azul");

        }

        if(GUI.Button(new Rect(20,Screen.height - 70,80,40),"GuerraTotal"))
        {
            if(baseAzul.modo == Modo.TotalWar || baseRoja.modo == Modo.TotalWar) return;
            guerraTotal.ActivateTotalWar();
        }
        if (GUI.Button(new Rect(20+80+50, Screen.height - 70, 80, 40), "Ataca"))
        {
            baseAzul.modo = Modo.Ataque;
        }
        if(GUI.Button(new Rect(20+80+50+80+50,Screen.height - 70,80,40),"Defiende"))
        {
            baseAzul.modo = Modo.Defensa;
        }

        var mode = baseAzul.modo;
        string salida = mode switch
        {
            Modo.Ataque => "Modo Ataque",
            Modo.Defensa => "Modo Defensa",
            Modo.TotalWar => "Modo Guerra total",
            _ => ""
        };
        GUI.TextField(
            new Rect(20 + 80 + 50 + 80 + 50 + 80 + 50, Screen.height - 70, 80, 40),
            salida);
    }

    
}
