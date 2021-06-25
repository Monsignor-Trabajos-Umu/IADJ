using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 50.0f)] private float border;

    [SerializeField] private float speed;
    // Update is called once per frame
    private void Update()
    {
        var posVector3 = transform.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - border)
            posVector3.z += speed * Time.deltaTime;
        if (Input.GetKey("s") || Input.mousePosition.y <= border)
            posVector3.z -= speed * Time.deltaTime;
        if (Input.GetKey("a") || Input.mousePosition.x <= border)
            posVector3.x -= speed * Time.deltaTime;
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - border)
            posVector3.x += speed * Time.deltaTime;

        transform.position = posVector3;
    }
}