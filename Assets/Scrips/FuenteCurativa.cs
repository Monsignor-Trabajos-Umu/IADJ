using System.Collections;
using System.Linq;
using UnityEngine;

public class FuenteCurativa : MonoBehaviour
{
    //Curación de la fuente por segundo
    [SerializeField] [Range(0, 5)] public double curaSeg = 0.05;

    [SerializeField] [Range(1, 20)] public float multiplier;

    //Radio en el que cura la fuente;
    [SerializeField] public float radioCuracion = 10;
    [SerializeField] public bool debug;
    private float actualRange => radioCuracion * multiplier;

    // Update is called once per frame


    private void Start()
    {
        StartCoroutine(ICura());
    }

    private IEnumerator ICura()
    {
        for (;;)
        {
            Cura();
            yield return new WaitForSeconds(1);
        }
    }


    private void Cura()
    {
        var npcs = Physics.OverlapSphere(transform.position, actualRange).Where(col =>
            col.gameObject.GetComponent<Agent>() != null &&
            Mathf.Approximately(col.gameObject.GetComponent<Agent>().velocidad, 0 )).ToList();

        Debug.Log($"OJOOOOO {npcs.Count}");

        npcs.ForEach(npc => { npc.gameObject.GetComponent<Agent>().Curar(curaSeg); });
    }

    private void OnDrawGizmos() // Gizmo: una línea en la dirección del objetivo
    {
        if(!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, actualRange);
    }
}