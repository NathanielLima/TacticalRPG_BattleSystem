using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "BaseInventory", menuName = "BattleSystem/BaseInventory", order = 1)]
    public class BaseInventory : ScriptableObject
    {
        public List<Item> m_items = new List<Item>();
        public List<Weapon> m_weapons = new List<Weapon>();
    }
}
