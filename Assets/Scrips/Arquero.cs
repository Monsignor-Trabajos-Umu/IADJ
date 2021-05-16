using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arquero : AgentNPC
{
    public Chebychev heuristica = new Chebychev();
    
    public Arquero() : base()
    {
        vida = 50;
        alcance = 2;
        daño = 10;
    }
    // Start is called before the first frame update
    public Heuristic GetHeuristic()
    {
        return heuristica;
    }
}
