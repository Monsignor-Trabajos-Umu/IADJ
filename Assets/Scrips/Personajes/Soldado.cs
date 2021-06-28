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
        vidaMaxima = 100;
        alcance = 1; // ataca 2 bloques alrededor
        damage = 20;


        heuristic = gameObject.AddComponent(typeof(Euclidea)) as Euclidea;
    }

    public override Heuristic GetHeuristic() => heuristic;

    protected internal override void Atacar(Agent objetivo)
    {
        if (atacando) return;
        //Nos acercamos al objetivo hasta estar a el número de casillas necesarias

        //Lanzamos el ataque

        //Nos quedamos quietos durante un espacio de tiempo por haber atacado. \

        //Resteamos el estado si lo habia
        Debug.Log($"Ataco a {objetivo.name}");
        ChangeState(State.Action);
        ChangeAction(CAction.AttackEnemy);


        double dBase = BestTerrain() ? damage * 2 : damage;

        if (Random.value <= 0.1) dBase *= 2;

        var paticles = objetivo.transform.Find("ShellExplosion").gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(base.WaitBeforeAttack(1, dBase,objetivo,paticles));
    }


}


