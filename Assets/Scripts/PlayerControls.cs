using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float forwardSpeed = 20;
    public float horizontalControl = 4;
    public bool movementLocked = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (movementLocked) return;
        float moveLeftRight = Input.GetAxis("Horizontal");
        Vector3 xAcceleration = Vector3.right * moveLeftRight * Time.deltaTime * horizontalControl;
        rb.velocity += xAcceleration;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += Vector3.forward * forwardSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pin")){
            movementLocked = true;
            Camera.main.GetComponent<CameraFollow>().follow = false;
            // TriggerReset
        }
    }
}
