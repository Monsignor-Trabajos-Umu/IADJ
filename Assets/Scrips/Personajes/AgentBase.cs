using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Actions;
using UnityEngine;

public class AgentBase : Agent
{
    public Modo modo = Modo.Ataque;

    public bool IsAttacking() => modo == Modo.Ataque;
    protected override void Start()
    {
        // No hay movimiento

        mAcceleration = 0;
        mVelocity = baseVelocity = 0;

        mAngularAcceleration = 0;
        mRotation = 0;

        vida = 300;
        vidaMaxima = 300;
        alcance = 0;
        da�o = 0;

        Debug.Log(modo);



    }
}