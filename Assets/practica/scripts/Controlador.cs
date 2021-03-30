using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    private HashSet<GameObject> seleccionados;

    // Start is called before the first frame update
    void Awake()
    {
        seleccionados = new HashSet<GameObject>();
    }

    public HashSet<GameObject> getSeleccionados { get => seleccionados; }
    public void addOquitaSeleccion(GameObject agente)
    {
        if (seleccionados.Contains(agente))
            quitarSeleccionados(agente);
        else  
            addSeleccionados(agente);
    }
    

    private void addSeleccionados(GameObject agente)
    {
        seleccionados.Add(agente);
        Debug.Log("seleccionados: " + seleccionados.Count);
        Color c = new Color(1, 0, 0);
        agente.GetComponent<Agent>().cambiarColor(c);
    }

    private void quitarSeleccionados(GameObject agente)
    {
        seleccionados.Remove(agente);
        Debug.Log("quitar seleccionados: " + seleccionados.Count);
        agente.GetComponent<Agent>().ponerColorOriginal();
    }

    private void irPosicionRaton()
    {
        // Damos una orden cuando levantemos el botón del ratón.
        if (Input.GetMouseButtonUp(1))
        {

            // Comprobamos si el ratón golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {

                // Si lo que golpea es un punto del terreno entonces da la orden a todas las unidades NPC
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("ground"))
                {
                    Vector3 newTarget = hitInfo.point;

                    foreach (var character in this.seleccionados)
                    {
                        // Llama al método denominado "NewTarget" en TODOS y cada uno de los MonoBehaviour de este game object (npc)
                        character.SendMessage("NewTarget", newTarget);

                        // Se asume que cada NPC tiene varias componentes scripts (es decir, varios MonoBehaviour).
                        // En algunos de esos scripts está la función "NewTarget(Vector3 target)"
                        // Dicha función contendrá las instrucciones necesarias para ir o no al nuevo destino.
                        // P.e. Dejar lo que esté haciendo y  disparar a target.
                        // P.e. Si no tengo vida suficiente huir de target.
                        // P.e. Si fui seleccionado en una acción anterio y estoy a la espera de nuevas órdenes, entonces hacer un Arrive a target.

                        // Nota1: En el caso de que tu objeto tenga una estructura jerárquica, 
                        // y se quiera invocar a NewTarget de todos sus hijos, deberás usar BroadcastMessage.

                        // Nota 2: En el caso de que solo se tenga una función "NewTarget" para cada NPC, entonces 
                        // puede ser más eficiente algo como:
                        //                  npc.GetComponent<ComponenteScriptConteniendoLaFuncion>().NewTarget(newTarget);
                        // que obtiene la componente del NPC que yo sé que contiene a la función NewTarget(), y la invoca.
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        irPosicionRaton();
    }
}
