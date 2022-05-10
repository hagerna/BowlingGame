using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinScript : MonoBehaviour
{
    public string pinType;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckStanding());
    }

    IEnumerator CheckStanding()
    {
        bool standing = true;
        while (standing)
        {
            if (transform.up == Vector3.up) //if the pin is still standing
            {
                yield return new WaitForSeconds(0.05f);
            }
            else //pin is no longer standing
            {
                standing = false;
                AudioSingleton.Play("One Pin");
                PinFallEffect();
            }
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void PinFallEffect()
    {
        
        //Update pin counter
        GameManager.Instance.pinsCollected++;
        if (GameManager.Instance.currentBall == "gold")
        {
            GameManager.Instance.pinsCollected++;
        }
    }

    public void Explode(Vector3 origin)
    {
        GetComponent<Rigidbody>().AddExplosionForce(100, origin, 3);
    }

    public void Blackhole(Vector3 origin)
    {
        Vector3 direction = origin - transform.position;
        StartCoroutine(PushPull(direction));
    }

    IEnumerator PushPull(Vector3 direction)
    {
        float distance = direction.magnitude;
        GetComponent<Rigidbody>().AddForce(-direction * (20 / distance * distance), ForceMode.Acceleration);
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().AddForce(direction * (120 / distance * distance), ForceMode.Acceleration);
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
    }
}
