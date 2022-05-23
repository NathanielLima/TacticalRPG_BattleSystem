using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Team
    {
        public string m_name = "";
        public List<Unit> m_units = new List<Unit>();
        public List<String> m_unitsNames = new List<string>();

        Dictionary<string, int> m_spCounter = new Dictionary<string, int>();

        int m_totalSpeed = 0;

        public void AddUnit(Unit _unit)
        {
            if (!m_units.Contains(_unit))
            {
                m_units.Add(_unit);
                m_spCounter.Add(_unit.m_name, 0);
            }
        }

        public Unit RemoveUnit(string _name)
        {
            foreach (Unit current in m_units)
            {
                if (current.m_name == _name)
                {
                    Unit remove = current;
                    m_units.Remove(current);
                    m_spCounter.Remove(current.m_name);
                    return remove;
                }
            }
            return new Unit();
        }

        public void ComputeStartSpeed()
        {
            m_totalSpeed = 0;
            foreach (Unit current in m_units)
            {
                m_totalSpeed += current.GetStat(StatName.SPEED);
            }
            foreach (Unit current in m_units)
            {
                m_spCounter[current.m_name] = m_totalSpeed - current.GetStat(StatName.SPEED);
            }
        }

        public Unit GetCurrentTurnUnit()
        {
            Unit result = m_units[0];
            foreach (Unit current in m_units)
            {
                if (m_spCounter[current.m_name] < m_spCounter[result.m_name])
                {
                    result = current;
                }
            }
            m_spCounter[result.m_name] += (m_totalSpeed - result.GetStat(StatName.SPEED));
            return result;
        }
    }
}
