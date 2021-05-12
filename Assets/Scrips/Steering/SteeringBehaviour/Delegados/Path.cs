using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path
{
    public List<Vector3> nodes { get; }

    public Path()
    {
        this.nodes = new List<Vector3>();
    }
    //TODO Ver como limitamos su crecimiento
    /* Puedo poner un tamaño maximo y cuando llegue cargarme la mitad anterior
     */
    public void AddNode(Vector3 node)
    {
        nodes.Add(node);
    }
    public Vector3 GetLast()
    {
        return nodes.Last();
    }

}
