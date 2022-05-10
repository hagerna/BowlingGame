using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopSystem { 

    [CreateAssetMenu(fileName = "ShopData", menuName = "Scriptable/CreateShopData")]

    public class ShopStuff : ScriptableObject
    {
        public ShopItem[] shopItems;
        public int selectedIndex;
        public string selectedName;

    }

    [System.Serializable] //allows editing from inspector

    public class ShopItem
    {
        public string itemName;
        public bool isUnlocked;
        public int unlockCost;
        public int unlockedLevel = 1;

        public Control[] controlUpgrades;
        public Speed[] speedUpgradeArray;
        public Bumper[] bumperUpgradeArray;
        public Lives[] ballLives;
        
        //public Balls[] additionalBalls;
        //public Pin[] pinUpgrades;
        /*
        public Control[] controlUpgrades;
        public Speed[] speedUpgrades;
        public Bumper[] bumperUpgrades;*/

    }

    [System.Serializable]

    public class Lives
    {
        public int unlockCost;
        public int numberOfLives;
    }

    [System.Serializable]

    public class Control
    {
        public int unlockCost;
        public int control;
    }

    [System.Serializable]

    public class Speed
    {
        public int unlockCost;
        public int speed;
    }

    [System.Serializable]

    public class Bumper
    {
        public int unlockCost;
        public int numberOfBumpers;
    }


}