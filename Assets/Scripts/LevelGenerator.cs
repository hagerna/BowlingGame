using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject pin, ball, boostGate, ramp;
    public GameObject[] lanes;
    public GameObject[] obstacles;
    public GameObject[] ballPrefabs;
    int currentWorld;
    public bool[] worldsCompleted;

    private static LevelGenerator _instance;
    public static LevelGenerator Instance
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
        CheckWorld(); // Determine current world
        Vector3 lanePosition = new Vector3(0, 0, (laneLength / 2f) - 5f);
        if (currentWorld == 1)
        {
            lanePosition = new Vector3(0, 0, 0);
            Instantiate(lanes[currentWorld], lanePosition, Quaternion.identity);
            laneLength = 160;
        }
        else if (currentWorld == 4)
        {
            lanePosition = new Vector3(0, 0, -5);
            Instantiate(lanes[currentWorld], lanePosition, Quaternion.identity);
            laneLength = 192;
        }
        else
        {
            GameObject newLane = Instantiate(lanes[currentWorld], lanePosition, Quaternion.identity);
            newLane.transform.localScale = new Vector3(pinRows + Offset, 1, laneLength);
            if (currentWorld == 2)
            {
                laneLength = laneLength * 2.5f + 3;
            }
            if (currentWorld == 3)
            {
                laneLength = laneLength * 4 + 2;
            }
        }
        Vector3 firstPin = new Vector3(Offset, 1, laneLength - (pinRows * pinSeparation + 5.5f));
        if (currentWorld == 4)
        {
            firstPin.y -= 24.5f;
        }
        SelectBall();
        GeneratePins(pinRows, firstPin, pinSeparation);
        GenerateObstacles(pinRows + Offset, firstPin.z);
    }

    //Every twenty levels pick a new world, one that the player has not played already, resetting if they have played them all
    void CheckWorld()
    {
        bool allWorldsCompleted = true;
        if (GameManager.Instance.gameData["level"] % 20 != 0)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            if (!worldsCompleted[i])
            {
                allWorldsCompleted = false;
                break;
            }
        }
        if (allWorldsCompleted)
        {
            for (int i = 0; i < 4; i++)
            {
                worldsCompleted[i] = false;
            }
        }
        currentWorld = Random.Range(0, 5);
        while (worldsCompleted[currentWorld])
        {
            currentWorld = Random.Range(0, 5);
        }
        worldsCompleted[currentWorld] = true;
    }

    // Based on the current ball tracked in Game Manager, select ball prefab to spawn
    void SelectBall()
    {
        switch (GameManager.Instance.currentBall)
        {
            case "basic":
                ball = ballPrefabs[0];
                break;
            case "fire":
                ball = ballPrefabs[1];
                break;
            case "ghost":
                ball = ballPrefabs[2];
                break;
            case "gold":
                ball = ballPrefabs[3];
                break;
            case "vortex":
                ball = ballPrefabs[4];
                break;
            default:
                ball = null;
                Debug.Log("Error with SelectBall() in LevelGenerator");
                break;
        }
        NewBall();
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
            Instantiate(pin, pinPosition, Quaternion.identity);
            pinPosition.x -= pinSeparation;
        }
    }

    //Script to randomly generate obstacles based on currentWorld --> calls helper script
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
                GenerateWorld2Level(level, laneWidth, laneLength);
                break;
            case 3:
                GenerateWorld3Level(level, laneWidth, laneLength);
                break;
            case 4:
                GenerateWorld4Level(level, laneWidth, laneLength);
                break;
            default:
                Debug.Log("Error on GenerateObstacles() --> switch failed");
                break;

        }

    }

    // Create a new ball object for the player to roll again
    public void NewBall()
    {
        Instantiate(ball, Vector3.up, Quaternion.LookRotation(Vector3.right));
    }



    // WORLD OBSTACLE POSITIONING FUNCTIONS BELOW

    // World 0 is the basic world, player will always go through variation of same 20 levels to start
    void GenerateWorld0Level(int level, float laneWidth, float laneLength)
    {
        level %= 20;
        Vector3 spawn = Vector3.up;
        if (level < 5)
        {
            int select = Random.Range(0, 2);
            if (select == 0)
            {
                spawn.x = 1f;
            }
            else
            {
                spawn.x = -1f;
            }
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

    // Helix World
    void GenerateWorld1Level(int level, float laneWidth, float laneLength)
    {
        level %= 20;
        Vector3 spawn = Vector3.up;
        if (level < 5)
        {
            spawn.x = Random.Range(-1.5f, 1.5f);
            spawn.z = Random.Range(5, 25);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;
        }
        if (level < 10)
        {
            spawn.x = Random.Range(-1.5f, 1.5f);
            spawn.z = Random.Range(5, 25);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.x = Random.Range(-1.5f, 1.5f);
            spawn.z = Random.Range(125, 140);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;

        }
        if (level < 15)
        {
            spawn.x = Random.Range(-1.5f, 1.5f);
            spawn.z = Random.Range(5, 25);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.x = Random.Range(-1.5f, 1.5f);
            spawn.z = Random.Range(125, 140);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            int i = Random.Range(0, 1);
            if (i == 0)
            {
                spawn.x = -8.07f;
                spawn.z = 56.3f;
                Instantiate(obstacles[0], spawn, Quaternion.identity);
            }
            else
            {
                spawn.x = 8.07f;
                spawn.z = 56.3f;
                Instantiate(obstacles[0], spawn, Quaternion.identity);
            }
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
            int i = Random.Range(0, 1);
            if (i == 0)
            {
                spawn.x = -8.07f;
                spawn.z = 56.3f;
                Instantiate(obstacles[0], spawn, Quaternion.identity);
            }
            else
            {
                spawn.x = 8.07f;
                spawn.z = 56.3f;
                Instantiate(obstacles[0], spawn, Quaternion.identity);
            }
            i = Random.Range(0, 1);
            if (i == 0)
            {
                spawn.x = 6.66f;
                spawn.z = 97.32f;
                Instantiate(obstacles[0], spawn, Quaternion.identity);
            }
            else
            {
                spawn.x = -6.66f;
                spawn.z = 97.32f;
                Instantiate(obstacles[0], spawn, Quaternion.identity);
            }
            return;
        }
    }

    // Ramps World
    void GenerateWorld2Level(int level, float laneWidth, float laneLength)
    {
        level %= 20;
        Vector3 spawn = Vector3.up;
        if (level < 5)
        {
            spawn.y = 0.5f;
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength/3 + 4;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 * 2 + 4;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.y = 1f;
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(laneLength/2, laneLength/2 + 15);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;
        }
        if (level < 10)
        {
            spawn.y = 0.5f;
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 - Random.Range(5,15);
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 + 6;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 * 2 - Random.Range(5, 15);
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 * 2 + 4;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.y = 1f;
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(laneLength*3 / 4 +10, laneLength*3 / 4 + 20);
            Instantiate(obstacles[0], spawn, Quaternion.identity);

            return;

        }
        if (level < 15)
        { 
            spawn.y = 0.5f;
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 + 6;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.y = 2.5f;
            spawn.x = Random.Range(-(laneWidth / 2), (laneWidth / 2));
            spawn.z += 10;
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.y = 0.5f;
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 * 2 + 6;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.y = 2.5f;
            spawn.x = Random.Range(-(laneWidth / 2), (laneWidth / 2));
            spawn.z += 10;
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.y = 1f;
            spawn.x = Random.Range(-(laneWidth / 2) + 2, (laneWidth / 2) - 2);
            spawn.z += Random.Range(5, 15);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            return;
        }
        if (level < 20)
        {
            spawn.y = 0.5f;
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 3 - 10;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength*2 / 3 - 8;
            Instantiate(ramp, spawn, Quaternion.Euler(66, 0, 0));
            spawn.y = 1f;
            spawn.x = Random.Range(-(laneWidth / 2) + 2, (laneWidth / 2) - 2);
            spawn.z = (laneLength / 3 - 10) - Random.Range(10, 15);
            Instantiate(boostGate, spawn, Quaternion.identity);
            spawn.y = 6f;
            spawn.x = Random.Range(-(laneWidth / 2), laneWidth / 2 - 3);
            spawn.z = Random.Range(laneLength / 3 + 6, laneLength / 3 + 14);
            Instantiate(obstacles[1], spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2), laneWidth / 2 - 3);
            spawn.z = Random.Range(2 * (laneLength / 3) + 6, 2 * (laneLength / 3) + 14);
            Instantiate(obstacles[1], spawn, Quaternion.identity);
            return;
        }
    }

    // Bouncy World
    void GenerateWorld3Level(int level, float laneWidth, float laneLength)
    {
        level %= 20;
        Vector3 spawn = Vector3.up;
        if (level < 5)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2), laneLength / 2 + 20);
            Instantiate(obstacles[2], spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) + 10, (laneLength / 2) + 40);
            Instantiate(obstacles[2], spawn, Quaternion.identity);
            return;
        }
        if (level < 10)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = laneLength / 5 + Random.Range(-15, 0);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = 4 * laneLength / 5 + Random.Range(30, 40);
            Instantiate(obstacles[0], spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2), laneWidth / 2);
            spawn.z = Random.Range((laneLength / 2) - 30, laneLength / 2) - 20;
            Instantiate(obstacles[2], spawn, Quaternion.identity);
            spawn.y = -13f;
            spawn.x = Random.Range(-(laneWidth / 2), laneWidth / 2);
            spawn.z += Random.Range(15, 30);
            Instantiate(obstacles[2], spawn, Quaternion.Euler(90, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2), laneWidth / 2);
            spawn.z += Random.Range(15, 30);
            Instantiate(obstacles[2], spawn, Quaternion.Euler(90, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2), laneWidth / 2);
            spawn.z += Random.Range(15, 30);
            Instantiate(obstacles[2], spawn, Quaternion.Euler(90, 0, 0));
            spawn.x = Random.Range(-(laneWidth / 2), laneWidth / 2);
            spawn.z += Random.Range(15, 30);
            Instantiate(obstacles[2], spawn, Quaternion.Euler(90, 0, 0));
            return;

        }
        if (level < 15)
        {
            spawn.z = Random.Range((laneLength / 2) - 40, laneLength / 2) - 30;
            for (int i = 0; i < 10; i++)
            {
                spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                spawn.y = Random.Range(-10, 2);
                Instantiate(obstacles[0], spawn, Quaternion.identity);
                spawn.z += 11;
            }
            return;
        }
        if (level < 20)
        {
            spawn.z = laneLength / 2 - 50;
            for (int i = 0; i < 6; i++)
            {
                spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                spawn.y = Random.Range(-10, 2);
                Instantiate(obstacles[1], spawn, Quaternion.identity);
                spawn.z += 10;
                spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                spawn.y = -13;
                Instantiate(obstacles[2], spawn, Quaternion.Euler(90, 0, 0));
                spawn.z += 10;
            }
            return;
        }
    }

    // Slope World

    float ySlope(float z)
    {
        return (-0.27f * z) + 11.9f;
    }

    void GenerateWorld4Level(int level, float laneWidth, float laneLength)
        {
            level %= 20;
            Vector3 spawn = Vector3.up;
            if (level < 5)
            {
                spawn.z = 50;
                for (int i = 0; i < 4; i++)
                {
                    spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                    spawn.y = ySlope(spawn.z);
                    Instantiate(obstacles[0], spawn, Quaternion.Euler(15, 0, 0));
                    spawn.z += 25;
                }
                return;
            }
            if (level < 10)
            {
                spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                spawn.z = 35;
                Instantiate(boostGate, spawn, Quaternion.identity);
                for (int i = 0; i < 5; i++)
                {
                    spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                    spawn.z += 20;  
                    spawn.y = ySlope(spawn.z);
                    Instantiate(obstacles[0], spawn, Quaternion.Euler(15, 0, 0));
                }
                return;
            }
            if (level < 15)
            {
                spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                spawn.z = 35;
                Instantiate(boostGate, spawn, Quaternion.identity);
                spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
                spawn.z += 15;
                spawn.y = ySlope(spawn.z);
                Instantiate(obstacles[2], spawn, Quaternion.Euler(15,0,0));
            for (int i = 0; i < 4; i++)
                {
                    spawn.x = Random.Range(-(laneWidth / 2) - 3, (laneWidth / 2) - 2);
                    spawn.z += 20;
                    spawn.y = ySlope(spawn.z);
                    Instantiate(obstacles[1], spawn, Quaternion.identity);
                }
                return;
            } 
            if (level < 20)
            {
                spawn.z = 35;
                for (int i = 0; i < 2; i++)
                {
                    spawn.x = 0;
                    spawn.z += 15;
                    spawn.y = ySlope(spawn.z);
                    Instantiate(obstacles[0], spawn, Quaternion.Euler(15, 0, 0));
                    spawn.x = Random.Range(-2.5f, 0);
                    spawn.z += 15;
                    spawn.y = ySlope(spawn.z);
                    Instantiate(obstacles[1], spawn, Quaternion.Euler(15, 0, 0));
            }
                spawn.x = Random.Range(-2.5f, 0);
                spawn.z += 20;
                spawn.y = ySlope(spawn.z);
                Instantiate(obstacles[1], spawn, Quaternion.Euler(15, 0, 0));
                spawn.x = Random.Range(-2.5f, 0);
                spawn.z += 35;
                spawn.y = -25;
                Instantiate(obstacles[1], spawn, Quaternion.identity);
                return;
            }
    }
}
