using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    private int action;
    public HashSet<Agent> GetSelected { get; private set; }

    // Builders
    private void Awake()
    {
        GetSelected = new HashSet<Agent>(); //Creamos la lista de seleccionados
    }

    public void AddOrRemoveFromSelected(Agent agente)
    {
        if (GetSelected.Contains(agente))
            RemoveFromSelected(agente);
        else
            AddToSelected(agente);
    }


    private void AddToSelected(Agent agente)
    {
        GetSelected.Add(agente);
        Debug.Log("seleccionados: " + GetSelected.Count);
        agente.MakeState(State.Selected);
    }

    private void RemoveFromSelected(Agent agente)
    {
        GetSelected.Remove(agente);
        Debug.Log("quitar seleccionados: " + GetSelected.Count);
        agente.MakeState(State.Normal);
        //TODO reactivar steering
    }


    private void GotoMousePosition()
    {
        // Damos una orden cuando levantemos el botón del ratón.
        if (Input.GetMouseButtonUp(1))
        {
            // Comprobamos si el ratón golpea a algo en el escenario.
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
                // Si lo que golpea es un punto del terreno entonces da la orden a todas las unidades NPC
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("ground"))
                {
                    var newTarget = hitInfo.point;

                    foreach (var agente in GetSelected)
                        // Llamamos al Action del Agente
                        agente.GoToTarget(newTarget);
                }
        }
    }


    private void FormarCuadrado()
    {
        Debug.Log("Formando Cuadrado");
        var selecionados = GetSelected.ToList().GetRange(0, 4);
        var lider = selecionados[0];
        var peloton = selecionados.GetRange(1, 4);
        foreach (var soldado in peloton) soldado.SendMessage("DesactivaSteering");
    }

    private void MakeAction()
    {
        switch (action)
        {
            case 1:
                GotoMousePosition();
                break;
            case 2:
                FormarCuadrado();
                break;
        }
    }

   

    // Update is called once per frame
    /**
     * Si hago click sobre un personaje lo selecciono
     * Si pulso R los pongo en estado defecto normal
     * Si pulso G quiero hacer un Go to
     */
    private void Update()
    {
        // R -> Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetSelected.ToList().ForEach(RemoveFromSelected);
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GetSelected.ToList().ForEach(agente => agente.MakeState(State.Waiting));
            action = 1;
        }

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    GetSelected.ToList().ForEach(g => actualizaColor(g, Color.yellow));
        //    cAction = 2;
        //}

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