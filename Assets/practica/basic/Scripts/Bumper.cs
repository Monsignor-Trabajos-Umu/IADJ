using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name.Equals("Frontera")) // ¿Colisiona con el objeto "Frontera"?
            transform.root.position = Vector3.zero;  // Reset posición del padre.
    }
}
