using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITracking : MonoBehaviour
{
    public bool displayLevel, displayLives, displayPins, displayStrikes;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (displayLevel)
        {
            text.text = "LEVEL: " + GameManager.Instance.gameData["level"];
        } else if (displayLives)
        {
            text.text = "LIVES: " + GameManager.Instance.gameData["ballsLeft"] + "/" + GameManager.Instance.gameData["ballsPerLevel"];
        } else if (displayPins)
        {
            text.text = "PINS: " + GameManager.Instance.pinsCollected;
        } else if (displayStrikes)
        {
            text.text = "STRIKES: " + GameManager.Instance.gameData["strikes"];
        }
    }
}
