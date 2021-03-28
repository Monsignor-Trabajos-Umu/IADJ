using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPlayer : Agent
{

    void mover()
    {
        Vector3 Velocity = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) * this.vMaxima;
        transform.Translate(Velocity * Time.deltaTime, Space.World);
        transform.LookAt(transform.position + Velocity);
        //this.Orientacion = transform.rotation.eulerAngles.y;

    }
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        mover();
        //printDebug();
    }
}
