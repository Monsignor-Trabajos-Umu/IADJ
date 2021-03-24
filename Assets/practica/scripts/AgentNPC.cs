using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Steering
{
    public float angular;
    public Vector3 linear;

    public Steering(float newAngular, Vector3 newLinear)
    {
        angular = newAngular;
        linear = newLinear;
    }
}


public abstract class AgentNPC : Agent
{
    public Steering miSteering;
    public SteeringBehaviour[] listSteerings;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplySteering();
    }

    private void Awake()
    {
        //usar GetComponents<>() para cargar los SteeringBehavior del personaje
        listSteerings = GetComponents<SteeringBehaviour>();
        foreach(SteeringBehaviour str in listSteerings)
        {
            str.enabled = true;
        }
    }
     
    private void LateUpdate()
    {
        //Recorre la lista construida en Awake() y calcula los Steering de los SteeringBehaviour

    }

    public abstract void ApplySteering();
}
