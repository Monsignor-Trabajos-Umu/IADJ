using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldado : AgentNpc
{
   
    protected override void Start()
    {
        base.Start();
        // Movimiento
        peorTerreno = 0;    
        mejorTerreno = 3;
        mAcceleration = 4;
        baseVelocity = 20;
        mAngularAcceleration = 45;
        mRotation = 90;


        vida = 100;
        alcance = 1; // ataca 2 bloques alrededor
        daño = 20;


        heuristic = gameObject.AddComponent(typeof(Manhattan)) as Manhattan;
    }
    public new void ArrivedToTarget()
    {
        ChangeAction(CAction.None);
        ChangeState(State.Waiting);


    } 

    public override Heuristic GetHeuristic() => heuristic;


}


