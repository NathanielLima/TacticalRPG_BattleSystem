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
                // Load Database
                m_instance = new BattleSystemManager();
                m_instance.m_database = Resources.Load<BattleSystemDatabase>("BattleSystemDatabase");

                // Init Skills
                m_instance.m_skillManager.LoadSkills(m_instance.m_database);

                // Init BaseInventory
                m_instance.m_database.m_baseInventory.m_items.Clear();
                for (int i = 0; i < m_instance.m_database.m_baseInventory.m_itemsNames.Count; i++)
                {
                    foreach (Item currentItem in m_instance.m_database.m_items)
                    {
                        if (currentItem.m_name == m_instance.m_database.m_baseInventory.m_itemsNames[i])
                        {
                            Item item = currentItem;
                            item.m_count = m_instance.m_database.m_baseInventory.m_itemsCount[i];
                            m_instance.m_database.m_baseInventory.m_items.Add(item);
                            break;
                        }
                    }
                }

                // Init Teams
                foreach (Team currentTeam in m_instance.m_database.m_teams)
                {
                    currentTeam.m_units.Clear();
                    // Search for units
                    foreach(string currentUnitName in currentTeam.m_unitsNames)
                    {
                        // Init Unit
                        Unit unit = new Unit();
                        unit.m_name = currentUnitName;
                        // Get corresponding charcter sheet
                        foreach (CharacterSheet currentSheet in m_instance.m_database.m_characters)
                        {
                            if (currentSheet.m_name == currentUnitName)
                            {
                                unit.m_charSheet = currentSheet;
                                // Get corresponding weapon
                                foreach (Item currentItem in m_instance.m_database.m_items)
                                {
                                    if (currentItem.m_name == currentSheet.m_weaponName)
                                    {
                                        unit.m_charSheet.m_weapon = currentItem;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        currentTeam.AddUnit(unit);
                    }
                }
            }
            return m_instance;
        }
    }
}