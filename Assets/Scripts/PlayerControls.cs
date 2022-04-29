using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float forwardSpeed = 20;
    public float horizontalControl = 4;
    public bool movementLocked = false;
    bool resetTriggered = false;

    public bool explosion, gold, blackhole, ghost;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // make the camera follow the game object this script is attached to
        Camera.main.GetComponent<CameraFollow>().player = gameObject;
        Camera.main.GetComponent<CameraFollow>().follow = true;
    }

    void Update()
    {
        if (movementLocked) return;
        float moveLeftRight = Input.GetAxis("Horizontal");
        Vector3 xAcceleration = Vector3.right * moveLeftRight * Time.deltaTime * horizontalControl;
        rb.velocity += xAcceleration;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Boost();
        }
    }

    void Boost()
    {
        rb.velocity += Vector3.forward * forwardSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pin")) {
            movementLocked = true;
            if (explosion || blackhole)
            {
                HandleImpactEffect();
            }
        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            HandleObstacle();
        }
    }

    void HandleImpactEffect()
    {
        Collider[] pins = Physics.OverlapSphere(transform.position, 2f);
        for (int pin = 0; pin < pins.Length; pin++)
        {
            if (pins[pin].CompareTag("Pin"))
            {
                if (explosion) { pins[pin].GetComponent<PinScript>().Explode(transform.position); }
                if (explosion) { pins[pin].GetComponent<PinScript>().Blackhole(transform.position); }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boost"))
        {
            Boost();
        }
        else if (other.CompareTag("CameraStop"))
        {
            Camera.main.GetComponent<CameraFollow>().follow = false;
        }
        else if (other.CompareTag("Obstacle"))
        {
            HandleObstacle();
        }
    }

    void HandleObstacle()
    {
        movementLocked = true;
        Camera.main.GetComponent<CameraFollow>().follow = false;
        StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath()
    {
        rb.isKinematic = true;
        GetComponent<MeshRenderer>().enabled = false;
        //trigger particle effect
        yield return new WaitForSeconds(1f);
        if (!resetTriggered)
        {
            GameManager.Instance.BallReset();
            resetTriggered = true;
        }
        Destroy(gameObject);
    }
}
