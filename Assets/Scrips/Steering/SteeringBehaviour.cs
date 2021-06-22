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

    public bool useReferee; // Flag para que mi arbitros lo tengan en cuenta

    public Grupo grupo; // Grupo para el arbitro grupal
    public Agent target; // Target si existe

    //Peso del SteeringBehaviour
    public float weight = 1f;
    // Usamos estas dos variables para evitarnos modificar el transform
    protected Vector3 customDirection;
    protected float customRotation = 0f;
    protected bool useCustom = false;
    public Steering steering;
    [SerializeField]
    protected bool debug = false;

   
    
    //Calcula el Steering para el agente dado en funcion del comportamiento deseado
    public abstract Steering GetSteering(AgentNPC agent);


    private void Awake()
    {
        grupo = Grupo.PERSECUCION;
    }
    public void UseCustomDirectionAndRotation(Vector3 predictedDirection,float predictedRotation =0f)
    {
        this.useCustom = true;
        this.customDirection = predictedDirection;
        this.customRotation = predictedRotation;
    }
    public void UseCustomRotation(float predictedRotation)
    {
        this.useCustom = true;
        this.customRotation = predictedRotation;
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.DrawRay(transform.position, steering.lineal);
    }

    // 
    protected static Vector3 RemoveY(Vector3 steering)
    {
        steering.y = 0;
        return steering;
    }
}
