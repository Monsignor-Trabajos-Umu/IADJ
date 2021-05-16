using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldado : AgentNPC
{
    public Manhattan heuristica = new Manhattan();
    // Start is called before the first frame update

    public Soldado() : base()
    {
        vida = 100;
        alcance = 1;
        daño = 20;
    }

    public Heuristic GetHeuristic()
    {
        return heuristica;
    }
}
