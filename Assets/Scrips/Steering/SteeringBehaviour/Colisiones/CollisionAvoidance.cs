using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehaviour
{
   
    //Lista de potenciales objetivos de colision;
    public Agent[] colisiones;
    private float tiempoMasCerca;
    Vector3 relativePos;
    private void Start()
    {
        grupo = Grupo.COLISIONES;
    }

    public override Steering GetSteering(AgentNPC miAgente)
    {
        colisiones = GameObject.FindObjectsOfType<Agent>();
        tiempoMasCerca = Mathf.Infinity;
        
        this.steering = new Steering(0, new Vector3(0,0,0));

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

            //Comprobar si habrá colisión
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

        //Vector3 aux = firstTarget.transform.position + (relativePos - firstTarget.transform.position);
        //aux = aux * ((float) firstTarget.RExterior + 2);

        relativePos = relativePos.normalized;
        steering.lineal = relativePos * miAgente.mAcceleration;
        Debug.DrawLine(this.transform.position, steering.lineal, Color.cyan);
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
