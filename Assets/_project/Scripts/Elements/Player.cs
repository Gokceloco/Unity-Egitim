using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ControlStyle controlStyle;

    public float speed;

    public Rigidbody rb;
    public Transform cameraHolder;

    public float cameraSmoothTime;

    Vector3 velocity;

    public Vector2 turn;
    public float sensitivity;

    public float jumpPower;
    public LayerMask jumpLayerMask;

    private void Start()
    {
        if (controlStyle == ControlStyle.ThirdPerson)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, 1f, jumpLayerMask))
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
            }
        }
    }

    void FixedUpdate()
    {
        if (controlStyle == ControlStyle.PlayerLooksToMouse)
        {
            Vector3 direction = Vector3.zero;

            float tempSpeed = speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                tempSpeed = speed * 2;
            }
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.forward;
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
            cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position,
                transform.position, ref velocity, cameraSmoothTime);
        }
        else if (controlStyle == ControlStyle.ThirdPerson)
        {
            turn.x += Input.GetAxis("Mouse X");
            turn.y += Input.GetAxis("Mouse Y");

            transform.rotation = Quaternion.Euler(0, turn.x * sensitivity, 0);
            cameraHolder.rotation = Quaternion.Euler(-turn.y * sensitivity, turn.x * sensitivity, 0);

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
                direction += -transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += -transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += transform.right;
            }

            rb.position += direction.normalized * tempSpeed * Time.fixedDeltaTime;
            cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position,
                transform.position, ref velocity, cameraSmoothTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}

public enum ControlStyle
{
    PlayerLooksToMouse,
    ThirdPerson,
    FPS,
}