using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanque : AgentNPC
{
    public Euclidea heuristica = new Euclidea();
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        vida = 250;
        alcance = 1;
        daño = 5;
    }

    public Heuristic GetHeuristic() => heuristica;
}
