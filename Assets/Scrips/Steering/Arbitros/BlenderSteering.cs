using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BehaviorAndWeight
{
    public Steering behavior;
    public readonly float weight;

    public BehaviorAndWeight(Steering behavior, float weight)
    {
        this.behavior = behavior;
        this.weight = weight;
    }
}

public class BlenderSteering : ArbitroSteering
{
    [SerializeField] private List<BehaviorAndWeight> behaviors;


    [SerializeField] private float weightFormationOffset=1;


    protected override void Awake()
    {
        base.Awake();
        behaviors = new List<BehaviorAndWeight>();

        // Usamos pesos para para el BlenderSteering tienen que ser un peso bastante grande
        formationOffset.weight = weightFormationOffset;
    }

    private void LateUpdate()
    {
        //Recorre la lista construida en Awake() y calcula los Steering de los SteeringBehaviour
        behaviors.Clear();
        foreach (var str in steeringList)
        {
            var temp = str.GetSteering(agent);
            behaviors.Add(new BehaviorAndWeight(temp, str.weight));
        }
    }


    protected override Steering GetSteering()
    {
        finalSteering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration Seek a tope

        foreach (var behavior in behaviors)
        {
            finalSteering.angular += behavior.weight * behavior.behavior.angular;
            finalSteering.lineal += behavior.weight * behavior.behavior.lineal;
        }

        // Técnicamente no debería de haber una y pero quien sabe
        finalSteering = FilterYFromSteering(finalSteering);

        // Puede que Vayamos demasido rapido
        finalSteering.lineal = finalSteering.lineal.magnitude > agent.mAcceleration
            ? finalSteering.lineal.normalized * agent.mAcceleration
            : finalSteering.lineal;

        
        // Si rotamos muy rápido la normalizamos
        var angularAcceleration = Math.Abs(finalSteering.angular);
        if (angularAcceleration > agent.mAngularAcceleration)
        {
            finalSteering.angular /= angularAcceleration;
            finalSteering.angular *= agent.mAngularAcceleration;
        }
        finalSteering.angular = (float)Math.Floor(finalSteering.angular);


        finalSteering = RoundSteering(finalSteering);
        return finalSteering;
    }



}