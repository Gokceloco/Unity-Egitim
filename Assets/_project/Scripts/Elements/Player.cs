using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public Rigidbody rb;
    public Transform cameraHolder;

    public float cameraSmoothTime;

    Vector3 velocity;

    public Vector2 turn;
    public float sensitivity;

    void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;

        float tempSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            tempSpeed = speed * 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        rb.position += direction.normalized * tempSpeed * Time.fixedDeltaTime;
        cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position, transform.position, ref velocity, cameraSmoothTime);

        
    }
}
