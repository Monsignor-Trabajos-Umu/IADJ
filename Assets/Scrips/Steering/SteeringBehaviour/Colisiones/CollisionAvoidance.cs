using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehaviour
{

    //Lista de potenciales objetivos de colision;
    public Agent[] colisiones;
    private float tiempoMasCerca;

    public override Steering GetSteering(AgentNPC miAgente)
    {
        colisiones = GameObject.FindObjectsOfType<Agent>();
        tiempoMasCerca = Mathf.Infinity;
        Steering steering = new Steering(0, new Vector3(0,0,0));

        if (colisiones.Length == 0) return steering;
        /*
         * TiempoMasCerca= - (posicionRelativaTarget*TargetVelocidadRelativa)/|TargetVelocidadRelativa|^2
         * TargetVelocidadRelativa = vt - vc
         * posicionRelativaTarget = pt - pc
         * 
         * Si TiempoMasCerca es negativo entonces el personaje ya se aparta del objetivo y no se 
         * necesita hacer ninguna acción
         * 
         * posicionPersonajeColision = posicionPersonaje + velocidadPersonaje*TiempoMasCerca
         * posicionObjetivoColision = posicionObjetivo + velocidadObjetivo*TiempoMasCerca
         */
        Vector3 relativePos;

        Agent objetivo = null;

        Vector3 firstRelativePos = colisiones[0].transform.position - miAgente.transform.position;
        float firstDistance = firstRelativePos.magnitude;
        Vector3 firstRelativeVel = colisiones[0].vAceleracion - miAgente.vAceleracion;
        var timeToCollision = Vector3.Dot(firstRelativePos, firstRelativeVel) / (firstRelativeVel.magnitude * firstRelativeVel.magnitude);
        float firstMinSeparation = firstDistance - firstRelativeVel.magnitude * timeToCollision;

 
        foreach(Agent obj in colisiones)
        {
            relativePos = obj.transform.position - miAgente.transform.position;
            Vector3 relativeVel = obj.vAceleracion - miAgente.vAceleracion;
            var relativeSpeed = relativeVel.magnitude;
            timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            float distancia = relativePos.magnitude;
            var minSeparacion = distancia - relativeSpeed * timeToCollision;

            if (minSeparacion > 2 * miAgente.rExterior) continue;

            if(timeToCollision > 0 && timeToCollision < tiempoMasCerca)
            {
                Debug.Log("Nuevo Objetivo" + obj.name);
                tiempoMasCerca = timeToCollision;
                objetivo = obj;
                firstMinSeparation = minSeparacion;
                firstDistance = distancia;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }
        
        if(objetivo == null)
        {
            return steering;
        }

        if(firstMinSeparation <= 0 || firstDistance < 2 * miAgente.rExterior)
        {
            relativePos = objetivo.transform.position - miAgente.transform.position;
        }
        else
        {
            relativePos = firstRelativePos + firstRelativeVel * tiempoMasCerca;
        }

        relativePos = relativePos.normalized;
        steering.lineal = relativePos * miAgente.mAceleracion;
        return steering;

    }

}
