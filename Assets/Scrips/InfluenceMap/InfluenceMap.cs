using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InfluenceMap
{
    List<IPropagator> propagadores = new List<IPropagator>();
    int ancho, alto;
    InfluenceGrid grid;
    //Número de casillas de distancia de influencia
    //Si por ejemplo es 2, un propagador no influyen en más de dos casillas de distancia
    int maxPropagacion;

    public float GetValue(int x, int y)
    {
        return grid.getGrid[x, y].valor;
    }

    public InfluenceMap(InfluenceGrid g, int distancia, int w, int h)
    {
        grid = g;
        maxPropagacion = distancia;
        ancho = w;
        alto = h;
    }

    public void SetInfluence(NodoI nodo, int value)
    {
        if (nodo.x < ancho && nodo.y < alto)
        {
            grid.getGrid[nodo.x, nodo.y].valor = value;
        }
    }

    public void SetInfluence(int x, int y, int value)
    {
        if (x < ancho && y < alto)
        {
            grid.getGrid[x, y].valor = value;
        }
    }

    public void RegisterPropagator(IPropagator p)
    {
        propagadores.Add(p);
    }

    public void Propagate()
    {
        UpdatePropagators();
        UpdatePropagation();
    }

    void UpdatePropagators()
    {
        foreach (IPropagator p in propagadores)
        {
            SetInfluence(p.GridPosition, p.Value);
        }
    }

    void UpdatePropagation()
    {
        NodoI bottomRight = grid.getBottomRight();
        foreach (IPropagator p in propagadores)
        {
            NodoI pos = p.GridPosition;
            for (int i = 1; i <= maxPropagacion; i++) {
                //Actualizamos la casilla arriba
                if(pos.x - i >= 0)
                    grid.getGrid[pos.x - i, pos.y].valor += p.Value;
                //Actualizamos la casilla de abajo
                if (pos.x + i < bottomRight.x)
                    grid.getGrid[pos.x + i, pos.y].valor += p.Value;
                //Actualizamos la casilla derecha
                if (pos.y + i < bottomRight.y)
                    grid.getGrid[pos.x, pos.y + i].valor += p.Value;
                //Actualizamos la casilla izquierda
                if (pos.y - i >= 0)
                    grid.getGrid[pos.x, pos.y - i].valor += p.Value;
            }
        }
    }

}