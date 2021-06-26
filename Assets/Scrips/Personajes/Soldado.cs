using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldado : AgentNPC
{
    private Manhattan heuristica;
    protected override void Start()
    {
        base.Start();
        // Movimiento
        mAcceleration = 4;
        mVelocity = baseVelocity =  5;

        mAngularAcceleration = 45;
        mRotation = 90;


        vida = 100;
        alcance = 1; // ataaa un bloque al rededor
        daño = 20;



        heuristica = new Manhattan();
    }

    public Heuristic GetHeuristic() => heuristica;
}