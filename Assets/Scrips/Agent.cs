using System;
using UnityEngine;

public class Agent : Body
{
    private const double margen = 1.1;

    //Angulos
    [SerializeField] public double aInterior;

    private bool cambioEstado;

    // Controlador
    private Controlador controlador;
    public bool dEbUg = false;
    [SerializeField] private readonly double mAExterior = 0;
    protected State oldState = State.Normal;

    //Radio
    [SerializeField] public double rInterior;

    // State para las ordenes y Action para las acciones
    public State state = State.Normal;
    public CAction cAction = CAction.None;

    public double rExterior => rInterior * margen;

    public double aExterior
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


    // Guardamos el color por defecto y asignamos el controlador para mandar mensajes.
    protected virtual void Start()
    {
        SaveDefaultColor();
        controlador = GameObject.FindGameObjectWithTag("controlador").GetComponent<Controlador>();
    }

    // Other Functions 


    private void OnMouseDown()
    {
        controlador.AddOrRemoveFromSelected(this);
    }

    // Esta llamada la recibimos desde el Controlador
    // Nos dice el nextState actual que tenemos que tener.
    public void MakeState(State nextState)
    {
        cambioEstado = true;
        oldState = state;
        state = nextState;
        Debug.Log($"Estado cambiado de {oldState} a {state}");
    }


    public void GoToTarget(Vector3 position)
    {
        MakeState(State.Action);
        cAction = CAction.GoToTarget;
        gameObject.SendMessage("NewTarget", position);
    }


    public void ArrivedToTarget()
    {
        MakeState(State.Waiting);
        cAction = CAction.None;
        controlador.Done();
    }


    // Solo cambiamos el color
    private void UpdateColor()
    {
        switch (state)
        {
            case State.Normal:
                SetColorNormal();
                break;
            case State.Selected:
                SetColorSelected();
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
                    case CAction.FormationLeader:
                        SetColorBoss();
                        break;
                    case CAction.FormationSoldier:
                        SetColorSoldier();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /**
     * Realizamos los cambios necesarios según el estado anterior.
     */
    private void CheckStateFromNormal()
    {
        switch (state)
        {
            case State.Normal: // Normal => Normal
            case State.Selected: // Normal => Selected || No Cambia nada
                break;
            case State.Waiting: // Normal => Waiting
            case State.Action: // Normal => Normal
                throw new Exception("Normal /=> Selected ");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckStateFromSelected()
    {
        switch (state)
        {
            case State.Normal: // Selected => Normal || No Cambia nada
                break;

            case State.Waiting: // Selected => Waiting
                break;
            // No accesibles
            case State.Selected: // Selected => Selected
                throw new Exception("Ya esta seleccionado");
            case State.Action: // Selected => Action
                throw new Exception("Tienes que mandar un orden antes");

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckStateFromWaiting()
    {
        switch (state)
        {
            case State.Normal: // Waiting => Normal
                break;
            case State.Selected: // Waiting => Selected
                break;
            case State.Waiting: // Waiting => Waiting
                break;
            case State.Action: // Waiting => Normal
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckStateFromAction()
    {
        switch (state)
        {
            case State.Normal: // Action => Normal
                break;
            case State.Selected: // Action => Selected
                break;
            case State.Waiting: // Action => Waiting
                break;
            case State.Action: // Action => Action
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //Actualiza el color si hay un cambio de estado
    private void CheckState()
    {
        //Debug.Log($"Estado actual -> {state} | Cambio -> {cambioEstado}");
        // Si el nextState ha cambiado cambiamos el color
        if (!cambioEstado) return;
        Debug.Log("Se ha producido un cambio de estado cambiando de color");

        // Primero cambiamos el color para no repetir mucho No comprobamos de que estado venimos
        UpdateColor();


        switch (oldState)
        {
            case State.Normal:
                CheckStateFromNormal();
                break;
            case State.Waiting:
                CheckStateFromWaiting();
                break;
            case State.Selected:
                CheckStateFromSelected();
                break;
            case State.Action:
                CheckStateFromAction();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        cambioEstado = false;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        CheckState();
    }

    // Debug


    private void OnDrawGizmos() // Gizmo: una línea en la dirección del objetivo
    {
        if (dEbUg)
            Gizmos.DrawSphere(transform.position, (float) rInterior);
        //Gizmos.DrawSphere(transform.position, (float)this.rExterior);
        //Gizmos.DrawSphere(transform.position, (float)this.);
        //Gizmos.DrawSphere(transform.position, (float)this.rInterior);
    }
}