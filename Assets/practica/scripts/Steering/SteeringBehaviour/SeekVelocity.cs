using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeekVelocity : SteeringBehaviour
{
    public override Steering GetSteering(AgentNPC miAgente)
    {
        this.steering = new Steering(0, new Vector3(0, 0, 0));

        Vector3 myPosition = miAgente.transform.position;
        Vector3 targetPosition = this.target.transform.position;

        float distancia = Vector3.Distance(myPosition, targetPosition);

        if (distancia > miAgente.rExterior)
            this.steering.velocidad = Vector3.ClampMagnitude(targetPosition - myPosition,
                miAgente.vMaxima);
        double angle = miAgente.MinAngleToRotate(target.gameObject);


        if (Math.Abs(angle) >= Math.Abs(miAgente.AExterior))
        {
            Debug.Log("Angulo " + angle);
            this.steering.angulo = (float)angle;
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
