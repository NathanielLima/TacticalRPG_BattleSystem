using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Unit
    {
        public string m_name = "";
        public Item m_weapon = null;

        public CharacterSheet m_charSheet = null;
        Stats m_stats = new Stats();
        Stats m_buffs = new Stats();
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
            m_weapon = m_charSheet.m_weapon;

            m_stats.SetCurrentLife(m_charSheet.m_maxLife);
            m_stats.SetStatBaseValue(StatName.MAX_LIFE, m_charSheet.m_maxLife);

            m_stats.SetCurrentMana(m_charSheet.m_maxMana);
            m_stats.SetStatBaseValue(StatName.MAX_MANA, m_charSheet.m_maxMana);

            m_stats.SetStatBaseValue(StatName.STRENGTH, m_charSheet.m_str);
            m_stats.SetStatBaseValue(StatName.DEFENSE, m_charSheet.m_def);
            m_stats.SetStatBaseValue(StatName.MAGIC_STRENGTH, m_charSheet.m_magStr);
            m_stats.SetStatBaseValue(StatName.MAGIC_DEFENSE, m_charSheet.m_magDef);
            m_stats.SetStatBaseValue(StatName.SPEED, m_charSheet.m_sp);
        }

        public bool IsAlive()
        {
            return m_stats.GetCurrentLife() > 0;
        }

        public List<int> GetStats()
        {
            return m_stats.GetStats();
        }

        public int GetStat(StatName _stat)
        {
            switch (_stat)
            {
                case StatName.STRENGTH:
                    return m_stats.GetStat(_stat) + m_weapon.m_str;
                case StatName.MAGIC_STRENGTH:
                    return m_stats.GetStat(_stat) + m_weapon.m_magStr;
                default:
                    return m_stats.GetStat(_stat);
            }
        }

        public int GetCurrentLife()
        {
            return m_stats.GetCurrentLife();
        }

        public int GetCurrentMana()
        {
            return m_stats.GetCurrentMana();
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
                    if (m_weapon.m_name != "" && m_mastery[m_weapon.m_weaponType].Leveling())
                    {
                        List<Skill> skills = BattleSystemManager.GetSingleton().m_skillManager.GetSkills(m_weapon.m_weaponType, m_mastery[m_weapon.m_weaponType].GetValue());
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
    }
}
