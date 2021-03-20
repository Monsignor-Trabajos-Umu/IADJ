using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class body : MonoBehaviour
{
    public float m;
    public float ev;
    public float mV;
    public float o;
    public float r;
    public float mO;
    public float mA;

    public Vector3 vv;
    public Vector3 a;


    private double DegreeToRadian(double angle)
    {
        return Math.PI * angle / 180.0;
    }
    private double RadianToDegree(double angle)
    {
        return angle * (180.0 / Math.PI);
    }

    public double getAnglePosition()
    {
       GameObject p1 = GameObject.Find("refencia1");
       GameObject p2 = GameObject.Find("refencia2");

        //Transform.position hace referencia al objeto que lo llama
        Vector3 vPersonaje = transform.position;
        Vector3 v1 = p1.transform.position;
        Vector3 v2 = p2.transform.position;

        Vector3 v1Personaje = vPersonaje - v1;
        Vector3 v12 = v2-v1;
        float v1pDotv12 = Vector3.Dot(v1Personaje, v12);

        float lv1Personaje = v1Personaje.magnitude;
        float lv12 = v12.magnitude;

        double cos = v1pDotv12 / (lv1Personaje * lv12);

        double acos = Math.Acos(cos);

        return acos;


    }

    // Start is called before the first frame update
    void Start()
    {
        double acos = getAnglePosition();
        Debug.Log(RadianToDegree(acos));
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }
}
