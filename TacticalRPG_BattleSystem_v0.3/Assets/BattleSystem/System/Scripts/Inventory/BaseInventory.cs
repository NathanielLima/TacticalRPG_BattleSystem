using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class BaseInventory
    {
        public List<Item> m_items = new List<Item>();
        public List<Weapon> m_weapons = new List<Weapon>();
        public List<string> m_itemsNames = new List<string>();
        public List<int> m_itemsCount = new List<int>();
    }
}
