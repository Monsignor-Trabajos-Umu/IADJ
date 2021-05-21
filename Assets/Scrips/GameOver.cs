using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver: MonoBehaviour
{
    GameObject baseRoja;
    GameObject baseAzul;

    //imagen del canvas a visualizar
    [SerializeField]
    GameObject gameOver;
    //private bool finJuego = false;
    // Start is called before the first frame update
    void Start()
    {
        baseRoja = GameObject.FindGameObjectWithTag("baseRoja");
        baseAzul = GameObject.FindGameObjectWithTag("baseAzul");
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine("CallGameOver", 200000);
    }
    
    public void CallGameOver()
    {
        var r1 = baseRoja.GetComponent<Agent>();
        var a1 = baseAzul.GetComponent<Agent>();
        if(r1 != null && a1 != null)
            if (r1.vida <=0 || a1.vida <= 0)
                gameOver.SetActive(true);
    }
}
