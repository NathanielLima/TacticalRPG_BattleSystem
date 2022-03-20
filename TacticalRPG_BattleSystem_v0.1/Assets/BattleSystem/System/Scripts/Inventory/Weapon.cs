using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "BattleSystem/Weapon", order = 8)]
    public class Weapon : ScriptableObject
    {
        public string m_name = "";
        public int m_count = 1;
        public WeaponType m_type = WeaponType.SWORD;

        public int m_range = 1;
        public int m_str;
        public int m_magStr;

        public static Weapon CreateCopy(Weapon _source)
        {
            Weapon weapon = CreateInstance<Weapon>();
            weapon.m_name = _source.m_name;
            weapon.m_count = _source.m_count;
            weapon.m_type = _source.m_type;
            weapon.m_range = _source.m_range;
            weapon.m_str = _source.m_str;
            weapon.m_magStr = _source.m_magStr;
            return weapon;
        }
    }
}
