using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "BaseStats", menuName = "BattleSystem/BaseStats", order = 2)]
    public class BaseStats : ScriptableObject
    {
        public Weapon m_weapon = null;

        public int m_maxLife = 0;

        public int m_maxMana = 0;

        public int m_str = 0;
        public int m_def = 0;
        public int m_magStr = 0;
        public int m_magDef = 0;
        public int m_sp = 0;
    }
}
