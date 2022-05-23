using System.Collections.Generic;

namespace BattleSystem
{
    public class SkillManager
    {
        Dictionary<WeaponType, Dictionary<int, List<Skill>>> m_skillDatabase;

        public void LoadSkills(BattleSystemDatabase _database)
        {
            m_skillDatabase = new Dictionary<WeaponType, Dictionary<int, List<Skill>>>();
            foreach (Skill current in _database.m_skills)
            {
                if (!m_skillDatabase.ContainsKey(current.m_weapon))
                {
                    m_skillDatabase.Add(current.m_weapon, new Dictionary<int, List<Skill>>());
                }
                if (!m_skillDatabase[current.m_weapon].ContainsKey(current.m_level))
                {
                    m_skillDatabase[current.m_weapon].Add(current.m_level, new List<Skill>());
                }
                m_skillDatabase[current.m_weapon][current.m_level].Add(current);
            }
        }

        public List<Skill> GetSkills(WeaponType _weaponType, int _level)
        {
            if (m_skillDatabase.ContainsKey(_weaponType) && m_skillDatabase[_weaponType].ContainsKey(_level))
            {
                return m_skillDatabase[_weaponType][_level];
            }
            return new List<Skill>();
        }
    }
}
