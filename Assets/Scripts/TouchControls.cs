using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    public float forwardSpeed = 10;
    public float horizontalControl = 10;
    public bool movementLocked = true;
    public bool initialLaunch = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(InitialLaunch());
    }

    void Update()
    {
        if (movementLocked) return;
        if (Input.touchCount != 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 horizontal = Vector3.zero;
            Debug.Log(touch.position);
            if (touch.position.x > 562.5f)
            {
                horizontal = Vector3.right * (touch.position.x / 1125f) * Time.deltaTime * horizontalControl;
                rb.velocity += horizontal;
            } else if (touch.position.x < 562.5f)
            {
                horizontal = -Vector3.right * (touch.position.x / 1125f) * Time.deltaTime * horizontalControl;
                rb.velocity += horizontal;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pin"))
        {
            movementLocked = true;
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
            touch = Input.GetTouch(0);
            yield return new WaitForEndOfFrame();
        }
        while (touch.deltaPosition.y < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        launchSpeed = launchSpeed / 2000f * forwardSpeed;
        rb.velocity = new Vector3(0,0,launchSpeed);
        movementLocked = false;
    }
}
