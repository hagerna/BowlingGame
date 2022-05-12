using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ShopSystem
{

    public class ShopUI : MonoBehaviour
    {
        public int totalPins = 5000;

      
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
            Debug.Log("Button clicked!");
            bool yesSelected = false;
            if(item.isUnlocked)
            {
                yesSelected = true;
            } else
            {
                if (GameManager.Instance.totalScore >= item.unlockCost)
                {
                    GameManager.Instance.totalScore -= item.unlockCost;
                    if (item.incrementCost == 0) //we know it is a ball and not a stat upgrade
                    {
                        GameManager.Instance.currentBall = item.itemName;
                        yesSelected = true;
                        costText[index].enabled = false;

                        //buttonStatus(index, currentButton); // set button to equip

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
                        Debug.Log("I like stats");
                    }
                       
                }
            }
           
             if(yesSelected)
            {
                Debug.Log("Spot 1");
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

        private void buttonStatus(int index, Button currentButton)
        {
            ShopItem item = shopData.shopItems[index];

            Debug.Log("Spot 2");
            if (item.isUnlocked)
            {
                //if selectedIndex is not equal to index set unlockBtn interactable false else make it true
                currentButton.interactable = selectedIndex != index ? true : false;

                //set the text
                currentButton.image.sprite = selectedIndex != index ? equiptedButtonImage : equipButtonImage; //change to actual images

            } else if (!item.isUnlocked)
            {
                currentButton.interactable = true;
                
            }
        }

        /*
         *         private void UnlockButtonStatus()
        {
            //if current item is unlocked
            if (shopData.shopItems[currentIndex].isUnlocked)
            {
                //if selectedIndex is not equal to currentIndex set unlockBtn interactable false else make it true
                unlockBtn.interactable = selectedIndex != currentIndex ? true : false;
                //set the text
                unlockBtnText.text = selectedIndex == currentIndex ? "Selected" : "Select";
            }
            else if (!shopData.shopItems[currentIndex].isUnlocked) //if current item is not unlocked
            {
                unlockBtn.interactable = true;  //set the unlockbtn interactable
                unlockBtnText.text = shopData.shopItems[currentIndex].unlockCost + ""; //set the text as cost of item
            }
        }

        /* private void BuyButtonStatus()
        {
            if (shopData.shopItems[currentIndex].isUnlocked)
            {
                //buyButton.interactable = selectedIndex != currentIndex ? true : false;
                if(selectedIndex == currentIndex)
                {
                    buySelectButton.interactable = false;
                }
                else
                {
                    buySelectButton.interactable = true;
                }
                 
                buySelectButton.image.sprite = selectedIndex != currentIndex ? equiptedButtonImage : selectButtonImage ; //change to actual images
            }
            else
            {
                buySelectButton.interactable = true;
                buySelectButton.image.sprite = buyButtonImage;
            }
        } */

        /*

        private void UpgradeButtonMethod(string ballName)
        {

            //get the next level index
            int nextLevelIndex = shopData.shopItems[currentIndex].unlockedLevel + 1;
            //we check if we have enough coins
            if (totalPins >= shopData.shopItems[currentIndex].controlUpgrades[nextLevelIndex].unlockCost)
            {
                totalPins -= shopData.shopItems[currentIndex].controlUpgrades[nextLevelIndex].unlockCost;
                totalPinsText.text = "" + totalPins;          //set the coins text
                                                                //if yes we increate the unlockedLevel by 1
                shopData.shopItems[currentIndex].unlockedLevel++;

                //we check if are not at max level
                if (shopData.shopItems[currentIndex].unlockedLevel < shopData.shopItems[currentIndex].controlUpgrades.Length - 1)
                {
                    upgradeCostText.text = "Upgrade Cost " +
                        shopData.shopItems[currentIndex].controlUpgrades[nextLevelIndex + 1].unlockCost;
                }
                else    //we check if we are at max level
                {
                    buyUpgradeButton.interactable = false;            //set upgradeBtn interactable to false
                    upgradeCostText.text = "Max Level Reached";    //set the btn text
                }

                setBallInfo();
            } 
        } */

    }


}