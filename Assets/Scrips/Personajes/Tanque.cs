using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanque : AgentNpc
{

    // Start is called before the first frame update


    protected override void Start()
    {
        base.Start();
        //Moviemento

        mAcceleration = 4;
        baseVelocity = 20;
        mAngularAcceleration = 45;
        mRotation = 90;
        peorTerreno = 4;
        mejorTerreno = 2;
        vida = vidaMaxima;
        alcance = 3;

        damage = 5;
        defensa = 2;
        //if (actuator == null)
        //    actuator = gameObject.AddComponent(typeof(TankActuator)) as TankActuator;
        heuristic = gameObject.AddComponent(typeof(Manhattan)) as Manhattan;
    }

    protected internal override void Atacar(Agent objetivo)
    {
        if (atacando) return;
        //Nos acercamos al objetivo hasta estar a el número de casillas necesarias

        //Lanzamos el ataque

        //Nos quedamos quietos durante un espacio de tiempo por haber atacado. \

        //Resteamos el estado si lo habia
        if(debug) Debug.Log($"Ataco a {objetivo.name}");
        ChangeState(State.Action);
        ChangeAction(CAction.AttackEnemy);


        var dBase = BestTerrain() ? damage * 1.5 : damage;
        
        if (Random.value <= 0.1) dBase *= 2;

        var realDamage = dBase ;

        var paticles = objetivo.transform.Find("TankExplosion").gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(base.WaitBeforeAttack(1, realDamage,objetivo,paticles));
    }

    public override Heuristic GetHeuristic() => heuristic;
}
