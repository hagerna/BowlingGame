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
                yield return new WaitForSeconds(0.1f);
            }
            else //pin is no longer standing
            {
                standing = false;
                PinFallEffect();
            }
        }
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void PinFallEffect()
    {
        //Update pin counter
        GameManager.instance.pinsCollected++;
    }
}
