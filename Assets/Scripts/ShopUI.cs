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

        public Text costText, upgradeCostText;
        public Text totalPinsText;
        public Button buySelectButton;
        public Button buyUpgradeButton;

        //private string currentName;
        //private string selectedName;
     
        private int currentIndex = 0;
        private int selectedIndex = 0;

        private void Start()
        {
            //selectedName = shopData.selectedName;
            //currentName = selectedName;

            selectedIndex = shopData.selectedIndex;
            currentIndex = selectedIndex;

            //totalPinsText.text = "" + totalPins;
            setBallInfo();

            //buySelectButton.onClick.AddListener(()=> buySelectButtonMethod()); maybe just do this in Unity UI?



        //ballModels[currentIndex].SetActive(true);
            

        }

        private void setBallInfo()
        { 

            int currentLevel = shopData.shopItems[currentIndex].unlockCost;

        }

        public void buySelectButtonMethod(string ballName)
        {
            Debug.Log("Button clicked!");
            currentIndex = checkIndex(ballName);
            bool yesSelected = false;
            if(shopData.shopItems[currentIndex].isUnlocked)
            {
                yesSelected = true;
            } else
            {
                if (totalPins >= shopData.shopItems[currentIndex].unlockCost)
                {
                    totalPins -= shopData.shopItems[currentIndex].unlockCost;
                    //totalPinsText.text = "" + totalPins;
                    yesSelected = true;
                    GameManager.Instance.currentBall = ballName;
                    shopData.shopItems[currentIndex].isUnlocked = true;
                   
                       
                }
            }

            if(yesSelected)
            {
                buySelectButton.image.sprite = selectButtonImage;
                selectedIndex = currentIndex;
                shopData.selectedIndex = selectedIndex;
                buySelectButton.interactable = false; 
            }

        }

        private int checkIndex(string ballName)
        {
            switch (ballName)
            {
                case "fire":
                    currentIndex = 0;
                    return currentIndex;

                case "ghost":
                    currentIndex = 1;
                    return currentIndex;

                case "gold":
                    currentIndex = 2;
                    return currentIndex;

                case "vortex":
                    currentIndex = 3;
                    return currentIndex;

                default:
                    currentIndex = 0;
                    Debug.Log("Something went wrong with checkIndex! :(");
                    return currentIndex;
                   
            }
        }

        private void BuyButtonStatus()
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
        }

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