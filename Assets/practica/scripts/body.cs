﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Body : MonoBehaviour
{

    // Escalar 
    public float masa;
    public float velocidad;
    public float mVelocidad;
    // public Vector3 posicion; == transform.position
    public float orientacion
    {
        get => transform.rotation.eulerAngles.y;
        set
        {
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, value);
        }
    }
    public float rotacion;
    public float mRotacion;
    public float mAngularAceleracion;
    public float mAceleracion;

    // Vector
    public Vector3 vVelocidad;
    public Vector3 vAceleracion;



    private Color colorOriginal;


    public void cambiarColor(Color c)
    {
        GetComponent<Renderer>().material.color = c;

    }

    public void ponerColorOriginal()
    {
        GetComponent<Renderer>().material.color = colorOriginal;
    }
    public void setDefaultColor()
    {
        this.colorOriginal = GetComponent<Renderer>().material.color;
        Debug.Log("color: " + colorOriginal);
    }



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
    public Vector3 OrientationToVector()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    public double MinAngleToRotate(GameObject obj)
    {
        //Transform.position hace referencia al objeto que lo llama
        Vector3 pObjeto = obj.transform.position;
        Vector3 pYo = transform.position;
        Vector3 vYoObjeto = pObjeto - pYo;
        Vector3 vYoHeading = OrientationToVector();

        // Tenemos que calular ahora si el objeto esta a la "izquierda o la derecha "
        float angle = Vector3.Angle(vYoHeading, vYoObjeto);
        bool objectIsToTheRight = Vector3.Dot(vYoObjeto, transform.right) > 0;
        if (!objectIsToTheRight)
            angle = -angle;
        return angle;
    }

    public double MinAngleToRotate(Vector3 pObjeto)
    {
        //Transform.position hace referencia al objeto que lo llama
        Vector3 pYo = transform.position;
        Vector3 vYoObjeto = pObjeto - pYo;
        Vector3 vYoHeading = OrientationToVector();

        // Tenemos que calular ahora si el objeto esta a la "izquierda o la derecha "
        float angle = Vector3.Angle(vYoHeading, vYoObjeto);
        bool objectIsToTheRight = Vector3.Dot(vYoObjeto, transform.right) > 0;
        if (!objectIsToTheRight)
            angle = -angle;
        return angle;
    }

    // Update is called once per frame

    public void printDebug()
    {
        Debug.Log("Orientacion Y " + this.orientacion);
        Debug.Log("Vector " + OrientationToVector());

    }



    void Update()
    {

    }


}
