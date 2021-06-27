using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scrips.Steering.Pathfinding.A;
using UnityEngine;

public abstract class ArbitroSteering : MonoBehaviour
{
    protected AgentNpc agent; //Mi agenteNPC que llama al getSteering
  

    [SerializeField] protected bool debug = false; //Debug flag

    [SerializeField] protected Steering finalSteering;

    protected FormationOffset formationOffset; // Me dice mi posicion si es formacion

    // SteeringBehaviour necesarios
    private GoTarget goToTarget; // Go to target
    [SerializeField] protected ASteering aSteering; // Go to target a*

    private Dictionary<SteeringBehaviour, float> savedWeightDictionary;

    [SerializeField]
    protected List<SteeringBehaviour> steeringList; // Lista con todos los steering

    // Usamos Awake para crer los steering necesarios antes de cualquier Start
    protected virtual void Awake()
    {
        agent = GetComponent<AgentNpc>();
        // Creamos los steering necesarios
        formationOffset = gameObject.AddComponent<FormationOffset>();
        goToTarget = gameObject.AddComponent<GoTarget>();
    

        // Añadimos los Steering a la lista
        steeringList = new List<SteeringBehaviour> {formationOffset};
        savedWeightDictionary = new Dictionary<SteeringBehaviour, float>();

        //usar GetComponents<>() para cargar los SteeringBehavior del personaje
        steeringList.AddRange(GetComponents<SteeringBehaviour>()
            .Where(behaviour => behaviour.useReferee));
        foreach (var str in steeringList) str.enabled = true;
    }


    public void SetFormation(Formation newFormation)
    {
        // Si hay mas steering no le hacemos caso menos los de colision
        steeringList.ForEach(behaviour =>
        {
            if (behaviour.steeringGroup != SteeringGroup.Formation &&
                behaviour.steeringGroup != SteeringGroup.Collision)
            {
                savedWeightDictionary[behaviour] = behaviour.weight;
                behaviour.weight = 0;
            }

            ;
        });
        formationOffset.SetFormation(newFormation);
    }

    public void RestoreWeight()
    {
        foreach (var keyValuePair in savedWeightDictionary)
            keyValuePair.Key.weight = keyValuePair.Value;
    }

    // Le digo a mi steering goToTarget que tenemos que ir a un objetivo
    public void SetNewTarget(Vector3 newPoint) => goToTarget.NewTarget(newPoint);

    private Steering GetGoToTargetSteering() => goToTarget.GetSteering(agent);

    #region Acciones arboles y segunda parte

    public void SetNewTargetWithA(int _nodes, Vector3 origen, Vector3 target,double radioExterior,
        Heuristic heuristic,Func<bool> checkCloser) => aSteering.StartMoving(_nodes, origen, target,radioExterior, heuristic,checkCloser);


    public void CancelSteeringAction(CAction action)
    {
        switch (action)
        {
            case CAction.None:
            case CAction.GoToTarget: // No se van a dar no hago nada
                break;
            case CAction.GoingToEnemy: // Estoy haciendo un aSteering
            case CAction.Retreat:
                aSteering.CancelPath();
                break;
            case CAction.Defend:
                break;
            case CAction.Forming:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    #endregion


    protected abstract Steering GetSteering();

    public Steering GetFinalSteering(State state, CAction cAction)
    {
        if (state == State.Action && cAction == CAction.GoToTarget)
            return GetGoToTargetSteering();
        return GetSteering();
    }


    protected static Steering FilterYFromSteering(Steering steering)
    {
        steering.lineal.y = 0;
        return steering;
    }


    // Quitamos decimales al float
    protected static Steering RoundSteering(Steering steering)
    {
        //Debug.Log($"PreFiltre {steering.lineal} {steering.angular}");
        // No tiene sentido 0,0000000001 de aceleracion
        steering.lineal.x = (float) Math.Round(steering.lineal.x, 8);
        steering.lineal.z = (float) Math.Round(steering.lineal.z, 8);

        // Para los angulos con tres decimales es mas que suficiente
        steering.angular = (float) Math.Round(steering.angular, 3);
        //Debug.Log($"Post {steering.lineal} {steering.angular}");
        return steering;
    }


    protected virtual void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, finalSteering.lineal);
    }
}