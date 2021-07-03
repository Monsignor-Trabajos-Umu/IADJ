using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SteeringGroup
{
    Collision = 0,
    Formation = 1,
    Pursuit = 2,
    Distance = 3
}

public class PrioritySteering : ArbitroSteering
{
    /** Arbitro usando prioridades por defecto el orden es:
     *
     *  - Collision
     *  - Formation
     *  - Pursuit
     *  - Distance
     *
     *  Calcula el blender steering de cada bloque y luego elige uno por prioridad siendo Collision el preferido
     *  Si el flag de ordenMayor esta activado se voltea la lista
    */
    private const double Epsilon = 0.05;

    [SerializeField] private List<SteeringBehaviour> collisionList;

    [SerializeField] private List<SteeringBehaviour> distanceList;

    [SerializeField] private List<SteeringBehaviour> formationList;

    [SerializeField] private Dictionary<SteeringGroup, Steering> groupDict;

    [SerializeField] private bool ordenMayor;

    [SerializeField] private List<SteeringBehaviour> pursuitList;


    protected override void Awake()
    {
        base.Awake();

        //Lista de Steering
        groupDict = new Dictionary<SteeringGroup, Steering>();
        collisionList = new List<SteeringBehaviour>();
        formationList = new List<SteeringBehaviour>();
        distanceList = new List<SteeringBehaviour>();
        pursuitList = new List<SteeringBehaviour>();
        //Lista de Steering con sus prioridades

        //usar GetComponents<>() para cargar los SteeringBehavior del personaje
        foreach (var str in steeringList)
            switch (str.steeringGroup)
            {
                case SteeringGroup.Collision:
                    collisionList.Add(str);
                    break;
                case SteeringGroup.Formation:
                    formationList.Add(str);
                    break;
                case SteeringGroup.Pursuit:
                    pursuitList.Add(str);
                    break;
                case SteeringGroup.Distance:
                    distanceList.Add(str);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    private void LateUpdate()
    {
        groupDict.Clear();

        var aux = BlenderCalc(collisionList);
        groupDict.Add(SteeringGroup.Collision, aux);

        aux = BlenderCalc(formationList);
        groupDict.Add(SteeringGroup.Formation, aux);

        aux = BlenderCalc(pursuitList);
        groupDict.Add(SteeringGroup.Pursuit, aux);

        aux = BlenderCalc(distanceList);
        groupDict.Add(SteeringGroup.Distance, aux);
    }


    protected override Steering GetSteering()
    {
        finalSteering = new Steering(0, new Vector3(0, 0, 0));

        IEnumerable<Steering> listas;
        if (ordenMayor)
            listas =
                from pair in groupDict
                orderby pair.Key descending
                select pair.Value;
        else
            listas =
                from pair in groupDict
                orderby pair.Key
                select pair.Value;

        foreach (var grupo in listas)
        {
            finalSteering = grupo;
            if (grupo.lineal.magnitude > Epsilon || Math.Abs(grupo.angular) > Epsilon)
                return finalSteering;
        }

        return finalSteering;
    }


    private Steering BlenderCalc(IEnumerable<SteeringBehaviour> steering)
    {
        var aux = new List<BehaviorAndWeight>();

        foreach (var str in steering)
            if (str.enabled)
            {
                var temp = str.GetSteering(agent);
                aux.Add(new BehaviorAndWeight(temp, str.weight));
            }

        var resultado = Blender(aux);
        return resultado;
    }


    private Steering Blender(List<BehaviorAndWeight> behaviors)
    {
        var steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration Seek a tope

        foreach (var behavior in behaviors)
        {
            steering.angular += behavior.weight * behavior.behavior.angular;
            steering.lineal += behavior.weight * behavior.behavior.lineal;
        }

        steering = FilterYFromSteering(steering);
        steering.lineal = steering.lineal.magnitude > agent.mAcceleration
            ? steering.lineal.normalized * agent.mAcceleration
            : steering.lineal;

        // Si rotamos muy rápido la normalizamos
        var angularAcceleration = Math.Abs(steering.angular);
        if (angularAcceleration > agent.mAngularAcceleration)
        {
            steering.angular /= angularAcceleration;
            steering.angular *= agent.mAngularAcceleration;
        }
        steering.angular = (float)Math.Floor(steering.angular);

        steering = RoundSteering(steering);

        return steering;
    }
}