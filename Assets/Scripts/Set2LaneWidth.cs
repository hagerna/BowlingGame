using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set2LaneWidth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //sets a game objects width to be equal to the width of the lane
        transform.localScale = new Vector3(GameManager.Instance.gameData["pinRows"] + GameManager.Instance.pinSeparation, 1, transform.localScale.z);
    }
}
