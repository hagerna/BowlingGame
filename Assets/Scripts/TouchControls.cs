using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    public float forwardSpeed, horizontalControl;
    public bool movementLocked = true;
    public bool initialLaunch = true;
    bool resetTriggered = false;
    RectTransform powerBar;

    public bool explosion, gold, blackhole, ghost;
    public ParticleSystem impactEffect, deathEffect;
    public Material ghostRegular, ghostFade;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        powerBar = GameObject.Find("PowerBar").GetComponent<RectTransform>();
        // make the camera follow the game object this script is attached to
        Camera.main.GetComponent<CameraFollow>().player = gameObject;
        Camera.main.GetComponent<CameraFollow>().follow = true;
        forwardSpeed = GameManager.Instance.gameData["speed"];
        horizontalControl = GameManager.Instance.gameData["control"];
        StartCoroutine(InitialLaunch());
    }

    void Update()
    {
        if (movementLocked) return;
        if (Input.touchCount != 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 horizontal = Vector3.zero;
            if (touch.position.x > 562.5f)
            {
                horizontal = Vector3.right * Time.deltaTime * horizontalControl;
                rb.velocity += horizontal;
            } else if (touch.position.x < 562.5f)
            {
                horizontal = -Vector3.right * Time.deltaTime * horizontalControl;
                rb.velocity += horizontal;
            }
            if (Input.touchCount == 2 && ghost)
            {
                StartCoroutine(GhostMode());
            }
        }
        if (rb.velocity.z < 0.25f)
        {
            HandleObstacle();
        }
    }

    IEnumerator InitialLaunch()
    {
        while (Input.touchCount == 0)
        {
            yield return new WaitForEndOfFrame();
        }
        Touch touch = Input.GetTouch(0);
        float launchSpeed = 0;
        while (touch.deltaPosition.y <= 0)
        {
            //Provide UI indicating predicted speed
            launchSpeed -= touch.deltaPosition.y;
            if (launchSpeed > 1900)
            {
                launchSpeed = 1900;
            }
            powerBar.sizeDelta = new Vector2(50, launchSpeed);
            if (Input.touchCount != 0)
            {
                touch = Input.GetTouch(0);
            }
            else
            {
                powerBar.sizeDelta = new Vector2(50, 0);
                StartCoroutine(InitialLaunch());
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        while (touch.deltaPosition.y < 1)
        {
            yield return new WaitForEndOfFrame();
            if (Input.touchCount == 0)
            {
                powerBar.sizeDelta = new Vector2(50, 0);
                StartCoroutine(InitialLaunch());
                yield break;
            }
        }
        launchSpeed = launchSpeed / 1900f * forwardSpeed;
        if (launchSpeed < 0.25f)
        {
            launchSpeed = 0.5f;
        }
        rb.velocity = new Vector3(0,0,launchSpeed);
        movementLocked = false;
        powerBar.sizeDelta = new Vector2(50, 0);
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
        if (collision.collider.CompareTag("Pin"))
        {
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
        Collider[] pins = Physics.OverlapSphere(transform.position, 5f);
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        for (int pin = 0; pin < pins.Length; pin++)
        {
            if (pins[pin].CompareTag("Pin"))
            {
                if (explosion)
                {
                    pins[pin].GetComponent<PinScript>().Explode(transform.position);
                }
                else if (blackhole)
                {
                    pins[pin].GetComponent<PinScript>().Blackhole(transform.position);
                }
            }
        }
        explosion = false;
        blackhole = false;
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
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
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