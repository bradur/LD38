// Date   : 22.04.2017 07:40
// Project: Out of This Small World
// Author : bradur

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    private Vector3 direction;
    private Rigidbody rigidBody;

    [Range(20f, 300f)]
    [SerializeField]
    private float forwardSpeed = 1f;

    [Range(20f, 300f)]
    [SerializeField]
    private float backwardSpeed = 1f;

    [Range(20f, 300f)]
    [SerializeField]
    private float rotationSpeed = 100f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        if (horizontalAxis != 0)
        {
            float horizontalRotation = horizontalAxis * Time.deltaTime * rotationSpeed;
            //transform.Rotate(0f, 0f, -horizontalRotation);
            transform.eulerAngles -= new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, horizontalRotation);
        }

        float verticalAxis = Input.GetAxis("Vertical");

        if (verticalAxis > 0)
        {
            rigidBody.AddForce(transform.right * forwardSpeed, ForceMode.Force);
        }
        else if (verticalAxis < 0)
        {
            rigidBody.AddForce(-transform.right * forwardSpeed, ForceMode.Force);
        }
    }
}
