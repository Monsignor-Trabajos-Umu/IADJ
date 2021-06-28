using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Actions;
using UnityEngine;

public class AgentBase : Agent
{
    public Modo modo = Modo.Ataque;

    public bool IsDefending() => modo == Modo.Defensa;
    public bool IsAttacking() => modo == Modo.Ataque;
    public bool IsTotalWar() => modo == Modo.TotalWar;

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
        damage = 0;

        //Debug.Log(modo);
    }
    public void Dead(AgentNpc npc, float respawnTime) 
        => StartCoroutine(Respawn(npc, respawnTime));
    private IEnumerator Respawn(AgentNpc obj, float respawnTime)
    {
        obj.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(respawnTime);
        GameObject cuartel;
        if (tag == "equipoRojo")
            cuartel = GameObject.FindWithTag("baseRoja");
        else
            cuartel = GameObject.FindWithTag("baseAzul");

        //Hacemos que spawnee al lado de su base
        obj.transform.position =
            cuartel.transform.position + new Vector3(0, 0, -50);
        //Hacemos que vuelva a ser visible    
        obj.gameObject.SetActive(true);
        //Recuperamos su vida
        obj.vida = obj.vidaMaxima;
        obj.ResetStateAndSteering(); // Por si acaso
    }
}