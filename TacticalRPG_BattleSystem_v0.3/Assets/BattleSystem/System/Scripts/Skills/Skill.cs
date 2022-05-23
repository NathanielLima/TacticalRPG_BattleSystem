using System;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Skill
    {
        public string m_name = "";

        public Sprite m_icon = null;
        public Animation m_animation = null;

        public TargetType m_target = TargetType.OTHER;
        public WeaponType m_weapon = WeaponType.SWORD;
        public int m_level = 1;

        public bool m_isMagic = false;
        public int m_cost = 0;

        public int m_range = 0;

        public int m_life = 0;

        public int m_str = 0;
        public int m_magStr = 0;
    }
}
