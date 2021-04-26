using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding : MonoBehaviour
{


    public int ancho;
    public int alto;
    public float tamCelda;
    Vector3 puntoObjetivo;
    

    // Start is called before the first frame update
    void Start()
    {
        GridMap mapa = new GridMap(ancho, alto, tamCelda);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
