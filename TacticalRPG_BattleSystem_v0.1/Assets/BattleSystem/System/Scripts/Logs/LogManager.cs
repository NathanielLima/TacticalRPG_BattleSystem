using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class LogManager
    {
        static LogManager m_instance = null;
        Logger m_logger = null;

        public static LogManager GetSingleton()
        {
            if (m_instance == null)
            {
                m_instance = new LogManager();
            }
            return m_instance;
        }

        public void SetLogger(Logger _logger)
        {
            m_logger = _logger;
        }

        public void Log(string _log)
        {
            if (m_logger)
            {
                m_logger.Log(_log);
            }
        }
    }
}