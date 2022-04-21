using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Public Variables to be accessed by other scripts:
        //"ballsLeft" --> in tracking the lives that the player has, used in BallsReset function (GameManager)
        //"ballsPerLevel" --> self explanatory, used to reset "ballsLeft" between levels
        //"level" --> tracks the player's current level, to be used for increasing difficulty
        //"strikes" --> total strikes the player has gotten so far this run, used for bonus at end (?)
        //"strikeStreak" --> current consecutive strikes, used to determine what to display (ex. Turkey!)
        //"bumperLives" --> how many hits the bumpers can take before disappearing (doesn't reset level to level)
        //"bumperLivesTotal" --> to reset bumperLives between runs
        //"pinsCollected" --> pins knocked over, to be used as currency for upgrade menu
    public int ballsLeft, ballsPerLevel, level, strikes, strikeStreak, bumperLives, bumperLivesTotal;
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
        //Singleton Pattern
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

    // Check to see if there are any pins left, if not, go to next level
    void CheckPins()
    {
        if (GameObject.FindGameObjectWithTag("Pin") != null)
        {
            return;
        } else
        {
            //no pins left --> NEXT LEVEL
            Debug.Log("Next Level");
            ballsLeft = ballsPerLevel;
        }
    }


}
