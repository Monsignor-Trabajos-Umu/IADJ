using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase de GridMap basado en Grid cuadrado
public class GridMap: MonoBehaviour
{
    
    public int ancho;
    public int alto;
    public float cellSize;
    public int[,] mapa;

    Nodo[,] grid;

    public GridMap(int anchura, int altura, float tamCelda)
    {
        ancho = anchura;
        alto = altura;
        mapa = new int[anchura, altura];
        cellSize = tamCelda;

        for(int i = 0; i < mapa.GetLength(0); i++)
        {
            for (int j = 0; j < mapa.GetLength(1); j++)
            {
                Debug.Log(i + ", " + j);

            }
        }
        dibujarCasilla(Color.white);
    }

    public Vector3 getWorldPosition(Vector3 casilla)
    {
        //Del plano (u,v) al mundo (x,y) -> (x,y) = longitud del cuadrado * (u,v)
        return new Vector3(casilla.x, casilla.y, casilla.z)*cellSize;
    }

    public Vector3 getWorldPosition(int x, int y)
    {
        //Del plano (u,v) al mundo (x,y) -> (x,y) = longitud del cuadrado * (u,v)
        return new Vector3(x,y) * cellSize;
    }

    public Vector3 getCellPosition(Vector3 posicionMundo)
    {
        //Redondeamos a la cara mas cercana
        Vector3 celda = new Vector3(Mathf.Floor(posicionMundo.x / cellSize), posicionMundo.y, Mathf.Floor(posicionMundo.z / cellSize));
        return celda;
    }

    public void dibujarCasilla(Color color)
    {
        for (int i = 0; i < mapa.GetLength(0); i++)
        {
            for (int j = 0; j < mapa.GetLength(1); j++)
            {
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i, j + 1), color, cellSize);
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i + 1, j), color, cellSize);
                Vector3 v = getWorldPosition(i, j);
        
            }
        }
        Debug.DrawLine(getWorldPosition(0, alto), getWorldPosition(ancho, alto), color, cellSize);
        Debug.DrawLine(getWorldPosition(ancho, 0), getWorldPosition(ancho, alto), color, cellSize);
    }
    public void cambiarValor(int x, int y, int valor)
    {
        if(x >= 0 && y >= 0 && x < ancho && y < alto)
            mapa[x, y] = valor;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(ancho, 1 , alto));
    }

}
