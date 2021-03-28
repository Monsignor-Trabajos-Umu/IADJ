using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    private HashSet<GameObject> seleccionados;

    // Start is called before the first frame update
    void Start()
    {
        seleccionados = new HashSet<GameObject>();
    }
    
    public void addSeleccionados(GameObject agente)
    {
        seleccionados.Add(agente);
        Debug.Log("seleccionados: " + seleccionados.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
