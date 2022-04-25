using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject lane, pin, ball, obstacle, boostGate, ramp;

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
    }

    //Generate Level by instantiating lane, pins, and bowling ball (obstacles not implemented yet)
    //PARAMETERS: pinRows --> passed to Generate pins (see 'rows' parameter)
                //laneLength --> how long the lane is
                //pinSeparation --> distance between pins
                //laneColor --> what color the lane is
    public void GenerateLevel(int pinRows, float laneLength, float pinSeparation, float Offset = 0f, Material laneColor = null)
    {
        Vector3 lanePosition = new Vector3(0, 0, (laneLength / 2f) - 5f);
        GameObject newLane = Instantiate(lane, lanePosition, Quaternion.identity);
        newLane.transform.localScale = new Vector3(pinRows + Offset, 1, laneLength);
        if (laneColor)
        {
            lane.GetComponent<Renderer>().material = laneColor;
        }
        Vector3 firstPin = new Vector3(Offset, 1, laneLength - (pinRows * pinSeparation + 5.5f));
        GeneratePins(pinRows, firstPin, pinSeparation);
        Instantiate(ball, Vector3.up, Quaternion.LookRotation(Vector3.right));
        GenerateObstacles(pinRows + Offset, firstPin.z);
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

    void GenerateObstacles(float laneWidth, float laneLength)
    {
        int level = GameManager.Instance.gameData["level"];
        Vector3 spawn = Vector3.up;
        if (level == 1)
        {
            return;
        }
        if (level < 5)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) - 10, (laneLength / 2) + 10);
            Instantiate(obstacle, spawn, Quaternion.identity);
            return;
        }
        if (level < 10)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) - 10, laneLength / 2)-1;
            Instantiate(obstacle, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2)+1, laneLength / 2)+10;
            Instantiate(obstacle, spawn, Quaternion.identity);
            return;

        }
        if (level == 10)
        {
            return;
        }
        if (level < 15)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2) - 10, laneLength / 2)-1;
            Instantiate(boostGate, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range((laneLength / 2)+1, laneLength / 2) + 10;
            Instantiate(obstacle, spawn, Quaternion.identity);
            return;
        }
        if (level < 20)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(10, laneLength / 3);
            Instantiate(boostGate, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(laneLength / 3 , 2*(laneLength / 3));
            Instantiate(obstacle, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(2*(laneLength / 3), laneLength);
            Instantiate(obstacle, spawn, Quaternion.identity);
            return;
        }
        if (level < 20)
        {
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(10, laneLength / 3);
            Instantiate(obstacle, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(laneLength / 3, 2 * (laneLength / 3));
            Instantiate(obstacle, spawn, Quaternion.identity);
            spawn.x = Random.Range(-(laneWidth / 2) + 1, (laneWidth / 2) - 1);
            spawn.z = Random.Range(2 * (laneLength / 3), laneLength);
            Instantiate(obstacle, spawn, Quaternion.identity);
            return;
        }

    }

    // Create a new ball object for the player to roll again
    public void NewBall()
    {
        Instantiate(ball, Vector3.up, Quaternion.LookRotation(Vector3.right));
    }
}
