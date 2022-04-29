using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopSystem { 

    [CreateAssetMenu(fileName = "ShopData", menuName = "Scriptable/CreateShopData")]

    public class ShopStuff : ScriptableObject
    {
        public ShopItem[] shopItems;

    }

    [System.Serializable] //allows editing from inspector

    public class ShopItem
    {
        public string itemName;
        public bool isUnlocked;
        public int unlockCost;

        public Control[] controlUpgrades;
        public Speed[] speedUpgradeArray;
        public Bumper[] bumperUpgradeArray;
        
        //public Balls[] additionalBalls;
        //public Pin[] pinUpgrades;
        /*
        public Control[] controlUpgrades;
        public Speed[] speedUpgrades;
        public Bumper[] bumperUpgrades;*/

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