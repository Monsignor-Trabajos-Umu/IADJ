using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    public Agent target;
    //Peso del SteeringBehaviour
    public float weight = 1f;
    // Usamos estas dos variables para evitarnos modificar el transform
    protected Vector3 predictedPosition;
    protected float preditedRotation = 0f;
    protected bool usePredicted = false;
    public Steering steering;
    [SerializeField]
    protected bool debug = false;
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


    protected Steering returnDebuged(Color c)
    {
        if (debug)
            Debug.DrawRay(transform.position, steering.lineal, c);
        return this.steering;
    }

}
