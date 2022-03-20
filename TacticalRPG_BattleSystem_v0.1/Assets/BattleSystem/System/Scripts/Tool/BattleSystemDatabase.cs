using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "BattleSystemDatabase", menuName = "BattleSystem/BattleSystemDatabase", order = -1)]
    public class BattleSystemDatabase : ScriptableObject
    {
        public BaseInventory m_baseInventory;
        public List<BaseStats> m_baseStats = new List<BaseStats>();
        public List<Item> m_items = new List<Item>();
        public List<Skill> m_skills = new List<Skill>();
        public List<Team> m_teams = new List<Team>();
        public List<Weapon> m_weapons = new List<Weapon>();
    }
}