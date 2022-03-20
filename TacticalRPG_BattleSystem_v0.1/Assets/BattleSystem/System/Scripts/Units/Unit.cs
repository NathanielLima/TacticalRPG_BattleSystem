using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "Unit", menuName = "BattleSystem/Unit", order = 7)]
    public class Unit : ScriptableObject
    {
        public string m_name = "";
        public Weapon m_weapon = null;

        public BaseStats m_baseStats = null;
        Stats m_stats = new Stats();
        List<Skill> m_skills = new List<Skill>();
        Dictionary<WeaponType, Levelable> m_mastery = new Dictionary<WeaponType, Levelable>();

        public void InitMastery()
        {
            m_skills.Clear();
            for (int i = 0; i < (int)WeaponType.NB_WEAPON_TYPES; i++)
            {
                m_mastery.Add((WeaponType)i, new Levelable());
            }
        }

        public void SetBaseStats()
        {
            m_weapon = m_baseStats.m_weapon;

            m_stats.SetCurrentLife(m_baseStats.m_maxLife);
            m_stats.SetStatBaseValue(StatName.MAX_LIFE, m_baseStats.m_maxLife);

            m_stats.SetCurrentMana(m_baseStats.m_maxMana);
            m_stats.SetStatBaseValue(StatName.MAX_MANA, m_baseStats.m_maxMana);

            m_stats.SetStatBaseValue(StatName.STRENGTH, m_baseStats.m_str);
            m_stats.SetStatBaseValue(StatName.DEFENSE, m_baseStats.m_def);
            m_stats.SetStatBaseValue(StatName.MAGIC_STRENGTH, m_baseStats.m_magStr);
            m_stats.SetStatBaseValue(StatName.MAGIC_DEFENSE, m_baseStats.m_magDef);
            m_stats.SetStatBaseValue(StatName.SPEED, m_baseStats.m_sp);
        }

        public bool IsAlive()
        {
            return m_stats.GetCurrentLife() > 0;
        }

        public List<int> GetStats()
        {
            return m_stats.GetStats();
        }

        public int GetCurrentLife()
        {
            return m_stats.GetCurrentLife();
        }

        public int GetMaxLife()
        {
            return m_stats.GetMaxLife();
        }

        public int GetCurrentMana()
        {
            return m_stats.GetCurrentMana();
        }

        public int GetMaxMana()
        {
            return m_stats.GetMaxMana();
        }

        public int GetStrength()
        {
            if (m_weapon)
            {
                return m_stats.GetStrength() + m_weapon.m_str;
            }
            return m_stats.GetStrength();
        }

        public int GetDefense()
        {
            return m_stats.GetDefense();
        }

        public int GetMagicStrength()
        {
            if (m_weapon)
            {
                return m_stats.GetMagicStrength() + m_weapon.m_magStr;
            }
            return m_stats.GetMagicStrength();
        }

        public int GetMagicDefense()
        {
            return m_stats.GetMagicDefense();
        }

        public int GetSpeed()
        {
            return m_stats.GetSpeed();
        }

        public List<Skill> GetSkills()
        {
            return m_skills;
        }

        public void AddDamages(int _str)
        {
            m_stats.AddDamages(_str);
            if (m_stats.GetCurrentLife() == 0)
            {
                LogManager.GetSingleton().Log(m_name + " died");
            }
        }

        public void AddMagicDamages(int _magStr)
        {
            m_stats.AddMagicDamages(_magStr);
            if (m_stats.GetCurrentLife() == 0)
            {
                LogManager.GetSingleton().Log(m_name + " died");
            }
        }

        public void HealLife(int _life)
        {
            m_stats.HealLife(_life);
        }

        public void HealMana(int _mana)
        {
            m_stats.HealMana(_mana);
        }

        public bool Act(ActionType _actionType)
        {
            bool result = false;
            switch(_actionType)
            {
                case ActionType.ATTACK:
                    result = m_stats.LevelingStat(StatName.STRENGTH);
                    if (result)
                    {
                        LogManager.GetSingleton().Log(m_name + " earn 1 Str");
                    }
                    if (m_weapon && m_mastery[m_weapon.m_type].Leveling())
                    {
                        List<Skill> skills = BattleSystemManager.GetSingleton().m_skillManager.GetSkills(m_weapon.m_type, m_mastery[m_weapon.m_type].GetValue());
                        foreach(Skill current in skills)
                        {
                            m_skills.Add(current);
                        }
                    }
                    break;
                case ActionType.SKILL:
                    bool result1Skill = m_stats.LevelingStat(StatName.MAX_MANA);
                    bool result2Skill = m_stats.LevelingStat(StatName.STRENGTH);
                    result = result1Skill || result2Skill;
                    break;
                case ActionType.USE:
                    break;
                case ActionType.MOVE:
                    result = m_stats.LevelingStat(StatName.SPEED);
                    if (result)
                    {
                        LogManager.GetSingleton().Log(m_name + " earn 1 Sp");
                    }
                    break;
                case ActionType.MAGIC_SKILL:
                    bool result1MagSkill = m_stats.LevelingStat(StatName.MAX_MANA);
                    bool result2MagSkill = m_stats.LevelingStat(StatName.MAGIC_STRENGTH);
                    result = result1MagSkill || result2MagSkill;
                    break;
                case ActionType.HIT:
                    result = m_stats.LevelingStat(StatName.DEFENSE);
                    if (result)
                    {
                        LogManager.GetSingleton().Log(m_name + " earn 1 Def");
                    }
                    break;
                case ActionType.MAGIC_HIT:
                    result = m_stats.LevelingStat(StatName.MAGIC_DEFENSE);
                    if (result)
                    {
                        LogManager.GetSingleton().Log(m_name + " earn 1 Mag Def");
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public static Unit CreateCopy(Unit _source)
        {
            Unit unit = CreateInstance<Unit>();
            unit.m_name = _source.m_name;
            if (_source.m_weapon)
            {
                unit.m_weapon = Weapon.CreateCopy(_source.m_weapon);
            }
            unit.m_baseStats = _source.m_baseStats;
            unit.m_stats = _source.m_stats;
            return unit;
        }
    }
}
