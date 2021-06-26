using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldado : AgentNPC
{
    private Manhattan heuristica;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        vida = 100;
        alcance = 1;
        daño = 20;
        heuristica = new Manhattan();
    }

    public Heuristic GetHeuristic() => heuristica;
}
