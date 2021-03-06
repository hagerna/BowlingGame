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
        SceneManager.LoadScene("Upgrades");
        Invoke(nameof(Deconstructor), 0.75f);
    }

    void Deconstructor() {
        Destroy(gameObject);
    }
}
