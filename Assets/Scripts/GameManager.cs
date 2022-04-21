using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int ballsLeft, level, strikes, currentStrikeCount, bumperLives;
    public float pinsCollected;

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    throw new UnityException("Instance of GameManager not found in scene");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(_instance);
    }

        // Update is called once per frame
        void Update()
    {
        
    }

    // When the player falls of the sides, hits an obstacle, or reaches the end of the lane
    public void BallReset()
    {
        ballsLeft--;
        if (ballsLeft == 0)
        {
            // go to upgrade screen
        }
        else
        {
            // Reset the player back to beginning of the stage and check pins after 3 seconds
            LevelGenerator.instance.NewBall();
            Invoke(nameof(CheckPins), 3f);
        }
    }

    // Check to see if there are any pins left standing, if not, go to next level
    void CheckPins()
    {
        if (GameObject.FindGameObjectWithTag("Pin") != null)
        {
            return;
        } else
        {
            //no pins left, next level
            Debug.Log("Next Level");
        }
    }
}
