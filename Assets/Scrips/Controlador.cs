using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    private CAction action;
    public HashSet<AgentNPC> GetSelected { get; private set; }

    // Builders
    private void Awake() => GetSelected = new HashSet<AgentNPC>(); //Creamos la lista de seleccionados

    //Hemos terminado la acción
    public void Done() => action = CAction.None;

    public void ActionFinished(AgentNPC agent) => RemoveFromSelected(agent);

    public void AddOrRemoveFromSelected(AgentNPC agent)
    {
        if (GetSelected.Contains(agent))
            RemoveFromSelected(agent);
        else
            AddToSelected(agent);
    }


    public void AddToSelected(AgentNPC agent)
    {
        if (GetSelected.Contains(agent)) return;
        GetSelected.Add(agent);
        Debug.Log("seleccionados: " + GetSelected.Count);
        agent.AddToSelected();
    }

    public void RemoveFromSelected(AgentNPC agent)
    {
        if (!GetSelected.Contains(agent)) return;
        GetSelected.Remove(agent);
        agent.RemoveFromSelected();
        Debug.Log($"Quitar seleccionados: {GetSelected.Count}");
    }

    private void RemoveAndResetFromSelected(AgentNPC agent)
    {
        RemoveFromSelected(agent);
        Debug.Log($"Reset estado: {agent.name}");
        agent.ResetStateAction();
    }


    private void SetWaiting()
    {
        GetSelected.Where(agente => agente.selected)
            .ToList()
            .ForEach(agent => agent.ChangeState(State.Waiting));
    }

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
                var newTarget = hitInfo.point;

                foreach (var agente in GetSelected.Where(agente => agente.state == State.Waiting))
                    // Llamamos al Action del Agente
                    agente.GoToTarget(newTarget);
                Done();
            }
    }


    private static void MakeLine(List<AgentNPC> selected)
    {
        Debug.Log("Formando Cuadrado");
        selected.ForEach(agent =>
        {
            agent.ResetStateAction();
            agent.ChangeState(State.Waiting);
        });

        var leader = selected[0];
        var soldiers = selected.GetRange(1, 4);
        var formation = new Formation(leader);

        // Left 

        formation.soldiers.Add(soldiers[0], new Steering(0, new Vector3(-10, 0, 0)));
        formation.soldiers.Add(soldiers[1], new Steering(0, new Vector3(-5, 0, 0)));
        // Yo
        formation.soldiers.Add(soldiers[2], new Steering(0, new Vector3(5, 0, 0)));
        formation.soldiers.Add(soldiers[3], new Steering(0, new Vector3(10, 0, 0)));


        formation.MakeFormation();
    }


    private static void MakeCross(List<AgentNPC> selected)
    {
        Debug.Log("Formando Cuadrado");
        selected.ForEach(agent =>
        {
            agent.ResetStateAction();
            agent.ChangeState(State.Waiting);
        });

        var leader = selected[0];
        var soldiers = selected.GetRange(1, 4);
        var formation = new Formation(leader);

        // Left 

        formation.soldiers.Add(soldiers[0], new Steering(0, new Vector3(-10, 0, -10)));
        formation.soldiers.Add(soldiers[1], new Steering(0, new Vector3(-10, 0, 10)));
        // Yo
        formation.soldiers.Add(soldiers[2], new Steering(0, new Vector3(10, 0, -10)));
        formation.soldiers.Add(soldiers[3], new Steering(0, new Vector3(10, 0, 10)));


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
}