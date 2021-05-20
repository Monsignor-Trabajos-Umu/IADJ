using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AgentNPC : Agent
{
    public Steering miSteering;

    public BlenderSteering arbitro;
    public GoTarget goToTarget;
    bool targetExist;
    private Steering goToTargetSteering;

    //Los valores de las LayerMask para el mejor y el peor terreno de la unidad 
    public int mejorTerreno = 0;
    public int peorTerreno = 1;

    // Update is called once per frame
    void Update()
    {
        ApplySteering();
        //ActualizarVelocidad();
    }

    private void Awake()
    {
        //usar GetComponents<>() para cargar el arbitro del personaje
        arbitro = GetComponent<BlenderSteering>();
        // El go to target se salta todos los arbitros
        goToTarget = GetComponent<GoTarget>();
        targetExist = false;
        miSteering = new Steering(0, new Vector3(0, 0, 0));
        goToTargetSteering = new Steering(0, new Vector3(0, 0, 0));
    }

    private void ActualizarVelocidad()
    {
        /*Si el terreno de nuestros pies es mejor Terreno entonces
         * mejoramos la velocidad máxima del personaje multiplicando por 1.5
         * , y si es el peor terreno entonces la reducimos a la mitad
        */

    }

    private void LateUpdate()
    {
        //Pide el steering a Agente y si hay un target usa ese steering
        if (goToTarget != null)
        {
            this.targetExist = goToTarget.targetExists;
            this.goToTargetSteering = goToTarget.GetSteering(this);
        }
        miSteering = arbitro.GetSteering();

    }

    public void ApplySteering()
    {
        if (!targetExist)
            updateAcelerated(miSteering, Time.deltaTime);
        else
            updateNoAcelerated(goToTargetSteering, Time.deltaTime);
    }


    private void updateAcelerated(Steering steering, float time)
    {

        if (Vector3.Distance(steering.lineal, new Vector3(0, 0, 0)) == 0)
        {
            this.vVelocidad = new Vector3(0, 0, 0);
        }
        if (steering.angular == 0)
        {
            this.rotacion = 0;
        }


        //Debug.DrawRay(transform.position, this.vVelocidad, Color.white);
        transform.position = transform.position + this.vVelocidad * time;
        this.orientacion = this.orientacion + this.rotacion * time;

        this.vVelocidad = this.vVelocidad + steering.lineal * time;

        // Si vamos mas rapido que la velicidad maxima reducimos
        if (this.vVelocidad.magnitude > this.mVelocidad)
        {
            this.vVelocidad.Normalize();
            this.vVelocidad *= this.mVelocidad;
        }

        this.vAceleracion = steering.lineal;

        if (this.vAceleracion.magnitude > this.mAceleracion)
        {
            this.vAceleracion.Normalize();
            this.vAceleracion *= this.mAceleracion;
        }
        //TODO
        this.rotacion = this.rotacion + steering.angular * time;



    }

    private void updateNoAcelerated(Steering steering, float time)
    {
        transform.position = transform.position + steering.velocidad * time;
        this.orientacion = this.orientacion + steering.rotacion * time;

    }

    private void DesactivaSteering()
    {
        var steerings = this.GetComponents<SteeringBehaviour>();
        foreach (var steering in steerings)
        {
            steering.enabled = false;
        }
    }
    private void ActivaSteering()
    {
        var steerings = this.GetComponents<SteeringBehaviour>();
        foreach (var steering in steerings)
        {
            steering.enabled = true;
        }
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
        GameObject[] fuentes = GameObject.FindGameObjectsWithTag("puntoCurativo");
        float distanciaMinima = Mathf.Infinity;
        GameObject fuenteProxima;
        foreach(GameObject fuente in fuentes) {
            var aux = fuente.transform.position - transform.position;
            if(distanciaMinima > aux.magnitude)
            {
                distanciaMinima = aux.magnitude;
                fuenteProxima = fuente;
            }
        }
        //Aplicar un Pathfinding a una casilla al lado de una fuente próxima
    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(2);
        GameObject cuartel;
        if (tag == "equipoRojo")
        {
            cuartel = GameObject.FindWithTag("baseRoja");
            
        }
        else
        {
            cuartel = GameObject.FindWithTag("baseAzul");
        }
        //Hacemos que spawnee al lado de su base
        gameObject.transform.position = cuartel.transform.position + new Vector3(0, 0, -50);
        //Recuperamos su vida
        vida = vidaMaxima;
        //Hacemos que vuelva a ser visible
        gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(5);
    }
}
