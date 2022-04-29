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
    }

    public void Explode(Vector3 origin)
    {
        GetComponent<Rigidbody>().AddExplosionForce(10, origin, 2, 5);
    }

    public void Blackhole(Vector3 origin)
    {
        Vector3.RotateTowards(transform.rotation.eulerAngles, origin, 6.28f, 1f);
        GetComponent<Rigidbody>().AddForce(transform.forward * 5);
    }
}
