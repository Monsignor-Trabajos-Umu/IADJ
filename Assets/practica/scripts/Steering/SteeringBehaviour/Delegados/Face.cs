using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Face : Aling
{
    // Quiero que mi agente mire hacia delante
    [Range(0.0f, 10.0f)]
    public float maxPrediction;

    public override Steering GetSteering(AgentNPC miAgente)
    {
        steering = new Steering(0, new Vector3(0, 0, 0));
        // Vamosa  crear un nuevo target en la posicion donde estaria nuestro target
        Vector3 direction = target.transform.position - miAgente.transform.position;
        if (Math.Round(direction.magnitude) == 0)
            return steering;

        // TODO
        //this.preditedOrientation = (float)Math.Atan2(-direction.x, direction.z);
        this.usePredicted = true;




        return base.GetSteering(miAgente);

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
