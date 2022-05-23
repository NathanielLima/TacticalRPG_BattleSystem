using System;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Weapon
    {
        public string m_name = "";
        public int m_count = 1;
        public WeaponType m_type = WeaponType.SWORD;

        public int m_range = 1;
        public int m_str;
        public int m_magStr;
    }
}
