using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Body : MonoBehaviour
{
    public float masa;
    public float velocidad;
    public float vMaxima;
    public float orientacion;
    public float rotacion;
    public float mRotacion;
    public float mAceleracion;

    public Vector3 vVelocidad;
    public Vector3 vAceleracion;


    private double DegreeToRadian(double angle)
    {
        return Math.PI * angle / 180.0;
    }
    private double RadianToDegree(double angle)
    {
        return angle * (180.0 / Math.PI);
    }

    /* Calcula el angulo A entre 3 puntos
     *  
     */
    private double Angulo3PuntosGrados(Vector3 vPersonaje, Vector3 v1, Vector3 v2)
    {

        Vector3 v1Personaje = vPersonaje - v1;
        Vector3 v12 = v2 - v1;
        float v1pDotv12 = Vector3.Dot(v1Personaje, v12);

        float lv1Personaje = v1Personaje.magnitude;
        float lv12 = v12.magnitude;

        double cos = v1pDotv12 / (lv1Personaje * lv12);

        double acos = Math.Acos(cos);

        return RadianToDegree(acos);
    }
    public double PositionToAngle()
    {
        GameObject p1 = GameObject.Find("refencia1");
        GameObject p2 = GameObject.Find("refencia2");

        //Transform.position hace referencia al objeto que lo llama
        Vector3 vPersonaje = transform.position;
        Vector3 v1 = p1.transform.position;
        Vector3 v2 = p2.transform.position;

        return Angulo3PuntosGrados(vPersonaje, v1, v2);


    }
    Vector3 OrientationToAngle()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    double MinAngleToRotate(GameObject obj)
    {
        //Transform.position hace referencia al objeto que lo llama
        Vector3 pObjeto = obj.transform.position;
        Vector3 pYo = transform.position;
        Vector3 vYoObjeto = pObjeto - pYo;
        Vector3 vYoHeading = OrientationToAngle();

        return Vector3.Angle(vYoHeading, vYoObjeto);
    }
    // Start is called before the first frame update
    void Start()
    {
        double acos = PositionToAngle();
        Debug.Log(acos);

        Debug.Log(transform.forward);
        Debug.Log(transform.TransformDirection(Vector3.forward));
        Debug.Log(MinAngleToRotate(GameObject.Find("Ob2")));


    }

    // Update is called once per frame



    void Update()
    {
        float horizontalSpeed = 2.0f;

        Debug.Log("Rotacion " + transform.rotation);
        Debug.Log("Vector " + OrientationToAngle());
        Debug.Log("Orientacion objeto 2 " + MinAngleToRotate(GameObject.Find("Ob2")));

        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);
    }


}
