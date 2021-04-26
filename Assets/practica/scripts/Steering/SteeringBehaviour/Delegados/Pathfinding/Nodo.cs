using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo
{
    public bool alcanzable; //Decide si la casilla se puede atravesar
    public Vector3 posMundo; //valor de la casilla

    // Start is called before the first frame update
    public Nodo(bool atravesar, Vector3 world)
    {
        alcanzable = atravesar;
        posMundo = world;
        
    }
    public Nodo()
    {
        alcanzable = true;
        posMundo = new Vector3(0, 0, 0);
    }
}
