using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    public float forwardSpeed = 10;
    public float horizontalControl = 10;
    public bool movementLocked = true;
    public bool initialLaunch = true;
    bool resetTriggered = false;
    RectTransform powerBar;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        powerBar = GameObject.Find("PowerBar").GetComponent<RectTransform>();
        // make the camera follow the game object this script is attached to
        Camera.main.GetComponent<CameraFollow>().player = gameObject;
        Camera.main.GetComponent<CameraFollow>().follow = true;
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
        rb.velocity = new Vector3(0,0,launchSpeed);
        movementLocked = false;
        powerBar.sizeDelta = new Vector2(50, 0);
    }

    void Boost()
    {
        rb.velocity += Vector3.forward * forwardSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pin"))
        {
            movementLocked = true;
            // Trigger explosion if explosive ball
        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            HandleObstacle();
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

        GameManager.Instance.baseData["ballsPerLevel"]++;
    }
}
