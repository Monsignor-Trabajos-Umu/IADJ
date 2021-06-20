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
        Vector3 position = this.usePredicted ? this.predictedDirection : target.transform.position;
        steering.lineal = miAgente.transform.position - position;
        steering.lineal.Normalize();
        steering.lineal *= miAgente.mAcceleration;

        steering.angular = 0;

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
