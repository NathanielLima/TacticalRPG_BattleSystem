using System;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Item
    {
        public string m_name = "";

        public Sprite m_icon = null;
        public Animation m_animation = null;

        public ItemType m_itemType = ItemType.CONSUMABLE;
        public TargetType m_target = TargetType.OTHER;
        public WeaponType m_weaponType = WeaponType.SWORD;

        public int m_count = 1;

        public int m_range = 0;

        public int m_life = 0;
        public int m_mana = 0;

        public int m_str = 0;
        public int m_def = 0;
        public int m_magStr = 0;
        public int m_magDef = 0;
        public int m_sp = 0;
    }
}
