using UnityEngine;

public class Agent : Body
{
    [SerializeField,Range(1,5)] private double margen = 1.1;
    [SerializeField] private double mAExterior = 0;

    //Angulos
    [SerializeField] public double aInterior;

    [SerializeField] protected bool debug = false;
    [SerializeField] protected bool exterior = false;
    
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

    //Daño
    public bool Muerto => vida <= 0;

    protected virtual void Morir()
    {
        Debug.Log("Win win");
    }


    public void RecibirDaño(double cantidad)
    {
        if (Muerto) return;

        Debug.Log($"{name} ha recibido {cantidad} daño");
        vida -= cantidad;

        if (vida <= 0) Morir();
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
        if (exterior)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, (float)this.RExterior);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, (float) rInterior);
        }
      
        
        //Gizmos.DrawSphere(transform.position, (float)this.);
        //Gizmos.DrawSphere(transform.position, (float)this.rInterior);
    }
}