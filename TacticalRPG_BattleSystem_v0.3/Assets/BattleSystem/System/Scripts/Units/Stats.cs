using System.Collections.Generic;

namespace BattleSystem
{
    public class Stats
    {
        int m_currentLife = -1;
        Levelable m_maxLife = new Levelable();

        int m_currentMana = -1;
        Levelable m_maxMana = new Levelable();

        Levelable m_str = new Levelable();
        Levelable m_def = new Levelable();
        Levelable m_magStr = new Levelable();
        Levelable m_magDef = new Levelable();
        Levelable m_sp = new Levelable();

        public void SetStatBaseValue(StatName _statName, int _baseValue)
        {
            switch (_statName)
            {
                case StatName.MAX_LIFE:
                    m_maxLife.SetBaseValue(_baseValue);
                    break;
                case StatName.MAX_MANA:
                    m_maxMana.SetBaseValue(_baseValue);
                    break;
                case StatName.STRENGTH:
                    m_str.SetBaseValue(_baseValue);
                    break;
                case StatName.DEFENSE:
                    m_def.SetBaseValue(_baseValue);
                    break;
                case StatName.MAGIC_STRENGTH:
                    m_magStr.SetBaseValue(_baseValue);
                    break;
                case StatName.MAGIC_DEFENSE:
                    m_magDef.SetBaseValue(_baseValue);
                    break;
                case StatName.SPEED:
                    m_sp.SetBaseValue(_baseValue);
                    break;
                default:
                    break;
            }
        }

        public List<int> GetStats()
        {
            List<int> stats = new List<int>();
            stats.Add(m_str.GetValue());
            stats.Add(m_def.GetValue());
            stats.Add(m_magStr.GetValue());
            stats.Add(m_magDef.GetValue());
            stats.Add(m_sp.GetValue());
            return stats;
        }

        public int GetStat(StatName _stat)
        {
            switch(_stat)
            {
                case StatName.MAX_LIFE:
                    return m_maxLife.GetValue();
                case StatName.MAX_MANA:
                    return m_maxMana.GetValue();
                case StatName.STRENGTH:
                    return m_str.GetValue();
                case StatName.DEFENSE:
                    return m_def.GetValue();
                case StatName.MAGIC_STRENGTH:
                    return m_magStr.GetValue();
                case StatName.MAGIC_DEFENSE:
                    return m_magDef.GetValue();
                case StatName.SPEED:
                    return m_sp.GetValue();
                default:
                    return -1;
            }
        }

        public void SetCurrentLife(int _life)
        {
            m_currentLife = _life;
        }

        public int GetCurrentLife()
        {
            return m_currentLife;
        }

        public void SetCurrentMana(int _mana)
        {
            m_currentMana = _mana;
        }

        public int GetCurrentMana()
        {
            return m_currentMana;
        }

        public void AddDamages(int _str)
        {
            int damages = _str - m_def.GetValue();
            m_currentLife -= damages >= 0 ? damages : 0;
            if (m_currentLife < 0)
            {
                m_currentLife = 0;
            }
        }

        public void AddMagicDamages(int _magStr)
        {
            int damages = _magStr - m_magDef.GetValue();
            m_currentLife -= damages >= 0 ? damages : 0;
            if (m_currentLife < 0)
            {
                m_currentLife = 0;
            }
        }

        public void HealLife(int _life)
        {
            m_currentLife += _life;
            if (m_currentLife > m_maxLife.GetValue())
            {
                m_currentLife = m_maxLife.GetValue();
            }
        }

        public void HealMana(int _mana)
        {
            m_currentMana += _mana;
            if (m_currentMana > m_maxMana.GetValue())
            {
                m_currentMana = m_maxMana.GetValue();
            }
        }

        public bool LevelingStat(StatName _statName)
        {
            bool result = false;
            switch (_statName)
            {
                case StatName.MAX_LIFE:
                    result = m_maxLife.Leveling();
                    break;
                case StatName.MAX_MANA:
                    result = m_maxMana.Leveling();
                    break;
                case StatName.STRENGTH:
                    result = m_str.Leveling();
                    break;
                case StatName.DEFENSE:
                    result = m_def.Leveling();
                    break;
                case StatName.MAGIC_STRENGTH:
                    result = m_magStr.Leveling();
                    break;
                case StatName.MAGIC_DEFENSE:
                    result = m_magDef.Leveling();
                    break;
                case StatName.SPEED:
                    result = m_sp.Leveling();
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
