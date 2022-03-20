using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BattleSystem
{
    public class BattleSystemEditor : EditorWindow
    {
        BattleSystemDatabase m_database = null;
        int m_currentTab = 0;
        string[] m_tabs = {
            "Characters",
            "Enemies",
            "Teams",
            "Items",
            //"Inventory",
            "Skills"
        };

        //skill tab
        Vector2 m_skillScroll = Vector2.zero;
        string m_skillSearchFilter = "";
        Skill m_selectedSkill = null;

        [MenuItem("Window/BattleSystem Editor")]
        public static void ShowWindow()
        {
            BattleSystemEditor window = GetWindow<BattleSystemEditor>("BattleSystem Editor");
            window.minSize = new Vector2(1080, 768);
            window.maxSize = new Vector2(1080, 768);
            window.m_database = AssetDatabase.LoadAssetAtPath<BattleSystemDatabase>("Assets/BattleSystem/BattleSystemDatabase.asset");
        }

        void SortDatabase()
        {
            //m_database.m_baseStats = m_database.m_baseStats.OrderBy(x => x.m_name).ToList();
            m_database.m_items = m_database.m_items.OrderBy(x => x.m_name).ToList();
            m_database.m_skills = m_database.m_skills.OrderBy(x => x.m_name).ToList();
            m_database.m_teams = m_database.m_teams.OrderBy(x => x.m_name).ToList();
            m_database.m_weapons = m_database.m_weapons.OrderBy(x => x.m_name).ToList();
        }

        void OnGUI()
        {
            m_currentTab = GUILayout.Toolbar(m_currentTab, m_tabs);
            switch (m_currentTab)
            {
                case 0:
                    CharactersGUI();
                    break;
                case 1:
                    EnemiesGUI();
                    break;
                case 2:
                    TeamsGUI();
                    break;
                case 3:
                    ItemsGUI();
                    break;
                /*case 4:
                    InventoryGUI();
                    break;*/
                case 4:
                    SkillsGUI();
                    break;
                default:
                    break;
            }
        }

        void CharactersGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            GUILayout.TextField("", EditorStyles.toolbarSearchField);
            //elements
            GUILayout.BeginScrollView(Vector2.zero, EditorStyles.helpBox);
            GUILayout.EndScrollView();
            //add new element
            GUILayout.Button("Add new");
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void EnemiesGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            GUILayout.TextField("", EditorStyles.toolbarSearchField);
            //elements
            GUILayout.BeginScrollView(Vector2.zero, EditorStyles.helpBox);
            GUILayout.EndScrollView();
            //add new element
            GUILayout.Button("Add new");
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void ItemsGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            GUILayout.TextField("", EditorStyles.toolbarSearchField);
            //elements
            GUILayout.BeginScrollView(Vector2.zero, EditorStyles.helpBox);
            GUILayout.EndScrollView();
            //add new element
            GUILayout.Button("Add new");
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void InventoryGUI()
        {
            //
        }

        bool HaveSkill(string name)
        {
            foreach (Skill current in m_database.m_skills)
            {
                if (current.m_name == name)
                {
                    return true;
                }
            }
            return false;
        }

        void SkillsGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            m_skillSearchFilter = GUILayout.TextField(m_skillSearchFilter, EditorStyles.toolbarSearchField);
            //elements
            m_skillScroll = GUILayout.BeginScrollView(m_skillScroll, EditorStyles.helpBox);
            GUIStyle elemStyle = new GUIStyle(GUI.skin.button);
            elemStyle.alignment = TextAnchor.MiddleLeft;
            foreach (Skill current in m_database.m_skills)
            {
                if (current.m_name.Contains(m_skillSearchFilter) && GUILayout.Button(current.m_name, elemStyle))
                {
                    GUI.FocusControl(null);
                    m_selectedSkill = current;
                }
            }
            GUILayout.EndScrollView();
            //add new element
            if (GUILayout.Button("Add new"))
            {
                string tmpName = "New skill";
                int tmpCount = 1;
                while (HaveSkill(tmpName))
                {
                    tmpName = "New skill (" + tmpCount + ")";
                    ++tmpCount;
                }
                Skill tmpSkill = new Skill();
                tmpSkill.m_name = tmpName;
                m_database.m_skills.Add(tmpSkill);
                m_selectedSkill = tmpSkill;
                SortDatabase();
                EditorUtility.SetDirty(m_database);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            if (m_selectedSkill != null)
            {
                //id
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Id");
                string tmpName = GUILayout.TextField(m_selectedSkill.m_name);
                if (!HaveSkill(tmpName))
                {
                    m_selectedSkill.m_name = tmpName;
                    SortDatabase();
                }
                GUILayout.EndHorizontal();
                //weapon
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Use with weapon type");
                m_selectedSkill.m_weapon = (WeaponType)EditorGUILayout.EnumPopup(m_selectedSkill.m_weapon);
                GUILayout.EndHorizontal();
                //level
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Level of mastery");
                m_selectedSkill.m_level = EditorGUILayout.IntField(m_selectedSkill.m_level);
                GUILayout.EndHorizontal();
                //isMagic
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Is magic");
                m_selectedSkill.m_isMagic = GUILayout.Toggle(m_selectedSkill.m_isMagic, "");
                GUILayout.EndHorizontal();
                //cost
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Mana cost");
                m_selectedSkill.m_cost = EditorGUILayout.IntField(m_selectedSkill.m_cost);
                GUILayout.EndHorizontal();
                //remove element
                if (GUILayout.Button("Remove"))
                {
                    m_database.m_skills.Remove(m_selectedSkill);
                    m_selectedSkill = null;
                    SortDatabase();
                    EditorUtility.SetDirty(m_database);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void TeamsGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            GUILayout.TextField("", EditorStyles.toolbarSearchField);
            //elements
            GUILayout.BeginScrollView(Vector2.zero, EditorStyles.helpBox);
            GUILayout.EndScrollView();
            //add new element
            GUILayout.Button("Add new");
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            EditorUtility.SetDirty(m_database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}