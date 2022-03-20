namespace BattleSystem
{
    public class Levelable
    {
        int m_value = 0;

        int m_currentLevel = 1;
        int m_currentProgression = 0;
        int m_maxProgression = 10;

        public void SetBaseValue(int _baseValue)
        {
            m_value = _baseValue + m_currentLevel - 1;
        }

        public int GetValue()
        {
            return m_value;
        }

        void ComputeNextLevel()
        {
            ++m_value;
            ++m_currentLevel;
            m_currentProgression = 0;
            m_maxProgression = 10 * (m_currentLevel + (m_currentLevel - 1));
        }

        public bool Leveling()
        {
            ++m_currentProgression;
            if (m_currentProgression == m_maxProgression)
            {
                ComputeNextLevel();
                return true;
            }
            return false;
        }
    }
}
