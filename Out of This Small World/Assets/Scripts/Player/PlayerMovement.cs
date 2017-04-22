// Date   : 22.04.2017 07:40
// Project: Out of This Small World
// Author : bradur

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    private Vector3 direction;
    private Rigidbody rb2D;

    [Range(1f, 10f)]
    [SerializeField]
    private float forwardSpeed = 1f;

    [Range(1f, 10f)]
    [SerializeField]
    private float backwardSpeed = 1f;

    [Range(3f, 10f)]
    [SerializeField]
    private float rotationSpeed = 6f;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        transform.Rotate(0f, 0f, -horizontalAxis * rotationSpeed);

        float verticalAxis = Input.GetAxis("Vertical");

        if (verticalAxis > 0.55f)
        {
            direction = transform.right * forwardSpeed;
        } else if (verticalAxis < -0.05f)
        {
            direction = -transform.right * backwardSpeed;
        } else
        {
            direction = Vector3.zero;
        }

        rb2D.velocity = direction;
    }
}
