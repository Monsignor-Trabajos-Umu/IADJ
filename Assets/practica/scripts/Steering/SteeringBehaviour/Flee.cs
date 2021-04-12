using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Flee : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration

        steering.lineal = miAgente.transform.position - target.transform.position;
        steering.lineal.Normalize();
        steering.lineal *= miAgente.mAceleracion;

        steering.angular = 0;

        /* Anti Aling ?
            double angle = miAgente.MinAngleToRotate(target.gameObject);


            if (Math.Abs(angle) >= Math.Abs(miAgente.AExterior))
            {
                this.steering.angular = (float)angle -180;
            }
        */
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
