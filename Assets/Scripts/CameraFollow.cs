using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public bool follow = true;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 5, -5);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(player);
        } else if (follow)
        {
            transform.position = player.transform.position + offset;
        }
    }

    public void Reset()
    {
        transform.position = offset;
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
