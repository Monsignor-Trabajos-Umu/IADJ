using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AgentNPC : Agent
{
    public Steering miSteering;

    public BlenderSteering arbitro;
    public GoTarget goToTarget;
    bool targetExist;
    private Steering goToTargetSteering;


    // Update is called once per frame
    void Update()
    {
        ApplySteering();
    }

    private void Awake()
    {
        //usar GetComponents<>() para cargar el arbitro del personaje
        arbitro = GetComponent<BlenderSteering>();
        // El go to target se salta todos los arbitros
        goToTarget = GetComponent<GoTarget>();
        targetExist = false;
        miSteering = new Steering(0, new Vector3(0, 0, 0));
        goToTargetSteering = new Steering(0, new Vector3(0, 0, 0));
    }

    private void LateUpdate()
    {
        //Pide el steering a Agente y si hay un target usa ese steering
        if (goToTarget != null)
        {
            this.targetExist = goToTarget.targetExists;
            this.goToTargetSteering = goToTarget.GetSteering(this);
        }
        miSteering = arbitro.GetSteering();

    }

    public void ApplySteering()
    {
        if (!targetExist)
            updateAcelerated(miSteering, Time.deltaTime);
        else
            updateNoAcelerated(goToTargetSteering, Time.deltaTime);
    }


    private void updateAcelerated(Steering steering, float time)
    {

        if (Vector3.Distance(steering.lineal, new Vector3(0, 0, 0)) == 0)
        {
            this.vVelocidad = new Vector3(0, 0, 0);
        }
        if (steering.angular == 0)
        {
            this.rotacion = 0;
        }


        //Debug.DrawRay(transform.position, this.vVelocidad, Color.white);
        transform.position = transform.position + this.vVelocidad * time;
        this.orientacion = this.orientacion + this.rotacion * time;

        this.vVelocidad = this.vVelocidad + steering.lineal * time;

        // Si vamos mas rapido que la velicidad maxima reducimos
        if (this.vVelocidad.magnitude > this.mVelocidad)
        {
            this.vVelocidad.Normalize();
            this.vVelocidad *= this.mVelocidad;
        }

        this.vAceleracion = steering.lineal;

        if (this.vAceleracion.magnitude > this.mAceleracion)
        {
            this.vAceleracion.Normalize();
            this.vAceleracion *= this.mAceleracion;
        }
        //TODO
        this.rotacion = this.rotacion + steering.angular * time;



    }

    private void updateNoAcelerated(Steering steering, float time)
    {
        transform.position = transform.position + steering.velocidad * time;
        this.orientacion = this.orientacion + steering.rotacion * time;

    }


}
