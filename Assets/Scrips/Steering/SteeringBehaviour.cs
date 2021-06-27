using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    // Usamos estas dos variables para evitarnos modificar el transform
    protected Vector3 customDirection;
    protected float customRotation;

    [SerializeField] protected bool debug = false;

    public Steering steering;

    // Peso y grupo para los árbitros
    public SteeringGroup steeringGroup = SteeringGroup.Pursuit;


    public Agent target; // Target si existe
    protected bool useCustom;
    public bool useReferee; // Flag para que mi arbitros lo tengan en cuenta
    public float weight = 1f;


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