using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionAvoidance : SteeringBehaviour
{
   
    //Lista de potenciales objetivos de colision;
    public List<Agent> colisiones;
    private float tiempoMasCerca;
    Vector3 relativePos;
    private void Start()
    {
        steeringGroup = SteeringGroup.Collision;
        colisiones = new List<Agent>();
    }

    public override Steering GetSteering(AgentNPC miAgente)
    {
        colisiones = FindObjectsOfType<Agent>().Where(agent =>agent!= miAgente ).ToList();

        tiempoMasCerca = Mathf.Infinity;
        steering = new Steering(0, new Vector3(0,0,0));

        if (colisiones.Count == 0) return steering;
        /*
         * TiempoMasCerca= - (posicionRelativaTarget*TargetVelocidadRelativa)/|TargetVelocidadRelativa|^2
         * TargetVelocidadRelativa = vt - vc
         * posicionRelativaTarget = pt - pc
         * 
         * Si TiempoMasCerca es negativo entonces el personaje ya se aparta del objetivo y no se 
         * necesita hacer ninguna acci�n
         * 
         * posicionPersonajeColision = posicionPersonaje + velocidadPersonaje*TiempoMasCerca
         * posicionObjetivoColision = posicionObjetivo + velocidadObjetivo*TiempoMasCerca
         */
        Vector3 relativePos;

        Agent firstTarget = ColisionMasCercana();
        Vector3 firstRelativePos = firstTarget.transform.position - miAgente.transform.position;
        float firstDistance = firstRelativePos.magnitude;
        Vector3 firstRelativeVel = firstTarget.vVelocidad - miAgente.vVelocidad;
        var timeToCollision = -Vector3.Dot(firstRelativePos, firstRelativeVel) / (firstRelativeVel.magnitude * firstRelativeVel.magnitude);
        float firstMinSeparation = firstDistance - firstRelativeVel.magnitude * timeToCollision;
        firstTarget = null;
 
        foreach(Agent obj in colisiones)
        {
            //Calcular el tiempo de colision
            relativePos = obj.transform.position - miAgente.transform.position;
            Vector3 relativeVel = obj.vVelocidad - miAgente.vVelocidad;
            float relativeSpeed = relativeVel.magnitude;
            timeToCollision = -Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

            //Comprobar si habr� colisi�n
            float distancia = relativePos.magnitude;
            float minSeparacion = distancia - relativeSpeed * timeToCollision;

            if (minSeparacion > 2 * miAgente.RExterior)
                continue;


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

        if(firstMinSeparation <= 0 || firstDistance < 2 * miAgente.RExterior)
        {
            Debug.Log("Choque");
            relativePos = firstTarget.transform.position - miAgente.transform.position;
        }
        else
        {
            Debug.Log("No Choque");
            relativePos = firstRelativePos + firstRelativeVel * tiempoMasCerca;
        }

        Vector3 aux = firstTarget.transform.position + (relativePos - firstTarget.transform.position);
        aux = aux * ((float) firstTarget.RExterior + 2);

        relativePos = aux.normalized;
        steering.lineal = -relativePos * miAgente.mAcceleration;
        //if(debug) Debug.DrawLine(this.transform.position, relativePos, Color.cyan);
        //Debug.DrawRay(this.transform.position, steering.lineal, Color.green);
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
