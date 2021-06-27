using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuenteCurativa : MonoBehaviour
{
    //Radio en el que cura la fuente;
    Collider[] npcs;
    [SerializeField]
    public float radioCuracion = 10;
    //Curación de la fuente por segundo
    [SerializeField]
    public double curaSeg = 0.05;

    // Update is called once per frame
    void Update()
    {
        npcs = Physics.OverlapSphere(transform.position, radioCuracion);
        StartCoroutine("Curar",20000000);

    }

    void Curar()
    {
        foreach (Collider npc in npcs)
        {
            //Si se trata de un personaje y está parado, lo curamos
            if (npc.gameObject.GetComponent<Agent>() != null && npc.gameObject.GetComponent<Agent>().velocidad == 0)
            {
                npc.gameObject.GetComponent<Agent>().Curar(curaSeg);
                //npc.gameObject.GetComponent<Renderer>().material.color = Color.green;

                //Implementar alguna forma de que se vea que se curan. Por ejemplo, que salga un circulito verde
            }
        }
    }
    private void OnDrawGizmos() // Gizmo: una línea en la dirección del objetivo
    {
        Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, (float) radioCuracion);
      
      
            
    }


}
