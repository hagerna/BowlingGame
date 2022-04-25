using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Public Variables to be accessed by other scripts:
    public Dictionary<string, int> gameData = new Dictionary<string, int>();
    public Dictionary<string, int> baseData = new Dictionary<string, int>();
    public Material[] laneMaterials;
    public float pinsCollected, pinSeparation, laneLength;

    private static GameManager _instance;
    public static GameManager Instance
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
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(_instance);
        InitializeData();
        LevelReset();
    }

    private void InitializeData()
    {
        //"ballsLeft" --> in tracking the lives that the player has, used in BallsReset function (GameManager)
        //"ballsPerLevel" --> self explanatory, used to reset "ballsLeft" between levels
        //"level" --> tracks the player's current level, to be used for increasing difficulty
        //"strikes" --> total strikes the player has gotten so far this run, used for bonus at end (?)
        //"strikeStreak" --> current consecutive strikes, used to determine what to display (ex. Turkey!)
        //"bumperLives" --> how many hits the bumpers can take before disappearing (doesn't reset level to level)
        //"bumperLivesTotal" --> to reset bumperLives between runs
        //"
        baseData["ballsLeft"] = 2;
        baseData["ballsPerLevel"] = 2;
        baseData["level"] = 1;
        baseData["strikes"] = 0;
        baseData["strikeStreak"] = 0;
        baseData["bumperLives"] = 0;
        baseData["bumperLivesTotal"] = 0;
        baseData["pinRows"] = 4;
        baseData["laneLength"] = 50;
        pinsCollected = 0;
        pinSeparation = 1f;
        laneLength = 50f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // When the player falls of the sides, hits an obstacle, or reaches the end of the lane
    public void BallReset()
    {
        gameData["ballsLeft"]--;
        if (gameData["ballsLeft"] == 0)
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
        }
        else
        {
            if (gameData["ballsLeft"] == gameData["ballsPerLevel"] - 1)
            {
                //strike
            }
            else if (gameData["ballsLeft"] == gameData["ballsPerLevel"] - 2)
            {
                //spare
            }
            Debug.Log("Next Level");
            NextLevel();
            gameData["ballsLeft"] = gameData["ballsPerLevel"];
        }
    }

    void NextLevel()
    {
        gameData["level"]++;
        Debug.Log(gameData["level"]);
        if (gameData["level"] % 5 == 0)
        {
            gameData["pinRows"]++;
            gameData["laneLength"]+=5;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LevelGenerator.instance.GenerateLevel(gameData["pinRows"], gameData["laneLength"], pinSeparation); //, laneMaterials[gameData["level"] / 10]);
    }

    void LevelReset()
    {
        gameData = baseData;
        pinsCollected = 0;
        pinSeparation = 1f;
        laneLength = 50f;
        LevelGenerator.instance.GenerateLevel(4, 50, 1f);
    }
}
