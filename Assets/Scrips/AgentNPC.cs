using System;
using System.Collections;
using UnityEngine;

public class AgentNPC : Agent
{
    private Steering ActionSteering;
    public BlenderSteering arbitro;
    public GoTarget goToTarget;

    //Los valores de las LayerMask para el mejor y el peor terreno de la unidad 
    public int mejorTerreno = 0;
    public Steering miSteering;
    public int peorTerreno = 1;


    // Builders 


    protected override void Start()
    {
        base.Start();
        //usar GetComponents<>() para cargar el arbitro del personaje
        arbitro = GetComponent<BlenderSteering>();
        // El go to target se salta todos los arbitros
        goToTarget = GetComponent<GoTarget>();
        miSteering = new Steering(0, new Vector3(0, 0, 0));
        ActionSteering = new Steering(0, new Vector3(0, 0, 0));
    }


    private void ActualizarVelocidad()
    {
        /*Si el terreno de nuestros pies es mejor Terreno entonces
         * mejoramos la velocidad máxima del personaje multiplicando por 1.5
         * , y si es el peor terreno entonces la reducimos a la mitad
        */
    }


    private void UpdateAccelerated(Steering steering, float time)
    {
        if (Mathf.Approximately(Vector3.Distance(steering.lineal, new Vector3(0, 0, 0)), 0))
            vVelocidad = new Vector3(0, 0, 0);

        if (Mathf.Approximately(steering.angular, 0))
            rotacion = 0;


        Debug.DrawRay(transform.position, vVelocidad, Color.white);
        transform.position += vVelocidad * time;
        orientacion += rotacion * time;

        vVelocidad += steering.lineal * time;

        // Si vamos mas rapido que la velicidad maxima reducimos
        if (vVelocidad.magnitude > mVelocidad)
        {
            vVelocidad.Normalize();
            vVelocidad *= mVelocidad;
        }

        vAceleracion = steering.lineal;

        if (vAceleracion.magnitude > mAceleracion)
        {
            vAceleracion.Normalize();
            vAceleracion *= mAceleracion;
        }

        //TODO
        rotacion += steering.angular * time;
    }

    private void UpdateNoAccelerated(Steering steering, float time)
    {
        transform.position = transform.position + steering.velocidad * time;
        orientacion = orientacion + steering.rotacion * time;
    }

    private void DesactivaSteering()
    {
        var steerings = GetComponents<SteeringBehaviour>();
        foreach (var steering in steerings) steering.enabled = false;
    }

    private void ActivaSteering()
    {
        var steerings = GetComponents<SteeringBehaviour>();
        foreach (var steering in steerings) steering.enabled = true;
    }

    /*Deja Invisible al personaje y lo hace reaparecer en base tras un tiempo
    para ir despues al punto de muerte.*/
    protected void Morir()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        StartCoroutine("respawn");
    }

    protected void Atacar(GameObject objetivo)
    {
        //Nos acercamos al objetivo hasta estar a el número de casillas necesarias

        //Lanzamos el ataque

        //Nos quedamos quietos durante un espacio de tiempo por haber atacado. 
    }

    //El personaje recibe el daño. Si ese daño deja su vida a 0 o menos, entonces lo mata
    protected void RecibirDaño(float cantidad)
    {
        vida -= cantidad;
        if (vida < 0) Morir();
    }

    //Se busca la fuente mas cercana para ir hacia ella
    protected void Huir()
    {
        var fuentes = GameObject.FindGameObjectsWithTag("puntoCurativo");
        var distanciaMinima = Mathf.Infinity;
        GameObject fuenteProxima;
        foreach (var fuente in fuentes)
        {
            var aux = fuente.transform.position - transform.position;
            if (distanciaMinima > aux.magnitude)
            {
                distanciaMinima = aux.magnitude;
                fuenteProxima = fuente;
            }
        }
        //Aplicar un Pathfinding a una casilla al lado de una fuente próxima
    }

    private IEnumerator respawn()
    {
        yield return new WaitForSeconds(2);
        GameObject cuartel;
        if (tag == "equipoRojo")
            cuartel = GameObject.FindWithTag("baseRoja");
        else
            cuartel = GameObject.FindWithTag("baseAzul");

        //Hacemos que spawnee al lado de su base
        gameObject.transform.position = cuartel.transform.position + new Vector3(0, 0, -50);
        //Recuperamos su vida
        vida = vidaMaxima;
        //Hacemos que vuelva a ser visible
        gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(5);
    }


    private void ApplySteering()
    {
        if (state != State.Action)
        {
            UpdateAccelerated(miSteering, Time.deltaTime);
            return;
        }

        if (cAction == CAction.None) UpdateAccelerated(miSteering, Time.deltaTime);
        else UpdateNoAccelerated(ActionSteering, Time.deltaTime);
    }


    protected override void Update()
    {
        // Actualiza el color si hay un cambio de estado
        base.Update();
        // Si estamos esperando nos quedamos quietos
        if (state != State.Waiting) ApplySteering();
        //ActualizarVelocidad();
    }

    // Los Steering se actualizan se usen o no otra cosa es que los guardemos
    private void LateUpdate()
    {
        if (state == State.Action)
            ActionSteering = cAction switch
            {
                CAction.None => new Steering(0, new Vector3(0, 0, 0)),
                CAction.GoToTarget => goToTarget.GetSteering(this),
                _ => throw new ArgumentOutOfRangeException()
            };
        else
            miSteering = arbitro.GetSteering();
    }
}