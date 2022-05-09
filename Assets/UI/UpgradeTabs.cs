using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTabs : MonoBehaviour
{
    public GameObject[] tabs;
    GameObject currentTab;

    // Start is called before the first frame update
    void Start()
    {
        currentTab = tabs[0];
        currentTab.SetActive(true);
    }

    public void changeTab(string newTab)
    {
        currentTab.SetActive(false);
        switch (newTab)
        {
            case "control":
                currentTab = tabs[0];
                currentTab.SetActive(true);
                break;
            case "balls":
                currentTab = tabs[1];
                currentTab.SetActive(true);
                break;
            case "settings":
                currentTab = tabs[2];
                currentTab.SetActive(true);
                break;
            default:
                Debug.Log("error changing tabs");
                break;
        }
    }

    public void StartLevel()
    {
        GameManager.Instance.LevelReset();
    }
}
