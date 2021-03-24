using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    public Agent target;
    public Steering miSteering;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Calcula el Steering para el agente dado en funcion del comportamiento deseado
    public abstract Steering GetSteering(AgentNPC agent);
}
