using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
    public Text pinsHit, strikes, totalScore;
    // Start is called before the first frame update
    void Start()
    {
        pinsHit.CrossFadeAlpha(0, 0, true);
        strikes.CrossFadeAlpha(0, 0, true);
        totalScore.CrossFadeAlpha(0, 0, true);
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        pinsHit.text = "Total Pins Hit: " + GameManager.Instance.pinsCollected;
        float strikePoints = GameManager.Instance.gameData["strikes"] * 10;
        strikes.text = "Points from Strikes: " + strikePoints;
        totalScore.text = "Total Score: " + GameManager.Instance.TotalScore();
        pinsHit.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(0.5f);
        strikes.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(0.5f);
        totalScore.CrossFadeAlpha(1, 1, true);
    }

    public void LoadUpgradesScene()
    {
        Debug.Log("Change to Upgrades scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
        GameManager.Instance.LevelReset();
    }
}
