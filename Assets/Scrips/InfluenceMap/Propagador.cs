using UnityEngine;

public interface IPropagator
{
    NodoI GridPosition { get; }
    int Value { get; }
    int Radio { get; }
}

public class Propagador : MonoBehaviour, IPropagator
{
    [SerializeField,Range(0,10)] private int radio;

    [SerializeField] private int influencia;

    [SerializeField] private InfluenceMapControl mapa;

    public int Value => influencia;
    public int Radio => radio;


    public NodoI GridPosition => mapa.GetGridPosition(transform.position);

    // Use this for initialization
    private void Start()
    {
        if (mapa != null && mapa.gameObject.activeSelf)
            mapa.RegisterPropagator(this);
    }
}