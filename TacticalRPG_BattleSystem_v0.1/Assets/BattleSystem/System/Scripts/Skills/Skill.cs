using System;

namespace BattleSystem
{
    [Serializable]
    public class Skill
    {
        public string m_name = "";
        public WeaponType m_weapon = WeaponType.SWORD;
        public int m_level = 1;

        public bool m_isMagic = false;
        public int m_cost = 0;
    }
}
