using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ShopSystem
{

    public class ShopUI : MonoBehaviour
    {
        public Sprite buyButtonImage;
        public Sprite equiptedButtonImage;
        public Sprite equipButtonImage;
        public Button myButton;

        public ShopStuff shopData;

        private int selectedIndex;

        //public GameObject[] ballModels;

        public Text[] costText, currentLevelText;
        public Button[] selectButton;
        public Text totalPinsText;


        private void Start()
        {
            

            int itemNumber = 0;
            foreach (ShopItem item in shopData.shopItems)
            {
                item.isUnlocked = false;
                costText[itemNumber].text = "Cost: " + item.unlockCost;
                if (itemNumber >= 4)
                {
                    currentLevelText[itemNumber - 4].text = "Current Level: " + item.unlockedLevel;
                }
                itemNumber++;
            }
        }

        private void Update()
        {
            totalPinsText.text = "Total Pins: " + GameManager.Instance.totalScore;
        }

        public void buySelectButtonMethod(int index)
        {
            ShopItem item = shopData.shopItems[index];
            Button currentButton = selectButton[index];
            bool yesSelected = false;
            if(item.isUnlocked)
            {
                yesSelected = true;
            } else
            {
                if (GameManager.Instance.totalScore >= item.unlockCost)
                {
                    GameManager.Instance.totalScore -= item.unlockCost;
                    if (item.incrementCost == 0) // if ball upgrade and not a stat upgrade
                    {
                        GameManager.Instance.currentBall = item.itemName;
                        yesSelected = true;
                        costText[index].enabled = false;
                        costText[index].text = "";

                        for (int i = 0; i <= 4 + 1; i++)
                        {
                            if (shopData.shopItems[i].isUnlocked)
                            {
                                if (i == index)
                                {
                                    currentButton.image.sprite = equiptedButtonImage;
                                    currentButton.interactable = true;
                                } else
                                {
                                    selectButton[i].image.sprite = equipButtonImage;
                                    currentButton.interactable = true;
                                }
                            }
                        }
                        item.isUnlocked = true;


                    }
                    else
                    {
                        item.unlockCost += item.incrementCost;
                        GameManager.Instance.baseData[item.itemName] += item.incrementValue;
                        item.unlockedLevel++;
                        currentLevelText[index-4].text = "Current Level: " + item.unlockedLevel;
                        costText[index].text = "Cost: " + item.unlockCost;
                        currentButton.interactable = true;
                    }
                       
                }
            }
           
             if(yesSelected)
            {
                for (int i = 0; i <= 4 + 1; i++)
                {
                    if (shopData.shopItems[i].isUnlocked)
                    {
                        if (i == index)
                        {
                            currentButton.image.sprite = equiptedButtonImage;
                            currentButton.interactable = true;
                        }
                        else
                        {
                            selectButton[i].image.sprite = equipButtonImage;
                            currentButton.interactable = true;
                        }
                    }
                }
                GameManager.Instance.currentBall = item.itemName;



            } 

        }
    }


}