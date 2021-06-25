using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 50.0f)] private float border;

    [SerializeField] private bool mouse;

    [SerializeField] private float speed=5;

    // Update is called once per frame
    private void Update()
    {
        var posVector3 = transform.position;
        if (Input.GetKey("w") ||
            mouse && Input.mousePosition.y >= Screen.height - border)
            posVector3.z += speed * Time.deltaTime;
        if (Input.GetKey("s") || mouse && Input.mousePosition.y <= border)
            posVector3.z -= speed * Time.deltaTime;
        if (Input.GetKey("a") || mouse && Input.mousePosition.x <= border)
            posVector3.x -= speed * Time.deltaTime;
        if (Input.GetKey("d") || mouse && Input.mousePosition.x >= Screen.width - border)
            posVector3.x += speed * Time.deltaTime;

        transform.position = posVector3;
    }
}