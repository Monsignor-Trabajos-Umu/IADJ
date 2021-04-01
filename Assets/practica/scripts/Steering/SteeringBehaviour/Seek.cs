using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Seek : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration
        float distancia = Vector3.Distance(miAgente.transform.position, target.transform.position);
        if (distancia > miAgente.rExterior)
        {

            steering.lineal = target.transform.position - miAgente.transform.position;
            steering.lineal.Normalize();
            steering.lineal *= miAgente.mAceleracion;
        }
        steering.angular = 0;
      
        double angle = miAgente.MinAngleToRotate(target.gameObject);


        if (Math.Abs(angle) >= Math.Abs(miAgente.AExterior))
        {
            this.steering.angular = (float)angle;
        }
        return this.steering;
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
