using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/* Si un gameObject tiene este script 
 * guardara su path en una lista
 *
 */
public class Patheable : MonoBehaviour
{
    public float saveRate;
    [Range(0.0f, 5.0f)]
    public float separacion;
    [System.NonSerialized]
    public Path path;
    [SerializeField]
    private int pathSize;
    // Start is called before the first frame update
    void Start()
    {
        this.path = new Path();
        this.path.AddNode(this.transform.position);
        // Llamamos a UpdatePath
        InvokeRepeating("UpdatePath", 1.0f, saveRate);
    }

    private void UpdatePath()
    {
        Vector3 lastPosition = path.GetLast();
        Vector3 current = this.transform.position;

        if (Vector3.Distance(lastPosition, current) > separacion)
        {
            Debug.Log("Last -> " + lastPosition + " | Current -> " + current);
            path.AddNode(current);
            this.pathSize = this.path.nodes.Count();
        }


    }
}
