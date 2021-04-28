using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding : MonoBehaviour
{

    public int ancho;
    public int alto;
    public float tamCelda;
    Vector3 puntoObjetivo;
    private GridMap mapa;

    // Start is called before the first frame update
    void Start()
    {
        mapa = new GridMap(ancho, alto, tamCelda, new Vector3(100,0,10));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mapa.setValor(worldPosition, 56);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mapa.getValor(worldPosition));
        }
    }


}
