using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinGeneration : MonoBehaviour
{
    public GameObject pin;
    public float pinXSeparation = 0.75f;
    public float pinZSeparation = 0.75f;
    public int rowNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        GeneratePins(rowNumber, new Vector3(0, 1.15f, 13.75f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public function to be called on at level generation
    //PARAMETERS: rows is number of rows of pins (increase with difficulty),
    //            pinPosition is position of first pin at top of triangle (based on lane length)
    public void GeneratePins(int rows, Vector3 pinPosition)
    {
        for (int rowSize = 1; rowSize < rows+1; rowSize++)
        {
            makeRow(rowSize, pinPosition);
            pinPosition.z += pinZSeparation;
            pinPosition.x += pinXSeparation/2f;
        }
    }

    //Helper function for GeneratePins
    void makeRow(int pinsPerRow, Vector3 pinPosition)
    {
        for (int pins = 0; pins < pinsPerRow; pins++)
        {
            SelectPin(pinPosition);
            pinPosition.x -= pinXSeparation;
        }
    }

    //Function to determine type of pin placed, allows for later changes
    void SelectPin(Vector3 position)
    {
        Instantiate(pin, position, Quaternion.identity);
    }
}
