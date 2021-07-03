using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arquero : AgentNpc
{
    protected override void Start()
    {
        base.Start();
        peorTerreno = 3;
        mejorTerreno = 4;
        vida = vidaMaxima;
        alcance = 4;
        damage = 10;
        defensa = 1;
        heuristic = gameObject.AddComponent(typeof(Chebychev)) as Chebychev;
    }

    protected internal override void Atacar(Agent objetivo)
    {
        if (atacando) return;
        if (debug) Debug.Log($"Ataco a {objetivo.name}");
        ChangeState(State.Action);
        ChangeAction(CAction.AttackEnemy);


        var dBase = BestTerrain() ? damage * 2.5 : damage;

        if (Random.value <= 0.15) dBase *= 2;

        var realDamage = dBase;

        var paticles = objetivo.transform.Find("ShellExplosion").gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(base.WaitBeforeAttack(2, realDamage, objetivo, paticles));
    }


    // Start is called before the first frame update
    public override Heuristic GetHeuristic() => heuristic;
}
