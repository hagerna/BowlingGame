using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject pin, boostGate, ramp;
    public GameObject[] lanes;
    public GameObject[] obstacles;
    public GameObject[] ballPrefabs;
    int currentWorld;
    bool[] worldsCompleted;

    GameObject ball;

    private static LevelGenerator _instance;
    public static LevelGenerator instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelGenerator>();
                if (_instance == null)
                {
                    throw new UnityException("Instance of LevelGenerator not found in scene");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(_instance);
        currentWorld = 0;
    }

    //Generate Level by instantiating lane, pins, and bowling ball (obstacles not implemented yet)
    //PARAMETERS: pinRows --> passed to Generate pins (see 'rows' parameter)
                //laneLength --> how long the lane is
                //pinSeparation --> distance between pins
                //laneColor --> what color the lane is
    public void GenerateLevel(int pinRows, float laneLength, float pinSeparation, float Offset = 0f)
    {
        CheckWorld();
        Vector3 lanePosition = new Vector3(0, 0, (laneLength / 2f) - 5f);
        GameObject newLane = Instantiate(lanes[currentWorld], lanePosition, Quaternion.identity);
        if (currentWorld != 1)
        {
            newLane.transform.localScale = new Vector3(pinRows + Offset, 1, laneLength);
        }
        Vector3 firstPin = new Vector3(Offset, 1, laneLength - (pinRows * pinSeparation + 5.5f));
        GeneratePins(pinRows, firstPin, pinSeparation);
        ChooseBall();
        GenerateObstacles(pinRows + Offset, firstPin.z);
    }

    //Every twenty levels pick a new world, one that the player has not played already, resetting if they have played them all
    void CheckWorld()
    {
        if (GameManager.Instance.gameData["level"] % 20 != 0)
        {
            return;
        }
        if (worldsCompleted.Length == 4) // doesn't work right
        {
            worldsCompleted = null;
        }
        currentWorld = Random.Range(1, 5);
        while (worldsCompleted[currentWorld])
        {
            currentWorld = Random.Range(1, 5);
        }
        worldsCompleted[currentWorld] = true;
    }

    //public function to be called on at level generation
    //PARAMETERS: rows --> number of rows of pins (increase with difficulty),
    //pinPosition --> position of first pin at top of triangle (based on lane length)
    void GeneratePins(int rows, Vector3 pinPosition, float pinSeparation)
    {
        for (int rowSize = 1; rowSize < rows + 1; rowSize++)
        {
            makeRow(rowSize, pinPosition, pinSeparation);
            pinPosition.z += pinSeparation;
            pinPosition.x += pinSeparation / 2f;
        }
    }

    //Helper function for GeneratePins
    void makeRow(int pinsPerRow, Vector3 pinPosition, float pinSeparation)
    {
        for (int pins = 0; pins < pinsPerRow; pins++)
        {
            SelectPin(pinPosition);
            pinPosition.x -= pinSeparation;
        }
    }

    //Function to determine type of pin placed, allows for later changes
    void SelectPin(Vector3 position)
    {
        Instantiate(pin, position, Quaternion.identity);
    }

    //Determine which ball prefab to Instantiate
    void ChooseBall()
    {
        switch (GameManager.Instance.currentBall)
        {
            case "basic":
                ball = ballPrefabs[0];
                break;
            case "fire":
                ball = ballPrefabs[1];
                break;
            case "gold":
                ball = ballPrefabs[2];
                break;
            case "ghost":
                ball = ballPrefabs[3];
                break;
            case "vortex":
                ball = ballPrefabs[4];
                break;
            default:
                ball = null;
                Debug.Log("error in ChooseBall(), ball type not found");
                break;
        }
        Instantiate(ball, Vector3.up, Quaternion.LookRotation(Vector3.right));
    }

    void GenerateObstacles(float laneWidth, float laneLength)
    {
        int level = GameManager.Instance.gameData["level"];
        if (level == 1)
        {
            return;
        }
        switch (currentWorld)
        {
            case 0:
                GenerateWorld0Level(level, laneWidth, laneLength);
                break;
            case 1:
                GenerateWorld1Level(level, laneWidth, laneLength);
                break;
            case 2:
                //GenerateWorld2Level(level, laneWidth, laneLength);
                break;
            case 3:
                //GenerateWorld3Level(level, laneWidth, laneLength);
                break;
            case 4:
                //GenerateWorld4Level(level, laneWidth, laneLength);
                break;
            default:
                Debug.Log("Error on GenerateObstacles() --> switch failed");
                break;

        }

    }

    // World 0 is the basic world, player will always go through variation of same 20 levels to start
    void GenerateWorld0Level(int level, float laneWidth, float laneLength)
    {
        level = level % 20;
        Vector3 spawn = Vector3.up;
        if (level < 5)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) - 10, (laneLength / 2) + 10);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;
        }
        if (level < 10)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) - 10, laneLength / 2) - 1;
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) + 1, laneLength / 2) + 10;
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;

        }
        if (level == 10)
        {
            return;
        }
        if (level < 15)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 2, (laneWidth / 2) - 2);
            spawn.z = Random.Range((laneLength / 2) - 10, laneLength / 2) - 1;
            Instantiate(boostGate, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) + 1, laneLength / 2) + 10;
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;
        }
        if (level < 20)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 2, (laneWidth / 2) - 2);
            spawn.z = Random.Range(10, laneLength / 3);
            Instantiate(boostGate, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(laneLength / 3, 2 * (laneLength / 3));
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(2 * (laneLength / 3), laneLength - 5);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;
        }
    }

    void GenerateWorld1Level(int level, float laneWidth, float laneLength)
    {

    }

    // Create a new ball object for the player to roll again
    public void NewBall()
    {
        Instantiate(ball, Vector3.up, Quaternion.LookRotation(Vector3.right));
    }
}
