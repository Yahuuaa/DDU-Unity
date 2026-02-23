using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    
    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        float moveX = Keyboard.current.dKey.isPressed ? 1f : Keyboard.current.aKey.isPressed ? -1f : 0f;
        float moveY = Keyboard.current.wKey.isPressed ? 1f : Keyboard.current.sKey.isPressed ? -1f : 0f;
        
        Vector3 movement = new Vector3(moveX, moveY, 0) * Time.deltaTime * 5f;
        Camera.main.transform.Translate(movement);
        
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            yaw += mouseDelta.x * mouseSensitivity;
            pitch -= mouseDelta.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
            
            GetComponent<Camera>().transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
