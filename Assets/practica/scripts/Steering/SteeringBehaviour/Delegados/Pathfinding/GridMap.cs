using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase de GridMap basado en Grid cuadrado
public class GridMap
{
    public int ancho;
    public int alto;
    public float cellSize;
    public int[,] mapa;
    Vector3 posicionOrigen;

    Node[,] grid;

    public GridMap(int anchura, int altura, float tamCelda, Vector3 origen)
    {
        ancho = anchura;
        alto = altura;
        mapa = new int[anchura, altura];
        cellSize = tamCelda;
        posicionOrigen = origen;

        dibujarCasilla(Color.white);
    }

    public Vector3 getWorldPosition(Vector3 casilla)
    {
        //Del plano (u,v) al mundo (x,y) -> (x,y) = longitud del cuadrado * (u,v)
        return new Vector3(casilla.x + posicionOrigen.x, casilla.y + posicionOrigen.y, casilla.z + posicionOrigen.z)*cellSize;
    }

    public Vector3 getWorldPosition(int x, int z)
    {
        //Del plano (u,v) al mundo (x,y) -> (x,y) = longitud del cuadrado * (u,v)
        return new Vector3(x,1,z) * cellSize;
    }

    public void setValor(Vector3 worldPos, int valor)
    {
        int x, z;
        getCasilla(worldPos, out x, out z);
        setValor(x, z, valor);
    }

    public void setValor(int x, int z, int valor)
    {
        if (x >= 0 && z >= 0 && x < ancho && z < alto)
        {
            mapa[x, z] = valor;

            Debug.Log("Valor de la casilla (" + x + "," + z + ") actualizado a: " + valor);
        }
           
    }

    public int getValor(Vector3 worldPos)
    {
        int x, z;
        getCasilla(worldPos, out x, out z);
        return getValor(x, z);
    }

    public int getValor(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < ancho && z < alto)
        {
            Debug.Log("Valor de la casilla (" + x + "," + z);
            return mapa[x, z];
            
        }
        return -1;
    }

    private void getCasilla(Vector3 worldPos, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPos.x - posicionOrigen.x)/ cellSize);
        z = Mathf.FloorToInt((worldPos.z - posicionOrigen.z)/ cellSize);
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
        
            }
        }
        Debug.DrawLine(getWorldPosition(0, alto), getWorldPosition(ancho, alto), color, cellSize);
        Debug.DrawLine(getWorldPosition(ancho, 0), getWorldPosition(ancho, alto), color, cellSize);
    }

}
