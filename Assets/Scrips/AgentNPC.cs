using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AgentNpc : Agent
{
    // Actuadores
    [Header("Actuador")]
    [SerializeField] protected BaseActuator actuator;

    // Steerings
    [Header("Steerings")]
    [SerializeField] private ArbitroSteering arbitro; // Asigna mis steerings.
    [SerializeField] private Steering finalSteering;


    // Estados
    [Header("Estados")]
    [SerializeField] public CAction cAction = CAction.None; //  Action para las acciones
    public bool selected; // Si estoy seleccionado
    public State state = State.Normal; // State para las ordenes
    private bool stateChanged; // Mi estado ha cambiado recargar color y sombrero

    // Controller
    [Header("Controlador")]
    public Controlador controlador;
    [Header("Bases")]
    [SerializeField] private AgentBase enemyBase;
    // Para saber si estoy atacando
    [SerializeField] private AgentBase mybase;
    //Los valores de las LayerMask para el mejor y el peor terreno de la unidad 
    [SerializeField] protected int mejorTerreno = 3;
    [SerializeField] protected int peorTerreno = 0;
 

    [Header("Formaciones")]
    // Formaciones
    [SerializeField] private Formation formation;
    public bool InFormation => formation != null; // Si estoy en formacion

    [SerializeField]
    public GridChungo grid; //Grid para calcular posiciones de los enemigos

    [SerializeField] protected Heuristic heuristic;




    // Heuristca
    public virtual Heuristic GetHeuristic() => throw new NotImplementedException();


    protected override void Start()
    {
        base.Start();
        if (actuator == null)
            actuator = gameObject.AddComponent(typeof(FilterActuator)) as FilterActuator;
        SaveOriginalColor();
        controlador = GameObject.FindGameObjectWithTag("controlador")
            .GetComponent<Controlador>();
        //usar GetComponents<>() para cargar el arbitro del personaje
        arbitro = GetComponent<ArbitroSteering>();
        finalSteering = new Steering(0, new Vector3(0, 0, 0));

    }


    private void OnMouseDown() => controlador.AddOrRemoveFromSelected(this);


    #region Steering

    private void ApplySteering()
    {
        if (state != State.Action)
        {
            UpdateAccelerated(finalSteering, Time.deltaTime);
            return;
        }


        switch (cAction)
        {
            case CAction.None:
                break;
            case CAction.GoToTarget:
                UpdateNoAccelerated(finalSteering, Time.deltaTime);
                break;
            case CAction.Forming:
            case CAction.GoingToEnemy:
            case CAction.Defend:
            case CAction.Retreat:
            case CAction.AttackEnemy:
            case CAction.GoingToLandPoint:
                UpdateAccelerated(finalSteering, Time.deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion


    /*
     * Modo debug
     */
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!debug) return;
        //Pintamos el vector de acceleracion
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, finalSteering.lineal);
        //Gizmos.DrawRay(transform.position, steering.lineal);
    }


    #region SetColors

    public void SetColorTotalWar() => SetHat(HatsTypes.Police);

    private void UpdateColor()
    {
        void SetColorSelected() => SetColor(Color.white);

        void SetColorWaiting() => SetColorHeadBand(Color.red);

        void SetColorGoToTarget() => SetColorHeadBand(Color.green);

        void SetColorForming() => SetColorHeadBand(Color.black);

        void SetColorBoss() => SetHat(HatsTypes.Police);

        void SetColorSoldier() => SetHat(HatsTypes.CowBoy);

        // Solo cambiamos el color de la banda 

        if (!stateChanged) return;
        Debug.Log(
            $"State {state} | Action {cAction} | Formation {InFormation} | Selected {selected}");
        ResetVisualStatus(); //Reseteamos lo visual
        switch (state)
        {
            case State.Normal:
                break;
            case State.Waiting:
                SetColorWaiting();
                break;
            case State.Action:
                switch (cAction)
                {
                    case CAction.None:
                        break;
                    case CAction.GoToTarget:
                        SetColorGoToTarget();
                        break;
                    case CAction.Forming:
                        SetColorForming();
                        break;
                    case CAction.GoingToEnemy:
                        SetHat(HatsTypes.Police);
                        break;
                    case CAction.AttackEnemy:
                        SetHat(HatsTypes.Viking);
                        break;
                    case CAction.Defend:
                        SetHat(HatsTypes.Pajama);
                        break;
                    case CAction.Retreat:
                        SetHat(HatsTypes.Crown);
                        break;
                    case CAction.GoingToLandPoint:
                        SetHat(HatsTypes.Miner);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (ImLeader()) SetColorBoss();
        if (ImSoldier()) SetColorSoldier();
        if (selected) SetColorSelected();
        stateChanged = false;
    }

    #endregion


    #region Estados

    internal void AddToSelected()
    {
        selected = true;
        stateChanged = true;

        if (InFormation)
        {
            var resto = formation.GetRest(this);
            resto.ForEach(npc => controlador.AddToSelected(npc));
        }
    }

    internal void RemoveFromSelected()
    {
        selected = false;
        stateChanged = true;

        if (InFormation)
        {
            var resto = formation.GetRest(this);
            resto.ForEach(npc => controlador.RemoveFromSelected(npc));
        }
    }

    public void ChangeState(State nextState)
    {
        stateChanged = true;
        var oldState = state;
        state = nextState;
        Debug.Log($"Estado cambiado de {oldState} a {state}");
    }

    public void ChangeAction(CAction action)
    {
        stateChanged = true;
        var oldState = cAction;
        cAction = action;
        Debug.Log($"Action cambiado de {oldState} a {cAction}");
    }

    public void ResetStateAction()
    {
        ChangeAction(CAction.None);
        ChangeState(State.Normal);

        if (formation != null)
        {
            Debug.Log("Disolviendo la formación");

            formation.DissolveFormation();
            arbitro.RestoreWeight();
            formation = null;
        }
    }

    #endregion


    #region Formaciones

    private bool ImLeader() => InFormation && formation.ImLeader(this);

    private bool ImSoldier() => InFormation && formation.ImSoldier(this);

    public void BecameLeader(Formation newFormation) => formation = newFormation;

    public void BecameSoldier(Formation newFormation)
    {
        ChangeState(State.Action);
        ChangeAction(CAction.Forming);

        formation = newFormation;
        arbitro.SetFormation(newFormation);
    }


    // Semaforo MoveABitBeforeWaiting
    private bool willWait;

    public void MoveABitBeforeWaiting()
    {
        if (!formation.ImLeader(this)) throw new Exception("No eres el líder");
        if (willWait) return;

        IEnumerator Timer(float secondsToWait)
        {
            willWait = true;
            Debug.Log($"Will wait in {secondsToWait} seconds ");
            yield return new WaitForSeconds(secondsToWait);
            Debug.Log("Im waiting");
            ChangeState(State
                .Waiting); // Pasados X segundos nos esperamos a los soldados.
            willWait = false;
        }

        StartCoroutine(Timer(5));
    }

    //Semaforo AllInPositionWaitABitMore
    private bool waitingSoldiers;

    public void AllInPositionWaitABitMore()
    {
        if (!formation.ImLeader(this)) throw new Exception("No eres el líder");
        if (waitingSoldiers) return; // Si ya estamos esperando no hacemos nada

        IEnumerator Timer(float secondsToWait)
        {
            waitingSoldiers = true;
            //Print the time of when the function is first called.
            Debug.Log($"Will move in {secondsToWait} seconds");

            yield return new WaitForSeconds(secondsToWait);

            //After we have waited 5 seconds print the time again.
            Debug.Log("Im moving");
            ChangeState(State.Normal); // Ya te puedes mover líder
            controlador
                .ActionFinished(this);
            // Le decimos al controlador que la accion ya esta terminada
            formation
                ?.WaitForSoldiers(); // Le decimos a la formacion que espere a los soldados
            waitingSoldiers = false;
        }

        StartCoroutine(Timer(10));
    }

    #endregion

    /**
     * ALL UPDATE METHODS
     */

    #region Update

    private void UpdateAccelerated(Steering steering, float time)
    {
        //OrientationToVector() devuelve el vector hacia donde apunta
        var act = actuator.Act(steering, this);

        //Debug.Log($"PreFiltre {vVelocidad} {rotacion}");
        if (act.lineal == new Vector3(0, 0, 0))
            vVelocidad = new Vector3(0, 0, 0);

        if (Mathf.Approximately(act.angular, 0))
            rotacion = 0;

        //Debug.Log($"PostFiltre {vVelocidad} {rotacion}");
        //Debug.DrawRay(transform.position, vVelocidad, Color.white);

        if (debug) Debug.DrawRay(transform.position, vVelocidad, Color.magenta);


        transform.position += vVelocidad * time;
        orientacion += rotacion * time;

        vVelocidad += act.lineal * time;

        // Si vamos mas rapido que la velicidad maxima reducimos
        if (vVelocidad.magnitude > mVelocity)
        {
            vVelocidad.Normalize();
            vVelocidad *= mVelocity;
        }

        vAceleracion = act.lineal;

        if (vAceleracion.magnitude > mAcceleration)
        {
            vAceleracion.Normalize();
            vAceleracion *= mAcceleration;
        }

        // TODO si rotamos demasiado reducirmos
        rotacion += act.angular * time;

        // Si rotamos muy rápido la normalizamos
        //var angularAcceleration = Math.Abs(rotacion);
        //if (angularAcceleration > mAngularAcceleration)
        //{
        //    rotacion /= angularAcceleration;
        //    rotacion *= mAngularAcceleration;
        //}

        //rotacion = (float) Math.Floor(rotacion);

        //velocidad = vVelocidad.magnitude;
    }

    private void UpdateNoAccelerated(Steering steering, float time)
    {
        transform.position = transform.position + steering.velocidad * time;
        orientacion = orientacion + steering.rotacion * time;
    }

    private void ActualizarVelocidad()
    {
        /*Si el terreno de nuestros pies es mejor Terreno entonces
         * mejoramos la velocidad máxima del personaje multiplicando por 1.5
         * , y si es el peor terreno entonces la reducimos a la mitad
        */
        var terreno = FindObjectOfType<Terrain>();
        if (terreno != null)
        {
            var indice = controlador.GetTerrainLayer(transform.position, terreno);
            if (indice == mejorTerreno)
            {
                mVelocity = (float) (baseVelocity * 1.5);
                //Debug.Log("Aumento Velocidad");
            }
            else
            {
                if (indice == peorTerreno)
                    mVelocity = (float) (baseVelocity / 1.5);
                //Debug.Log("Disminuyo Velocidad");
                else
                    mVelocity = baseVelocity;
            }
        }
    }

    protected void Update()
    {
        ActualizarVelocidad();
        // Actualiza el color si hay un cambio de estado
        UpdateColor();
        // Si estamos esperando nos quedamos quietos
        if (state != State.Waiting) ApplySteering();
    }

    // Los Steering se actualizan se usen o no otra cosa es que los guardemos
    private void LateUpdate()
    {
        finalSteering = arbitro.GetFinalSteering(state, cAction);
        ActualizarVelocidad();
    }

    #endregion


    #region Actions

    /**
     * Voy a una posicion
     * Si hay agentes en formacion y sin formacion la disolvemos
     * Controller -> GoToTarget -> arbitro
     */
    public void GoToTarget(Vector3 newPoint, bool mixed)
    {
        // Si hay mezcla de en formación y fuera de ella rompemos formación;

        if (InFormation && mixed)
            formation.DissolveFormation();


        if (InFormation)
            if (formation.ImSoldier(this))
            {
                // Si soy un soldado no hago nada
                ChangeState(State.Action);
                ChangeAction(CAction.Forming);
                return;
            }


        // Soy el boss o no estoy en formacion
        ChangeState(State.Action);
        ChangeAction(CAction.GoToTarget);
        arbitro.SetNewTarget(newPoint);
    }


    public void ArrivedToTarget()
    {
        ChangeAction(CAction.None);
        ChangeState(State.Waiting);
        WaitAtTarget();
    }

    // Semaforo WaitBeforeFinishing
    // // Si estoy esperando para mandar un mensaje al controlador
    private bool alreadyWaiting;

    private void WaitAtTarget()
    {
        if (alreadyWaiting) return;

        IEnumerator WaitBeforeFinishing(float secondsToWait)
        {
            alreadyWaiting = true;
            //Print the time of when the function is first called.
            Debug.Log($"Will move in {secondsToWait} seconds | WaitAtTarget");

            yield return new WaitForSeconds(secondsToWait);

            //After we have waited 5 seconds print the time again.
            Debug.Log("Im moving | WaitAtTarget");
            ChangeState(State.Normal);
            controlador.ActionFinished(this);
            alreadyWaiting = false;
        }

        StartCoroutine(WaitBeforeFinishing(5));
    }

    #endregion

    // Set and update diferent colors.

    #region Actions Segunda parte

    // Condiciones
    public bool IsAttacking() => mybase != null && mybase.IsAttacking();

    public bool IsTotalWar() => mybase != null && mybase.IsTotalWar();

    public bool IsNotRunning() => cAction == CAction.None && state == State.Normal;

    public bool IsInjured() => vida < vidaMaxima / 2;
    public bool IsCloserToEnemy() => NearEnemy();
    public bool IsNotDead() => !Muerto;
    public bool AlreadyHealing() => NearFont();
    public bool CanGoToBase() => !NearBase();
    public bool CanGoToLandPoint() => !NearLandPoint();

    public bool BestTerrain() => CurrentTerrain() == mejorTerreno;

    public bool WorstTerrain() => CurrentTerrain() == peorTerreno;
    private int CurrentTerrain() =>
        controlador.GetTerrainLayer(transform.position, FindObjectOfType<Terrain>());

    public bool UnderMyDomain(int value) => (tag == "equipoRojo") ? value < 0 : value > 0;

    // Distancia de Chebyshov
    private int GetDistanceTwoPosition(Transform p1, Transform p2)
    {
        var basePosition = grid.GetNodeFromWorldPoint(p1.position);
        var currentPosition = grid.GetNodeFromWorldPoint(p2.position);


        var x = basePosition.gridX - currentPosition.gridX;
        var z = basePosition.gridZ - currentPosition.gridZ;

        return Math.Max(Math.Abs(x), Math.Abs(z));
    }

    private bool GetDistanceRayCast(Agent target)
    {
        var castRange = grid.nodeRaidus + grid.nodeRaidus * 2 * alcance;

        var vecinos = Physics.SphereCastAll(transform.position, castRange, Vector3.up);
        return vecinos
            .Select(raycastHit => raycastHit.collider.gameObject.GetComponent<Agent>())
            .Any(enemigo => enemigo == target);
    }


    private bool NearBase()
    {
        var distance = GetDistanceTwoPosition(transform, enemyBase.transform);


        return distance <= alcance || GetDistanceRayCast(enemyBase);
    }


    public bool NearMYBase()
    {
        var distance = GetDistanceTwoPosition(transform, mybase.transform);

        distance -= (int) Math.Floor(rInterior / grid.nodeRaidus);

        return distance <= alcance;
    }


    [SerializeField] private FuenteCurativa fuenteActual; // Fuente a la que voy yendo

    private bool NearFont()
    {
        if (fuenteActual == null) return false;

        var npcs = Physics.OverlapSphere(fuenteActual.transform.position, fuenteActual.radioCuracion)
            .Select(col => col.gameObject.GetComponent<AgentNpc>()).Contains(this as AgentNpc);;
        return npcs;

    }

    // Fuente a la que voy yendo
    [SerializeField] private Transform nearPosition;

    public bool InPosition()
    {
        var distance = GetDistanceTwoPosition(transform, nearPosition);

        return distance == 0;
    }
    [SerializeField] public GameObject landPoint=null;
    public bool NearLandPoint()
    {
        Debug.Log($"{name} Buscando puntos de interest");

        var iGrid = controlador.GetInfluenceMap();

        //Busco el que tengan mayor influencia enemiga
        // Positvo
        //float influencia = 0;
        var puntos = GameObject.FindGameObjectsWithTag("puntoInteres");
        foreach (var punto in puntos)
        {
            var current =
                iGrid.GetNodeFromWorldPoint(punto.transform.position);

            var vecinos = iGrid.GetNeighbors(current, alcance);

            var actual = current.valor + vecinos.Sum(nodo => nodo.valor);

            if (!UnderMyDomain(actual))
            {
                landPoint = punto;
            } 


        }

        //Si no hay puntos que conquistar vamos a uno random
        if (landPoint == null)
        {
               
            var index = (int)(Random.value * puntos.Length) % puntos.Length;
            landPoint = puntos[index];
            Debug.LogWarning($"Todo conquistado vamos a {landPoint.name}");
        }


        var distance = GetDistanceTwoPosition(transform,landPoint.transform);
        return distance  <= alcance;
         
    }


    //Comprueba si hay un enemigo en un radio de diez veces el radio exterior
    // Si lo hay lo0 mete un una lista de enemigos

    [SerializeField] public HashSet<Agent> enemigos;

    private bool NearEnemy()
    {
        if (atacando) return true; // Estoy atacando no hace falta que me calcules cosas
        enemigos = new HashSet<Agent>();
        var castRange = grid.nodeRaidus + grid.nodeRaidus * 2 * alcance;

        var vecinos = Physics.SphereCastAll(transform.position, castRange, Vector3.up);
        if(debug) DebugPlus.DrawSphere(transform.position, castRange);
        foreach (var vecinoHit in vecinos)
        {
            var enemigo = vecinoHit.collider.gameObject.GetComponent<Agent>();
            if (enemigo == null) continue;
            if (enemigo.Muerto) continue; //Esta muerto dejalo
            switch (tag)
            {
                case "equipoRojo"
                    when enemigo.tag == "baseAzul" || enemigo.tag == "equipoAzul":
                case "equipoAzul"
                    when enemigo.tag == "baseRoja" || enemigo.tag == "equipoRojo":
                    enemigos.Add(enemigo);
                    break;
            }
        }

        //Debug.Log($"Enemigos encontrados {enemigos.Count}");
        return enemigos.Count > 0;
    }

//Acciones como tal


//Resetea el estado y los steering especiales si los hubiera
// En la segunda parte el estado por defecto va a ser waiting
    public void ResetStateAndSteering()
    {
        if (state == State.Action)
            arbitro.CancelSteeringAction(cAction);

        vVelocidad = Vector3.zero;
        vAceleracion = Vector3.zero;


        ChangeAction(CAction.None);
        ChangeState(State.Waiting);
    }

    //Cuantos nodos queremos avanzar de una
    [SerializeField] [Range(1, 20)] private readonly int step =
        10;

    //Avanzamos hacia la base enemiga step casillas
    public void GoToEnemyBase()
    {
        ChangeState(State.Action);
        ChangeAction(CAction.GoingToEnemy);
        var origen = gameObject.transform.position;
        var target = enemyBase.transform.position;
        var rExterior = enemyBase.RExterior;
        var cH = heuristic;

        arbitro.SetNewTargetWithA(step, origen, target, rExterior, cH, NearBase);
    }

    // Avanzamos hacia un GameObject X casillas
    public void GoToEpicPoint(GameObject obj)
    {
        ChangeState(State.Action);
        ChangeAction(CAction.GoingToLandPoint);
        var origen = gameObject.transform.position;
        var target = obj.transform.position;
        var rExterior = RExterior;
        var cH = heuristic;

        arbitro.SetNewTargetWithA(step, origen, target, rExterior, cH, NearLandPoint);
    }

    public void Huir(GameObject obj)
    {
        ChangeState(State.Action);
        ChangeAction(CAction.Retreat);
        var origen = gameObject.transform.position;
        var target = obj.transform.position;
        var rExterior = RExterior;
        var cH = heuristic;
        fuenteActual = obj.GetComponent<FuenteCurativa>();

        arbitro.SetNewTargetWithA(step, origen, target, rExterior, cH, NearFont);
    }

    [SerializeField] public GameObject[] puntosPatrulla;
    /*public void Patrullar(int index)
    {
        ChangeState(State.Action);
        ChangeAction(CAction.GoToTarget);
        puntosPatrulla = GameObject.FindGameObjectsWithTag("Patrulla");
        int pos = index % puntosPatrulla.Length;
        var origen = gameObject.transform.position;
        var target = puntosPatrulla[pos].transform.position;
        var rExterior = RExterior;
        var cH = heuristic;
        nearPosition = puntosPatrulla[pos].transform;
        arbitro.SetNewTargetWithA(step, origen, target, rExterior, cH, InPosition);
    }*/

    /*Deja Invisible al personaje y lo hace reaparecer en base tras un tiempo
    para ir despues al punto de muerte.*/
    protected override void Morir()
    {
        atacando = false;
        mybase.Dead(this, 5);
        
    }

    protected bool atacando;


    protected IEnumerator WaitBeforeAttack(float secondsToWait, double realDamage,
        Agent objetivo, ParticleSystem particles)
    {
        atacando = true;
        particles.Play();

        int cDefensa = defensa;
        if(objetivo is AgentNpc)
            if (BestTerrain())
                cDefensa *=  2;
            else if(WorstTerrain())
                cDefensa /= 2;

        double finalDamage = realDamage - cDefensa;

        objetivo.RecibirDaño(finalDamage);
        yield return new WaitForSeconds(secondsToWait);
        ResetStateAndSteering();
        atacando = false;
    }

    protected internal abstract void Atacar(Agent objetivo);


    

    public void Defend()
    {
        ChangeState(State.Action);
        ChangeAction(CAction.Defend);
        var origen = gameObject.transform.position;
        var target = mybase.transform.position;
        var rExterior = mybase.RExterior;
        var cH = heuristic;

        arbitro.SetNewTargetWithA(step, origen, target, rExterior, cH, NearMYBase);
    }

//Se acerca al siguiente punto de interes que no esté conquistado
    public void FindEnemy()
    {
        ChangeState(State.Action);
        ChangeAction(CAction.GoToTarget);
        var origen = gameObject.transform.position;
        var rExterior = RExterior;
        var cH = heuristic;
    }

    #endregion
}