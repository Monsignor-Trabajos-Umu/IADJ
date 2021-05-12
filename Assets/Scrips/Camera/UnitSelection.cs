using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public RectTransform selectionBox;
    public LayerMask unidLayerMask;

    private List<AgentNPC> unidades = new List<AgentNPC>();
    private Vector2 posicionInicial;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
    }


    void UpdateSelectionBox(Vector2 cursor)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float ancho = cursor.x - posicionInicial.x;
        float alto = cursor.y - posicionInicial.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(ancho), Mathf.Abs(alto));
        selectionBox.anchoredPosition = posicionInicial + new Vector2(ancho / 2, alto / 2);
    }
}
