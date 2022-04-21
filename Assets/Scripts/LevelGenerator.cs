using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public bool generateOnStart;
    public GameObject lane;
    public GameObject pin;
    public GameObject ball;

    public float pinSeparation = 1f;
    public int playtestRows;

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
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(_instance);
        if (generateOnStart)
        {
            GenerateLevel(playtestRows, 50, 0);
        }
    }

    //Generate Level by instantiating lane, pins, and bowling ball (obstacles not implemented yet)
    //PARAMETERS: pinRows --> passed to Generate pins (see 'rows' parameter)
                //laneLength --> 
    public void GenerateLevel(int pinRows, float laneLength, float pinOffset, Material laneColor = null)
    {
        Vector3 lanePosition = new Vector3(0, 0, (laneLength / 2f) - 5f);
        GameObject newLane = Instantiate(lane, lanePosition, Quaternion.identity);
        newLane.transform.localScale = new Vector3(pinRows, 1, laneLength);
        if (laneColor)
        {
            lane.GetComponent<Renderer>().material = laneColor;
        }
        Vector3 firstPin = new Vector3(pinOffset, 1, laneLength - (pinRows * pinSeparation + 5.5f));
        GeneratePins(pinRows, firstPin);
        Instantiate(ball, Vector3.up, Quaternion.LookRotation(Vector3.right));
        //GenerateObstacles() --> potential function?

    }

    //public function to be called on at level generation
    //PARAMETERS: rows --> number of rows of pins (increase with difficulty),
                //pinPosition --> position of first pin at top of triangle (based on lane length)
    public void GeneratePins(int rows, Vector3 pinPosition)
    {
        for (int rowSize = 1; rowSize < rows + 1; rowSize++)
        {
            makeRow(rowSize, pinPosition);
            pinPosition.z += pinSeparation;
            pinPosition.x += pinSeparation / 2f;
        }
    }

    //Helper function for GeneratePins
    void makeRow(int pinsPerRow, Vector3 pinPosition)
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

    // Create a new ball object for the player to roll again
    public void NewBall()
    {
        Instantiate(ball, Vector3.up, Quaternion.LookRotation(Vector3.right));
    }
}
