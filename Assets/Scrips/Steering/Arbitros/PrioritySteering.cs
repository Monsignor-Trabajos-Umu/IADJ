using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PrioritySteering : ArbitroSteering
{
    List<SteeringBehaviour> listSteerings;
    List<SteeringBehaviour> listColisiones;
    List<SteeringBehaviour> listSeparacion;
    List<SteeringBehaviour> listPersecucion;

    Dictionary<Grupo, Steering> dicGrupos;

    [SerializeField]
    public bool ordenMayor;

    double epsilon = 0.05;
    AgentNPC agente;
    [SerializeField]
    Steering debugSteering;

    [SerializeField]
    public Grupo primeraOpcion = Grupo.COLISIONES;
    [SerializeField]
    public Grupo segundaOpcion = Grupo.PERSECUCION;
    private void Awake()
    {
        dicGrupos = new Dictionary<Grupo, Steering>();

        //Lista de steerings
        listSteerings = new List<SteeringBehaviour>();
        listColisiones = new List<SteeringBehaviour>();
        listSeparacion = new List<SteeringBehaviour>();
        listPersecucion = new List<SteeringBehaviour>();
        //Lista de steerings con sus prioridades

        agente = GetComponent<AgentNPC>();
        //usar GetComponents<>() para cargar los SteeringBehavior del personaje
        listSteerings = GetComponents<SteeringBehaviour>().ToList();
        foreach (SteeringBehaviour str in listSteerings)
        {
            str.enabled = true;
            switch (str.grupo)
            {
                case Grupo.COLISIONES:
                    listColisiones.Add(str);
                    break;
                case Grupo.SEPARACION:
                    listSeparacion.Add(str);
                    break;
                default:
                    listPersecucion.Add(str);
                    break;
            }
        }

    }

    private void LateUpdate()
    {
        Steering aux = BlenderCalc(listColisiones);
        dicGrupos.Add(Grupo.COLISIONES, aux);

        aux = BlenderCalc(listPersecucion);
        dicGrupos.Add(Grupo.PERSECUCION, aux);

        aux = BlenderCalc(listSeparacion);
        dicGrupos.Add(Grupo.SEPARACION, aux);
    }



    public override Steering GetSteering()
    {
        Steering steering = new Steering(0, new Vector3(0, 0, 0));

        IEnumerable<Steering> listas = new List<Steering>();
        if (ordenMayor)
        {
            listas = from pair in dicGrupos
                         orderby pair.Key descending
                         select pair.Value;
        }
        else
        {
            listas = from pair in dicGrupos
                         orderby pair.Key ascending
                         select pair.Value;
        }

        foreach (var grupo in listas){
            steering = grupo;
            if (grupo.lineal.magnitude > epsilon || Math.Abs(grupo.angular) > epsilon)
                return steering;
        }
        return steering;
    }



    Steering BlenderCalc(List<SteeringBehaviour> steerings)
    {
        List<BehaviorAndWeight> aux = new List<BehaviorAndWeight>();

        foreach (SteeringBehaviour str in steerings)
        {
            if (str.enabled)
            {
                Steering temp = str.GetSteering(agente);
                aux.Add(new BehaviorAndWeight(temp, str.weight));
            }
        }
        Steering resultado = Blender(aux);
        return resultado;
    }


    public Steering Blender(List<BehaviorAndWeight> behaviors)
    {
        Steering steering = new Steering(0, new Vector3(0, 0, 0));

        // Full Aceleration Seek a tope

        foreach (BehaviorAndWeight behavior in behaviors)
        {
            steering.angular += behavior.weight * behavior.behavior.angular;
            steering.lineal += behavior.weight * behavior.behavior.lineal;
        }
        steering.lineal = (steering.lineal.magnitude > agente.mAceleracion) ? steering.lineal.normalized * agente.mAceleracion : steering.lineal;
        steering.angular = (steering.angular > agente.mAngularAceleracion) ? (steering.angular * agente.mAngularAceleracion) / Math.Abs(steering.angular) : steering.angular;

        steering = filtroSteering(steering);

        return steering;
    }

    private Steering filtroSteering(Steering steering)
    {
        //Eliminamos el steering eje y
        steering.lineal.y = 0;

        // No tiene sentido 0,000001 de aceleracion
        steering.lineal.x  = (float)(Math.Round(steering.lineal.x, 4));
        steering.lineal.z  = (float)(Math.Round(steering.lineal.z, 4));

        // Para los angulos con dos decimales es mas que suficiente
        steering.angular  = (float)(Math.Round(steering.angular, 2));
        
        return steering;
    }
}
