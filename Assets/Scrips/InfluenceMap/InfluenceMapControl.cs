using UnityEngine;
using System.Collections;

public class InfluenceMapControl : MonoBehaviour
{

    //Distancia máxima a la que se puede propagar
    [SerializeField]
    int propagacion;

    [SerializeField]
    int frecuenciaUpdate = 3;

    InfluenceMap mapaInfluencia;

    [SerializeField]
    InfluenceGrid gridMap;

    void CreateMap()
    {
        // how many of gridsize is in Mathf.Abs(_upperRight.positon.x - _bottomLeft.position.x)
        int width = (int)gridMap.gridWorldSizeX;
        int height = (int)gridMap.gridWorldSizeX;

        Debug.Log(width + " x " + height);

        mapaInfluencia = new InfluenceMap(gridMap, propagacion, width, height);

    }

    public void RegisterPropagator(IPropagator p)
    {
        mapaInfluencia.RegisterPropagator(p);
    }

    public NodoI GetGridPosition(Vector3 pos)
    {
        return gridMap.GetNodeFromWorldPoint(pos);
    }


    void Awake()
    {
        CreateMap();

        InvokeRepeating("PropagationUpdate", 0.001f, 1.0f / frecuenciaUpdate);
    }

    void PropagationUpdate()
    {
        mapaInfluencia.Propagate();
    }

    void SetInfluence(int x, int y, int value)
    {
        mapaInfluencia.SetInfluence(x, y, value);
    }

    void SetInfluence(NodoI pos, int value)
    {
        mapaInfluencia.SetInfluence(pos, value);
    }

}
