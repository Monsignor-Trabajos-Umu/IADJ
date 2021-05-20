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
            if (npc.gameObject.GetComponent<Agent>() != null)
            {
                npc.gameObject.GetComponent<Agent>().Curar(curaSeg);
                npc.gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
}
