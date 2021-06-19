using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehaviour
{
   
    //Lista de potenciales objetivos de colision;
    public Agent[] colisiones;
    private float tiempoMasCerca;

    private void Start()
    {
        grupo = Grupo.COLISIONES;
    }

    public override Steering GetSteering(AgentNPC miAgente)
    {
        colisiones = GameObject.FindObjectsOfType<Agent>();
        tiempoMasCerca = Mathf.Infinity;
        Steering steering = new Steering(0, new Vector3(0,0,20));

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

        Agent firstTarget = ColisionMasCercana();
        Vector3 firstRelativePos = firstTarget.transform.position - miAgente.transform.position;
        float firstDistance = firstRelativePos.magnitude;
        Vector3 firstRelativeVel = firstTarget.vVelocidad - miAgente.vVelocidad;
        var timeToCollision = Vector3.Dot(firstRelativePos, firstRelativeVel) / (firstRelativeVel.magnitude * firstRelativeVel.magnitude);
        float firstMinSeparation = firstDistance - firstRelativeVel.magnitude * timeToCollision;
        firstTarget = null;
 
        foreach(Agent obj in colisiones)
        {
            relativePos = obj.transform.position - miAgente.transform.position;
            Vector3 relativeVel = obj.vVelocidad - miAgente.vVelocidad;
            var relativeSpeed = relativeVel.magnitude;
            timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            float distancia = relativePos.magnitude;
            var minSeparacion = distancia - relativeSpeed * timeToCollision;

            if (minSeparacion > 2 * miAgente.rExterior) continue;

            if(timeToCollision > 0 && timeToCollision < tiempoMasCerca)
            {
                Debug.Log("Nuevo Objetivo" + obj.name);
                tiempoMasCerca = timeToCollision;
                firstTarget = obj;
                firstMinSeparation = minSeparacion;
                firstDistance = distancia;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }
        //Parte 2: Calculamos el steering
        if(firstTarget == null)
        {
            return steering;
        }

        if(firstMinSeparation <= 0 || firstDistance < 2 * miAgente.rExterior)
        {
            Debug.Log("Choque");
            relativePos = firstTarget.transform.position - miAgente.transform.position;
        }
        else
        {
            Debug.Log("No Choque");
            relativePos = firstRelativePos + firstRelativeVel * tiempoMasCerca;
        }

        relativePos = relativePos.normalized;
        steering.lineal = relativePos * miAgente.mAceleracion;
        return steering;

    }

    private Agent ColisionMasCercana()
    {
        float distance = Mathf.Infinity;
        Agent aux = null;
        foreach(Agent g in colisiones)
        {
            float d = Vector3.Distance(g.transform.position,gameObject.transform.position);
            if (distance > d)
            {
                distance = d;
                aux = g;
            }
        }
        return aux;
    }
}
