using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BumpersEnabled());
    }

    IEnumerator BumpersEnabled()
    {
        while (GameManager.instance.bumperLives > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.instance.bumperLives--;
        }
    }
}
