using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    private CAction action;
    [SerializeField] private bool debug;

    [SerializeField] private HashSet<AgentNpc> GetSelected { get; set; }

    // Builders
    private void Awake() =>
        GetSelected = new HashSet<AgentNpc>(); //Creamos la lista de seleccionados

    //Hemos terminado la acción
    public void Done() => action = CAction.None;

    // La accion ha terminado
    public void ActionFinished(AgentNpc agent)
    {
        PrintIfDebug($"{agent.name} ha terminado");
        //RemoveFromSelected(agent);
    }

    public void AddOrRemoveFromSelected(AgentNpc agent)
    {
        if (GetSelected.Contains(agent))
            RemoveFromSelected(agent);
        else
            AddToSelected(agent);
    }


    public void AddToSelected(AgentNpc agent)
    {
        if (GetSelected.Contains(agent)) return;
        GetSelected.Add(agent);
        PrintIfDebug("seleccionados: " + GetSelected.Count);
        agent.AddToSelected();
    }

    public void RemoveFromSelected(AgentNpc agent)
    {
        if (!GetSelected.Contains(agent)) return;
        GetSelected.Remove(agent);
        agent.RemoveFromSelected();
        PrintIfDebug($"Quitar seleccionados: {GetSelected.Count}");
    }

    private void RemoveAndResetFromSelected(AgentNpc agent)
    {
        RemoveFromSelected(agent);
        PrintIfDebug($"Reset estado: {agent.name}");
        agent.ResetStateAction();
    }

    public int getInfluencia(Vector3 posicion)
    {
        var influenceController = GameObject.FindGameObjectWithTag("mapaInfluencia");
        var influenceMap = influenceController.GetComponent<InfluenceMapControl>();
        return influenceMap.GetGridPosition(posicion).valor;
    }

    public InfluenceGrid GetInfluenceMap()
    {
        var influenceController = GameObject.FindGameObjectWithTag("mapaInfluencia");
        var influenceMap = influenceController.GetComponent<InfluenceMapControl>();
        return influenceMap.gridMap;
    }



    private void SetWaiting()
    {
        GetSelected.Where(agente => agente.selected)
            .ToList()
            .ForEach(agent => agent.ChangeState(State.Waiting));
    }

    // Vamos a una posición del raton
    private void GotoMousePosition()
    {
        // Damos una orden cuando levantemos el botón del ratón.
        if (!Input.GetMouseButtonUp(1)) return;
        // Comprobamos si el ratón golpea a algo en el escenario.
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
            // Si lo que golpea es un punto del terreno entonces da la orden a todas las unidades NPC
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("ground"))
            {
                var center = hitInfo.point;

                var agentsNpc =
                    GetSelected.Where(agente => agente.state == State.Waiting).ToList();
                var numberOfAgents = agentsNpc.Count();


                if (numberOfAgents <= 0) return;

                // Nos piden que si hay mezcla de agentes sin formaciones y con formaciones
                // Disolvamos la formacion. Por eso vemos si hay de los dos
                // Primero vemos si hay formaciones
                var inFormation = 0;
                var alone = 0;
                var mixed = false;
                foreach (var agentNpc in agentsNpc)
                {
                    switch (agentNpc.InFormation)
                    {
                        case true:
                            inFormation++;
                            break;
                        case false:
                            alone++;
                            break;
                    }

                    mixed = inFormation > 0 && alone > 0;
                    if (mixed) break;
                }


                // Tenemos que colocar a los agentes al rededor de un punto para ello
                // Calculamos cuanto serioa mas menos la circurferencia
                // Todos los personajes tienen el mismo tamaño
                var sizeBox = agentsNpc.First().RExterior;
                var agentDiameter = sizeBox * 2;


                // Extra a ojo para que no colicionen a los lados
                agentDiameter *= 1.1;

                var radio = agentDiameter * numberOfAgents / (2 * Math.PI);
                foreach (var agente in agentsNpc.Select((value, index) =>
                    new {value, index}))
                {
                    // Use x.value and x.index in here
                    var angle = (float) (agente.index * (2 * Math.PI / numberOfAgents));
                    var x = (float) (center.x + Mathf.Cos(angle) * radio);
                    var z = (float) (center.z + Mathf.Sin(angle) * radio);
                    var newTarget = new Vector3(x, center.y, +z);
                    PrintIfDebug($"{agente.index} to {newTarget}");

                    agente.value.GoToTarget(newTarget, mixed);
                }


                Done();
            }
    }


    private void MakeLine(List<AgentNpc> selected)
    {
        PrintIfDebug("Formando Cuadrado");
        selected.ForEach(agent =>
        {
            agent.ResetStateAction();
            agent.ChangeState(State.Waiting);
        });

        var leader = selected[0];
        var soldiers = selected.GetRange(1, 4);
        var formation = new Formation(leader, debug);

        // Left 

        formation.soldiers.Add(soldiers[0], new Steering(0, new Vector3(-10, 0, 0)));
        formation.soldiers.Add(soldiers[1], new Steering(0, new Vector3(-5, 0, 0)));
        // Yo
        formation.soldiers.Add(soldiers[2], new Steering(0, new Vector3(5, 0, 0)));
        formation.soldiers.Add(soldiers[3], new Steering(0, new Vector3(10, 0, 0)));


        formation.MakeFormation();
    }


    private void MakeCross(List<AgentNpc> selected)
    {
        PrintIfDebug("Formando Cuadrado");
        selected.ForEach(agent =>
        {
            agent.ResetStateAction();
            agent.ChangeState(State.Waiting);
        });

        var leader = selected[0];
        var soldiers = selected.GetRange(1, 4);
        var formation = new Formation(leader, debug);

        // Left 

        formation.soldiers.Add(soldiers[0], new Steering(0, new Vector3(-5, 0, -5)));
        formation.soldiers.Add(soldiers[1], new Steering(0, new Vector3(-5, 0, 5)));
        // Yo
        formation.soldiers.Add(soldiers[2], new Steering(0, new Vector3(5, 0, -5)));
        formation.soldiers.Add(soldiers[3], new Steering(0, new Vector3(5, 0, 5)));


        formation.MakeFormation();
    }

    private void MakeAction()
    {
        switch (action)
        {
            case CAction.None:
                break;
            case CAction.GoToTarget:
                GotoMousePosition();
                break;
            case CAction.Forming:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    // Update is called once per frame
    /**
     * Si hago click sobre un personaje lo selecciono
     * Si pulso R los pongo en estado defecto normal
     * Si pulso G quiero hacer un Go to
     * Si pulso L quiero hacer una Formación linea
     */
    private void Update()
    {
        // R -> Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetSelected.ToList().ForEach(RemoveAndResetFromSelected);
            Done();
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GetSelected.ToList().ForEach(RemoveFromSelected);
            Done();
            return;
        }


        if (Input.GetKeyDown(KeyCode.G)) // Go to target
        {
            SetWaiting();
            action = CAction.GoToTarget;
        }

        if (Input.GetKeyDown(KeyCode.L)) // MakeLine
        {
            if (GetSelected.Count < 5) return;

            var selected = GetSelected.ToList().GetRange(0, 5);

            MakeLine(selected);
        }

        if (Input.GetKeyDown(KeyCode.X)) // Make cross
        {
            if (GetSelected.Count < 5) return;

            var selected = GetSelected.ToList().GetRange(0, 5);

            MakeCross(selected);
        }

        if (action != 0)
            MakeAction();
    }

    public int GetTerrainLayer(Vector3 worldPos, Terrain t)
    {
        var index = 0;
        var scriptTerreno = FindObjectOfType<GetTerreno>();
        index = scriptTerreno.GetainTexture(worldPos, t);
        return index;
    }

    private void PrintIfDebug(string text)
    {
        if(debug) Debug.Log(text);
    } 
}