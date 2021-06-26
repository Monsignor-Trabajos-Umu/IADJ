using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanque : AgentNpc
{
    private Euclidea heuristica;
    // Start is called before the first frame update


    protected override void Start()
    {
        base.Start();
        vida = 250;
        alcance = 1;
        daño = 5;
        if (actuator == null)
            actuator = gameObject.AddComponent(typeof(TankActuator)) as TankActuator;
        heuristica = gameObject.AddComponent(typeof(Euclidea)) as Euclidea;
    }

    public Heuristic GetHeuristic() => heuristica;
}
