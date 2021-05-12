using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    private readonly Color cSelecionado = new Color(1, 0, 0);
    private int accion;

    public HashSet<GameObject> getSeleccionados { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        getSeleccionados = new HashSet<GameObject>();
    }

    public void addOquitaSeleccion(GameObject agente)
    {
        if (getSeleccionados.Contains(agente))
            quitarSeleccionados(agente);
        else
            addSeleccionados(agente);
    }


    private void addSeleccionados(GameObject agente)
    {
        getSeleccionados.Add(agente);
        Debug.Log("seleccionados: " + getSeleccionados.Count);
        agente.GetComponent<Agent>().cambiarColor(cSelecionado);
    }

    private void quitarSeleccionados(GameObject agente)
    {
        getSeleccionados.Remove(agente);
        Debug.Log("quitar seleccionados: " + getSeleccionados.Count);
        agente.GetComponent<Agent>().ponerColorOriginal();
        //TODO reactivar steering
    }

    private void actualizaColor(GameObject agente, Color c)
    {
        agente.GetComponent<Agent>().cambiarColor(c);
    }

    public void accionTermianda(GameObject agente)
    {
        accion = 0;
        agente.GetComponent<Agent>().cambiarColor(cSelecionado);
    }

    private void irPosicionRaton()
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

                    foreach (var character in getSeleccionados)
                        // Llama al método denominado "NewTarget" en TODOS y cada uno de los MonoBehaviour de este game object (npc)
                        character.SendMessage("NewTarget", newTarget);
                }
        }
    }


    private void FormarCuadrado()
    {
        Debug.Log("Formando Cuadrado");
        var selecionados = getSeleccionados.ToList().GetRange(0, 4);
        var lider = selecionados[0];
        var peloton = selecionados.GetRange(1, 4);
        foreach (var soldado in peloton) soldado.SendMessage("DesactivaSteering");
    }

    private void RealizaAccion()
    {
        switch (accion)
        {
            case 1:
                irPosicionRaton();
                break;
            case 2:
                FormarCuadrado();
                break;
        }
    }

    private void ResetAccion()
    {
        if (accion == 2)
        {
            Debug.Log("Deformando");
            getSeleccionados.ToList().ForEach(o => o.SendMessage("ActivaSteering"));
        }

        accion = 0;
        getSeleccionados.ToList().ForEach(accionTermianda);
    }

    // Update is called once per frame
    private void Update()
    {
        // Veo si quiero resetearlos
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAccion();
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            getSeleccionados.ToList().ForEach(g => actualizaColor(g, Color.magenta));
            accion = 1;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            getSeleccionados.ToList().ForEach(g => actualizaColor(g, Color.yellow));
            accion = 2;
        }

        if (accion != 0)
            RealizaAccion();
    }
}