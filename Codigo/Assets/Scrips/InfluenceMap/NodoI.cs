using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoI
{
    public int valor;//valor del nodo
    public int x, y; //Posición del nodo
    public Color color; //Color del nodo (para saber a que bando pertenece)
    public Vector3 worldPosition; //Posicion en el mundo

    #region Contrutores

    public NodoI()
    {
        valor = 0;
        x = 0;
        y = 0;
        color = Color.white;
        worldPosition = new Vector3();
    }

    public NodoI(int puntuacion, int xpos, int ypos)
    {
        valor = puntuacion;
        x = xpos;
        y = ypos;
        calcularColor();
    }

    public NodoI(int puntuacion, Vector3 pos)
    {
        valor = puntuacion;
        worldPosition = pos;
        calcularColor();
    }
    public NodoI(Vector3 pos)
    {
        valor = 0;
        worldPosition = pos;
        calcularColor();
    }
    public NodoI(int xpos, int ypos)
    {
        valor = 0;
        x = xpos;
        y = ypos;
        calcularColor();
    }

    #endregion
   
    public NodoI(Vector3 worldPos, int xpos, int ypos)
    {
        valor = 0;
        x = xpos;
        y = ypos;
        worldPosition = worldPos;
        calcularColor();
    }

    //Dado el valor del nodo, ajusta la tonalidad del color
    public void calcularColor()
    {
        if (valor > 0) color = Color.blue; //Casilla controlada por el equipo azul
        else if (valor < 0) color = Color.red; //Casilla controlada por el equipo rojo
        else color = Color.white; //Casilla sin controlar
    }


   
}
