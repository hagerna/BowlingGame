using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float forwardSpeed = 20;
    public float horizontalControl = 4;
    public bool movementLocked = false;
    bool resetTriggered = false;
    bool initialBoost = true;

    public bool explosion, gold, ghost, blackhole;
    public ParticleSystem impactEffect, deathEffect;
    public Material ghostRegular, ghostFade;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // make the camera follow the game object this script is attached to
        Camera.main.GetComponent<CameraFollow>().player = gameObject;
        Camera.main.GetComponent<CameraFollow>().follow = true;
        forwardSpeed = GameManager.Instance.gameData["speed"];
        horizontalControl = GameManager.Instance.gameData["control"];
    }

    void Update()
    {
        if (movementLocked) return;
        float moveLeftRight = Input.GetAxis("Horizontal");
        Vector3 xAcceleration = Vector3.right * moveLeftRight * Time.deltaTime * horizontalControl;
        rb.velocity += xAcceleration;
        if (Input.GetKeyDown(KeyCode.Space) && initialBoost)
        {
            Boost();
            initialBoost = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && ghost)
        {
            StartCoroutine(GhostMode());
        }
        if (rb.velocity.z < 0.25f && !initialBoost)
        {
            HandleObstacle();
        }
    }

    void Boost(Vector3 direction = default(Vector3))
    {
        if (direction == Vector3.zero)
        {
            direction = Vector3.forward;
        }
        rb.velocity += direction * forwardSpeed;
    }

    IEnumerator GhostMode()
    {
        ghost = false;
        rb.useGravity = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().material = ghostFade;
        //change material
        yield return new WaitForSeconds(1.5f);
        rb.useGravity = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().material = ghostRegular;
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
        if (collision.collider.CompareTag("Bouncy"))
        {
            Boost(Vector3.up + 0.25f * Vector3.forward);
        }
    }

    void HandleImpactEffect()
    {
        Collider[] pins = Physics.OverlapSphere(transform.position, 5f);
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        for (int pin = 0; pin < pins.Length; pin++)
        {
            if (pins[pin].CompareTag("Pin"))
            {
                if (explosion) {
                    pins[pin].GetComponent<PinScript>().Explode(transform.position);
                }
                else if (blackhole) {
                    pins[pin].GetComponent<PinScript>().Blackhole(transform.position);
                }
            }
        }
        explosion = false;
        blackhole = false;
        //HandleObstacle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boost"))
        {
            Boost(other.gameObject.transform.forward);
        }
        else if (other.CompareTag("Lock"))
            {
                movementLocked = true;
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
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.35f);
        if (!resetTriggered)
        {
            GameManager.Instance.BallReset();
            resetTriggered = true;
        }
        Destroy(gameObject);
    }
}
