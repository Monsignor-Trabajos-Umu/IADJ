using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arquero : AgentNpc
{
    private Chebychev heuristica;
    protected override void Start()
    {
        base.Start();
        vida = 50;
        alcance = 2;
        daño = 10;
        heuristica = gameObject.AddComponent(typeof(Chebychev)) as Chebychev;
    }



    // Start is called before the first frame update
    public Heuristic GetHeuristic() => heuristica;
}
