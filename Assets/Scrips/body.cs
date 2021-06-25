using System;
using UnityEngine;

public class Body : MonoBehaviour
{
    public int
        alcance = 1; //Max de casillas de distancia para golpear. Por defecto vale 1

    private GameObject currentHat;
    public int daño = 2; //daño de la unidad por hit. Por defecto vale 2
    private GameObject fatherHats;
    private GameObject headBands;


    public float mAcceleration;
    public float mAngularAcceleration;

    // Escalar 
    public float masa;
    public float mRotation;
    public float mVelocity;
    public float baseVelocity;
    // Visual clues
    private Color originalBandColor;
    private Color originalColor;


    public float rotacion;
    public Vector3 vAceleracion;
    public float velocidad;

    public double
        vida = 100; //Vida actual. Por defecto se inicializa al valor de la vida máxima

    public double vidaMaxima = 100; //Vida Máxima. Por defecto 100 puntos de salud

    // Vector
    public Vector3 vVelocidad;

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


    // Start
    protected virtual void Start()
    {
        baseVelocity = mVelocity;
        try
        {
            var temp = transform.Find("Hats");
            fatherHats = temp.gameObject;

            temp = transform.Find("Headband");
            headBands = temp.gameObject;
        }
        catch (Exception)
        {
            Debug.LogWarning("No hay hats ");
        }
    }


    protected void SetColor(Color c)
    {
        GetComponent<Renderer>().material.color = c;
    }

    protected void SetColorHeadBand(Color c)
    {
        headBands.GetComponent<Renderer>().material.color = c;
    }

    protected void SaveOriginalColor()
    {
        originalColor = GetComponent<Renderer>().material.color;
        originalBandColor = headBands.GetComponent<Renderer>().material
            .color;
        Debug.Log($"Color original {originalColor} Banda {originalBandColor}");
    }

    protected void ResetVisualStatus()
    {
        SetColor(originalColor);
        SetColorHeadBand(originalBandColor);
        SetHat(HatsTypes.None);
    }

    protected void SetHat(HatsTypes hat)
    {
        void EnableHat(string path)
        {
            //Debug.Log($"Enabling hat {path}");
            var temp = fatherHats.transform.Find(path);
            currentHat = temp.gameObject;
            currentHat.SetActive(true);
        }

        void Unload()
        {
            //Debug.Log("Disabling hats");
            if (currentHat != null) currentHat.SetActive(false);
        }


        const string none = "NONE";
        var newPath = none;
        newPath = hat switch
        {
            HatsTypes.None => none,
            HatsTypes.CowBoy => "CowboyHat",
            HatsTypes.Crown => "Crown",
            HatsTypes.Magician => "MagicianHat",
            HatsTypes.Miner => "MinerHat",
            HatsTypes.Mustache => "Mustache",
            HatsTypes.Pajama => "PajamaHat",
            HatsTypes.Pillbox => "PillboxHat",
            HatsTypes.Police => "PoliceCap",
            HatsTypes.Shower => "ShowerCap",
            HatsTypes.Sombrero => "Sombrero",
            HatsTypes.Viking => "VikingHelmet",
            _ => throw new NotImplementedException()
        };

        if (newPath == none)
            Unload();
        else
            EnableHat(newPath);
    }


    private double DegreeToRadian(double angle) => Math.PI * angle / 180.0;

    private double RadianToDegree(double angle) => angle * (180.0 / Math.PI);

    /* Calcula el angulo A entre 3 puntos
     *  
     */
    private double Angulo3PuntosGrados(Vector3 vPersonaje, Vector3 v1, Vector3 v2)
    {
        var v1Personaje = vPersonaje - v1;
        var v12 = v2 - v1;
        var v1pDotv12 = Vector3.Dot(v1Personaje, v12);

        var lv1Personaje = v1Personaje.magnitude;
        var lv12 = v12.magnitude;

        double cos = v1pDotv12 / (lv1Personaje * lv12);

        var acos = Math.Acos(cos);

        return RadianToDegree(acos);
    }

    public double PositionToAngle()
    {
        var p1 = GameObject.Find("refencia1");
        var p2 = GameObject.Find("refencia2");

        //Transform.position hace referencia al objeto que lo llama
        var vPersonaje = transform.position;
        var v1 = p1.transform.position;
        var v2 = p2.transform.position;

        return Angulo3PuntosGrados(vPersonaje, v1, v2);
    }

    public Vector3 OrientationToVector() => transform.TransformDirection(Vector3.forward);

    private double CalculateAngleToRate(Vector3 vYoHeading, Vector3 vYoObjeto)
    {
        // Tenemos que calular ahora si el objeto esta a la "izquierda o la derecha "
        var angle = Vector3.Angle(vYoHeading, vYoObjeto);
        var objectIsToTheRight = Vector3.Dot(vYoObjeto, transform.right) > 0;
        if (!objectIsToTheRight)
            angle = -angle;
        return angle;
    }

    public double MinAngleToRotate(Vector3 pObjeto)
    {
        //Transform.position hace referencia al objeto que lo llama
        var pYo = transform.position;
        var vYoObjeto = pObjeto - pYo;
        var vYoHeading = OrientationToVector();

        return CalculateAngleToRate(vYoHeading, vYoObjeto);
    }

    public double MinAngleToRotate(GameObject obj) =>
        MinAngleToRotate(obj.transform.position);

    public double MinAngleToRotateVector(Vector3 direction)
    {
        //Transform.position hace referencia al objeto que lo llama
        var vYoObjeto = direction;
        var vYoHeading = OrientationToVector();

        return CalculateAngleToRate(vYoHeading, vYoObjeto);
    }

    /* Calcula el minimo angulo para darle la "espalda" al objeto
     *  Es igual que MinAngleToRotate pero aplicamos una matriz de
     *  transformacion de 180
     */
    public double MinAngleToRotate180(GameObject obj)
    {
        //Transform.position hace referencia al objeto que lo llama
        var pObjeto = obj.transform.position;
        var pYo = transform.position;
        var vYoObjeto = pObjeto - pYo;
        vYoObjeto.x *= -1; // -1  0  0 
        vYoObjeto.y *= 1; //  0  1  0
        vYoObjeto.z *= -1; //  0  0 -1
        var vYoHeading = OrientationToVector();

        return CalculateAngleToRate(vYoHeading, vYoObjeto);
    }

    //Cura una cantidad de vida al personaje si no tiene la vida al máximo
    public void Curar(double cantidad)
    {
        if (vida < vidaMaxima) vida += cantidad;
    }

    // Update is called once per frame

    public void printDebug()
    {
        Debug.Log("Orientacion Y " + orientacion);
        Debug.Log("Vector " + OrientationToVector());
    }
}