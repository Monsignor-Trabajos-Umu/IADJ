using UnityEngine;

public class InfluenceMapControl : MonoBehaviour
{
    [SerializeField] private readonly int frecuenciaUpdate = 3;

    [SerializeField] private InfluenceGrid gridMap;

    private InfluenceMap mapaInfluencia;

    //Distancia m�xima a la que se puede propagar
    [SerializeField] private int propagacion;

    private void CreateMap()
    {
        // how many of gridsize is in Mathf.Abs(_upperRight.positon.x - _bottomLeft.position.x)
        var width = (int) gridMap.gridWorldSizeX;
        var height = (int) gridMap.gridWorldSizeZ;

        Debug.Log(width + " x " + height);

        mapaInfluencia = new InfluenceMap(gridMap, propagacion, width, height);
    }

    public void RegisterPropagator(IPropagator p)
    {
        mapaInfluencia.RegisterPropagator(p);
    }

    public NodoI GetGridPosition(Vector3 pos) => gridMap.GetNodeFromWorldPoint(pos);

    private void Awake()
    {
        CreateMap();

        InvokeRepeating("PropagationUpdate", 0.001f, 1.0f / frecuenciaUpdate);
    }

    private void PropagationUpdate()
    {
        mapaInfluencia.Propagate();
    }

    private void SetInfluence(int x, int y, int value)
    {
        mapaInfluencia.SetInfluence(x, y, value);
    }

    private void SetInfluence(NodoI pos, int value)
    {
        mapaInfluencia.SetInfluence(pos, value);
    }


    public float GetInfluence(int x, int y) => mapaInfluencia.GetValue(x, y);
    
}