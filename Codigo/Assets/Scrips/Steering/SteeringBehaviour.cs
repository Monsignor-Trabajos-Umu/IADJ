using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    // Usamos estas dos variables para evitarnos modificar el transform
    protected Vector3 customDirection;
    protected float customRotation;
    [Header("Debug")]
    [SerializeField] protected bool debug = false;

    [Header("Steerings")]
    public Steering steering;

    // Peso y grupo para los árbitros
    [Header("Arbitros")]
    public SteeringGroup steeringGroup = SteeringGroup.Pursuit;
    public float weight = 1f;
    public Agent target; // Target si existe
    public bool useReferee; // Flag para que mi arbitros lo tengan en cuenta


    protected bool useCustom;


    //Calcula el Steering para el agente dado en funcion del comportamiento deseado
    public abstract Steering GetSteering(AgentNpc agent);

    public void UseCustomDirectionAndRotation(Vector3 predictedDirection,
        float predictedRotation = 0f)
    {
        useCustom = true;
        customDirection = predictedDirection;
        customRotation = predictedRotation;
    }

    public void UseCustomRotation(float predictedRotation)
    {
        useCustom = true;
        customRotation = predictedRotation;
    }

    protected virtual void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, steering.lineal);
    }

    // 
    protected static Vector3 RemoveY(Vector3 steering)
    {
        steering.y = 0;
        return steering;
    }
}