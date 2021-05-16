using UnityEngine;
using System.Collections;

public interface IPropagator
{
    NodoI GridPosition { get; }
    int Value { get; }
}

public class Propagador : MonoBehaviour, IPropagator
{
    [SerializeField]
    int influencia;
    public int Value { get { return influencia; } }

    [SerializeField]
    InfluenceMapControl mapa;

    public NodoI GridPosition
    {
        get
        {
            return mapa.GetGridPosition(transform.position);
        }
    }

    // Use this for initialization
    void Start()
    {
        mapa.RegisterPropagator(this);
    }
}