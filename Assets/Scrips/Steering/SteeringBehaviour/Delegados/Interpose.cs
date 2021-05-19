using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpose : Arrive
{
    [SerializeField]
    Agent agenteA;
    [SerializeField]
    Agent agenteB;

    private Vector3 midPoint;
    private void Start()
    {
       // target = Instantiate(agenteA);
        //target.GetComponent<MeshRenderer>().enabled = false;
    }

    public override Steering GetSteering(AgentNPC agent)
    {
        //Si no tenemos los parámetros devolvemos un steering básico
        if (agenteA == agenteB || agenteA == null || agenteB == null) return new Steering();

        //Calculamos el punto medio entre los dos objetivos
        midPoint = (agenteA.transform.position + agenteB.transform.position) / 2;


        float timeToReachMidPoint = Vector3.Distance(agent.transform.position, midPoint) / agent.mVelocidad;

        //Posiciones de los objetivos a futuro
        Vector3 posA = agenteA.transform.position + agenteA.vVelocidad * timeToReachMidPoint;
        Vector3 posB = agenteB.transform.position + agenteB.vVelocidad * timeToReachMidPoint;
        midPoint = (posA + posB) / 2;

        target.transform.position = midPoint;
        return base.GetSteering(agent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(agenteA.transform.position, agenteB.transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(midPoint, 2);

        Gizmos.color = Color.red;
        Vector3 v = this.gameObject.GetComponent<AgentNPC>().vVelocidad;
        Vector3 dot = Vector3.Cross(this.gameObject.transform.position , v);
        Gizmos.DrawLine(this.gameObject.transform.position, dot);
    }
}
