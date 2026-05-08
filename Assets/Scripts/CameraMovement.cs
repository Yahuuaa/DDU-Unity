using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float panSpeed = 20f;
    public float scrollThickness = 20f;

    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    public float rotationSpeed = 100f;

    void Update()
    {
        HandlePan();
        HandleZoom();
        HandleRotation();
    }

    void HandlePan()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    move += transform.forward;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  move -= transform.forward;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  move -= transform.right;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move += transform.right;
        
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < scrollThickness)                    move -= transform.right;
        if (mousePos.x > Screen.width - scrollThickness)     move += transform.right;
        if (mousePos.y < scrollThickness)                    move -= transform.forward;
        if (mousePos.y > Screen.height - scrollThickness)    move += transform.forward;
        
        move.y = 0;
        transform.position += move * panSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
        transform.position = pos;
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up,  -rotationSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}