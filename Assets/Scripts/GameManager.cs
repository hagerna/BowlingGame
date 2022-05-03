using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Public Variables to be accessed by other scripts:
    public Dictionary<string, int> gameData = new Dictionary<string, int>();
    public Dictionary<string, int> baseData = new Dictionary<string, int>();
    public float pinsCollected, pinSeparation, laneLength, totalScore;
    public string currentBall; //types: basic, fire, gold, ghost, vortex
    public GameObject scoreScreenUI;

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


    // When the player falls of the sides, hits an obstacle, or reaches the end of the lane
    public void BallReset()
    {
        gameData["ballsLeft"]--;
        if (GameObject.FindGameObjectWithTag("Pin") != null && gameData["ballsLeft"] > 0)
        {
            LevelGenerator.instance.NewBall();
        }

    }

    IEnumerator CheckPins()
    {
        bool pinsStanding = true;
        while (pinsStanding)
        {
            if (GameObject.FindGameObjectWithTag("Pin") == null)
            {
                pinsStanding = false;
            } else
            {
                yield return new WaitForSeconds(0.1f);
                if (gameData["ballsLeft"] == 0)
                {
                    yield return new WaitForSeconds(2f);
                    if (GameObject.FindGameObjectWithTag("Pin") == null)
                    {
                        pinsStanding = false;
                    }
                    else
                    {
                        Instantiate(scoreScreenUI, transform);
                        yield break;
                    }
                }
            }
        }
        //FindObjectOfType<PlayerControls>().movementLocked = true;
        StartCoroutine(NextLevel());
    }

    void StrikeCheck()
    {
        if (gameData["ballsPerLevel"] - 1 == gameData["ballsLeft"] || gameData["ballsPerLevel"] == gameData["ballsLeft"])
        {
            //strike
            gameData["strikes"]++;
            gameData["strikeStreak"]++;
        }
        else
        {
            gameData["strikeStreak"] = 0;
        }
    }

    IEnumerator NextLevel()
    {
        StrikeCheck();
        yield return new WaitForSeconds(2f); // wait for celebration graphic
        gameData["level"]++;
        gameData["ballsLeft"] = gameData["ballsPerLevel"];
        if (gameData["level"] % 5 == 0)
        {
            gameData["pinRows"]++;
            gameData["laneLength"]+=5;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        LevelGenerator.instance.GenerateLevel(gameData["pinRows"], gameData["laneLength"], pinSeparation); //, laneMaterials[gameData["level"] / 10]);
        StartCoroutine(CheckPins());
    }

    public void LevelReset()
    {
        gameData = baseData;
        Debug.Log(gameData["level"]);
        pinsCollected = 0;
        pinSeparation = 1f;
        laneLength = 50f;
        LevelGenerator.instance.GenerateLevel(4, 50, 1f);
        StartCoroutine(CheckPins());
    }

    public float TotalScore()
    {
        totalScore = pinsCollected + (gameData["strikes"] * 10);
        return totalScore;
    }
}
