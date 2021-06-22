using UnityEngine;

public class Agent : Body
{
    private const double margen = 1.1;
    [SerializeField] private double mAExterior = 0;

    //Angulos
    [SerializeField] public double aInterior;

    [SerializeField] protected bool debug = false;


    //Radio
    [SerializeField] public double rInterior;


    public double RExterior => rInterior * margen;

    public double AExterior
    {
        get
        {
            var aTemp = aInterior * margen;
            return aTemp < mAExterior ? mAExterior : aTemp;
        }
    }

    // Builders

    public Agent NotSoShallowCopy()
    {
        var fastAnget = new Agent();
        fastAnget.transform.position = transform.position;
        fastAnget.orientacion = orientacion;
        return fastAnget;
    }


    protected virtual void OnDrawGizmos() // Gizmo: una línea en la dirección del objetivo
    {
        if (!debug) return;

        Gizmos.DrawSphere(transform.position, (float) rInterior);
        //Gizmos.DrawSphere(transform.position, (float)this.RExterior);
        //Gizmos.DrawSphere(transform.position, (float)this.);
        //Gizmos.DrawSphere(transform.position, (float)this.rInterior);
    }
}