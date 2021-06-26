using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMapControl : MonoBehaviour
{
    //Distancia máxima a la que se puede propagar
    [SerializeField]
    int propagacion;

    [SerializeField]
    int frecuenciaUpdate = 3;

    FogMap mapaNiebla;

    [SerializeField]
    InfluenceGrid gridMap;

    void CreateMap()
    {
        // how many of gridsize is in Mathf.Abs(_upperRight.positon.x - _bottomLeft.position.x)
        int width = (int)gridMap.gridWorldSizeX;
        int height = (int)gridMap.gridWorldSizeX;

        Debug.Log(width + " x " + height);

        mapaNiebla = new FogMap(gridMap, propagacion, width, height);

    }

    public void RegisterPropagator(IPropagator p)
    {
        mapaNiebla.RegisterPropagator(p);
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
        mapaNiebla.Propagate();
        mapaNiebla.FogWar();
    }

    void SetInfluence(int x, int y, int value)
    {
        mapaNiebla.SetInfluence(x, y, value);
    }

    void SetInfluence(NodoI pos, int value)
    {
        mapaNiebla.SetInfluence(pos, value);
    }

}
