using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AgentNPC : Agent
{
    public Steering miSteering;

    List<SteeringBehaviour> listSteerings = new List<SteeringBehaviour>();
    bool targetExist;
    private Steering goToTargetSteering;


    // Update is called once per frame
    void Update()
    {
        ApplySteering();
    }

    private void Awake()
    {
        //usar GetComponents<>() para cargar los SteeringBehavior del personaje
        listSteerings = GetComponents<SteeringBehaviour>().ToList<SteeringBehaviour>();
        foreach (SteeringBehaviour str in listSteerings)
        {
            str.enabled = true;
        }
        targetExist = false;
        miSteering = new Steering(0, new Vector3(0, 0, 0));
        goToTargetSteering = new Steering(0, new Vector3(0, 0, 0));
    }

    private void LateUpdate()
    {

        //Recorre la lista construida en Awake() y calcula los Steering de los SteeringBehaviour

        List<Steering> calculatedStearing = new List<Steering>();
        miSteering = new Steering(0, new Vector3(0, 0, 0));
        goToTargetSteering = new Steering(0, new Vector3(0, 0, 0));
        foreach (SteeringBehaviour str in listSteerings)
        {
            if (str.enabled)
            {
                if (str is GoTarget)
                {
                    GoTarget temp = (GoTarget)str;
                    this.targetExist = temp.targetExists;
                    this.goToTargetSteering = temp.GetSteering(this);

                }
                else
                {
                    Steering temp = str.GetSteering(this);
                    miSteering.lineal += temp.lineal;
                    miSteering.angular += temp.angular;
                }

            }
        }


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

        transform.position = transform.position + this.vVelocidad * time;
        this.orientacion = this.orientacion + this.rotacionn * time;

        this.vVelocidad = this.vVelocidad + steering.lineal * time;
        this.rotacionn = this.rotacionn + steering.angular * time;

    }

    private void updateNoAcelerated(Steering steering, float time)
    {
        transform.position = transform.position + steering.velocidad * time;
        this.orientacion = this.orientacion + steering.rotacion * time;

    }


}
