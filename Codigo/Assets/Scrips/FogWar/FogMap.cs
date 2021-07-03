using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMap : InfluenceMap
{
    public FogMap(InfluenceGrid g, int distancia, int w, int h) : base(g, distancia, w, h)
    {
    }

    //Hacemos que los propagadores que no estén en la zona visible sean invisibles.
    public void FogWar()
    {
        foreach(var p in propagadores)
        {
            if(p.GridPosition.valor <= 0)
            {
                p.Object.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                p.Object.GetComponent<MeshRenderer>().enabled = true;

                var colliders = Physics.OverlapSphere(p.Object.transform.position, maxPropagacion * p.Radio);
                //Debug.Log("Numero Colisiones:" + colliders.Length);
                foreach(var c in colliders)
                {
                    var renderer = c.gameObject.GetComponent<MeshRenderer>();
                    if (renderer != null)
                        renderer.enabled = true;
                }
            }
        }
    }

    public new void SetInfluence(NodoI nodo, int value)
    {
        if (nodo.x < ancho && nodo.y < alto)
        {
            grid.getGrid[nodo.x, nodo.y].valor = Mathf.Max(0,value);
        }
    }

    public new void SetInfluence(NodoI nodo, int value, int radio)
    {
        // Ponemos el central
        SetInfluence(nodo, value);
        // Influimos a los que estan al rededor
        var vecinos = grid.GetNeighbors(nodo, radio);
        vecinos.ForEach(i => SetInfluence(i, value));


    }

    public new void SetInfluence(int x, int y, int value)
    {
        if (x < ancho && y < alto)
        {
            grid.getGrid[x, y].valor = Mathf.Max(0, value);
        }
    }

    new void  UpdatePropagators()
    {
        foreach (IPropagator p in propagadores)
        {
            SetInfluence(p.GridPosition, Mathf.Max(0,p.Value), p.Radio);
        }
    } 
    // Propaga en cruz
    protected new void UpdatePropagation()
    {
        NodoI bottomRight = grid.getBottomRight();
        foreach (IPropagator p in propagadores)
        {
            NodoI pos = p.GridPosition;
            for (int i = 1; i <= maxPropagacion; i++)
            {
                //Actualizamos la casilla arriba
                if (pos.x - i >= 0)
                    grid.getGrid[pos.x - i, pos.y].valor = Mathf.Max(0, p.Value - i);
                //Actualizamos la casilla de abajo
                if (pos.x + i < bottomRight.x)
                    grid.getGrid[pos.x + i, pos.y].valor = Mathf.Max(0, p.Value - i);
                //Actualizamos la casilla derecha
                if (pos.y + i < bottomRight.y)
                    grid.getGrid[pos.x, pos.y + i].valor = Mathf.Max(0, p.Value - i);
                //Actualizamos la casilla izquierda
                if (pos.y - i >= 0)
                    grid.getGrid[pos.x, pos.y - i].valor = Mathf.Max(0, p.Value - i);
            }
        }
    }
}
