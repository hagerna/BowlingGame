using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ShopSystem
{

    public class ShopUI : MonoBehaviour
    {
        public int totalPins = 5000;

        public Sprite equiptedButtonImage;
        public Sprite buyButtonImage;
        public Sprite selectButtonImage;

        public ShopStuff shopData;

        //public GameObject[] ballModels;

        public Text[] costText, currentLevelText;
        public Text totalPinsText;
        public Button buySelectButton;
        public Button buyUpgradeButton;


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
            }
        }

        public void buySelectButtonMethod(int index)
        {
            ShopItem item = shopData.shopItems[index];
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
                        Debug.Log("I am well behaved :)");
                        // set button to equip
                    }
                    else
                    {
                        item.unlockCost += item.incrementCost;
                        GameManager.Instance.baseData[item.itemName] += item.incrementValue;
                        currentLevelText[index-4].text = "Current Level = " + item.unlockedLevel;
                        costText[index].text = "Cost: " + item.unlockedLevel;
                        Debug.Log("I like stats");
                    }
                    item.isUnlocked = true;      
                }
            }

            /* if(yesSelected)
            {
                buySelectButton.image.sprite = selectButtonImage;
                selectedIndex = currentIndex;
                shopData.selectedIndex = selectedIndex;
                buySelectButton.interactable = false; 
            } */

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