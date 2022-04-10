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

    // Start is called before the first frame update
    void Start()
    {
        if (generateOnStart)
        {
            GenerateLevel(4, 50, 0);
        }
    }

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
        Instantiate(ball, Vector3.zero, Quaternion.identity);
        //GenerateObstacles() --> potential function?

    }

    //public function to be called on at level generation
    //PARAMETERS: rows is number of rows of pins (increase with difficulty),
    //            pinPosition is position of first pin at top of triangle (based on lane length)
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
}
