using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Huir")]
[Help("Huye hacia la fuente curativa más cercana")]
public class Huir : BasePrimitiveAction
{
    [InParam("npcPoint")]
    public Transform position;
    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {

        var fuentes = GameObject.FindGameObjectsWithTag("puntoCurativo");

        var aux = Mathf.Infinity;
        GameObject fuente;
        foreach (GameObject f in fuentes)
        {
            if (Vector3.Distance(position.position, f.transform.position) < aux){
                fuente = f;
            }
        }
        //TODO Mover el personaje a la fuente
        
        return TaskStatus.COMPLETED;

    } // OnUpdate
}
