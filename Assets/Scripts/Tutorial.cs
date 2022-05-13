using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    RawImage display;
    public Texture[] slides;
    int currentSlide;

    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<RawImage>();
        display.texture = slides[0];
        currentSlide = 0;
    }

    public void NextSlide()
    {
        if (currentSlide < 3)
        {
            currentSlide++;
            display.texture = slides[currentSlide];
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
