using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Grupo{
    COLISIONES = 0,
    PERSECUCION = 1,
    SEPARACION = 2
};

public abstract class SteeringBehaviour : MonoBehaviour
{
    public Grupo grupo;
    public Agent target;
    //Peso del SteeringBehaviour
    public float weight = 1f;
    // Usamos estas dos variables para evitarnos modificar el transform
    protected Vector3 predictedDirection;
    protected float predictedRotation = 0f;
    protected bool usePredicted = false;
    public Steering steering;
    [SerializeField]
    protected bool debug = false;
    
    //Calcula el Steering para el agente dado en funcion del comportamiento deseado
    public abstract Steering GetSteering(AgentNPC agent);


    private void Awake()
    {
        grupo = Grupo.PERSECUCION;
    }
    public void UsePredicted(Vector3 predictedDirection,float predictedRotation =0f)
    {
        this.usePredicted = true;
        this.predictedDirection = predictedDirection;
        this.predictedRotation = predictedRotation;
    }
    public void UsePredicted(float predictedRotation)
    {
        this.usePredicted = true;
        this.predictedRotation = predictedRotation;
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.DrawRay(transform.position, steering.lineal);
    }
}
