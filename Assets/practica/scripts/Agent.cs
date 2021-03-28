using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Body
{
    //Radio
    [SerializeField]
    double rInterior;
    public double rExterior { get => this.rInterior * margen; }
    //Angulos
    [SerializeField]
    double aInterior;
    public double AExterior { get => this.aInterior*margen; }

    //Controlador
    [SerializeField]
    private Controlador controlador;

    //Stearing
    public Steering steering;

    public bool dEbUg = false;

    private const double margen = 1.1;




    private void OnDrawGizmos() // Gizmo: una línea en la dirección del objetivo
    {
        if (this.dEbUg)
        {
            Gizmos.DrawSphere(transform.position, (float)this.rInterior);
            //Gizmos.DrawSphere(transform.position, (float)this.rExterior);
        }
        //Gizmos.DrawSphere(transform.position, (float)this.);
        //Gizmos.DrawSphere(transform.position, (float)this.rInterior);
    }

    private void OnMouseDown()
    {
        controlador.addSeleccionados(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
