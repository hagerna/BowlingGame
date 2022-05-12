using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public int incrementCost, incrementValue;
    }
}