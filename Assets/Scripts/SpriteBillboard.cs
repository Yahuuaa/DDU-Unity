using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    void Update()
    {
        transform.forward = transform.position - Camera.main.transform.position;
    }
}