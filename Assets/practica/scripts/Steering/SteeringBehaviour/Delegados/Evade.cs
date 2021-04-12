using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Evade : SteeringBehaviour
{

    public float maxPrediction;

    public override Steering GetSteering(AgentNPC miAgente)
    {
        // Vamosa  crear un nuevo target en la posicion donde estaria nuestro target
        Vector3 direction = target.transform.position - miAgente.transform.position;
        float distance = direction.magnitude;

        // Current Speed
        float speed = miAgente.vVelocidad.magnitude;

        // Si la velocidad es muy pequeña vamos a darle un predicion 
        float prediction = (speed <= distance / maxPrediction) ? maxPrediction : distance / speed;

        // NO Puedo usar el target porque va asignado a otro objecto
        //base.target = explicitTarget;
        // base.target.transform.position += explicitTarget.vVelocidad * prediction;


        this.steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration Seek a tope

        Vector3 position = target.transform.position + target.vVelocidad * prediction;


        this.steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration

        steering.lineal = miAgente.transform.position - position;
        steering.lineal.Normalize();
        steering.lineal *= miAgente.mAceleracion;

        steering.angular = 0;



        return steering;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
