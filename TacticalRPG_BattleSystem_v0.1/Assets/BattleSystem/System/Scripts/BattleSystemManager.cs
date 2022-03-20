using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleSystem
{
    public class BattleSystemManager
    {
        static BattleSystemManager m_instance = null;

        public BattleSystemDatabase m_database = null;
        public SkillManager m_skillManager = new SkillManager();

        public static BattleSystemManager GetSingleton()
        {
            if (m_instance == null)
            {
                m_instance = new BattleSystemManager();
                m_instance.m_database = AssetDatabase.LoadAssetAtPath<BattleSystemDatabase>("Assets/BattleSystem/BattleSystemDatabase.asset");
                m_instance.m_skillManager.LoadSkills(m_instance.m_database);
            }
            return m_instance;
        }
    }
}