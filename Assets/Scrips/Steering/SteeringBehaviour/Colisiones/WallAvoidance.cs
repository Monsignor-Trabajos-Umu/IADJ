using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallAvoidance : MonoBehaviour
{

    // Distancia minima a la pared
    public float avoidDistance;
    // Distancia del rayo
    public float lookAhead;

    //Lista de posibles colisiones
    List<GameObject> colisiones;


}
