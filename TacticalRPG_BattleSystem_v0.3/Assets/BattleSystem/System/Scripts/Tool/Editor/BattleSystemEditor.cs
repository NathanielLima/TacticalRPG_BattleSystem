using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BattleSystem
{
    [Obsolete]
    public class BattleSystemEditor : EditorWindow
    {
        BattleSystemDatabase m_database = null;
        int m_currentTab = 0;
        string[] m_tabs = {
            "Units",
            "Teams",
            "Items",
            "Inventory",
            "Skills"
        };

        //unit tab
        Vector2 m_unitScroll = Vector2.zero;
        string m_unitSearchFilter = "";
        CharacterSheet m_selectedUnit = null;
        int m_currentUnitTab = 0;
        string[] m_unitTabs = {
            "Character sheet",
            "Animations"
        };

        //team tab
        Vector2 m_teamScroll = Vector2.zero;
        string m_teamSearchFilter = "";
        Team m_selectedTeam = null;

        //item tab
        Vector2 m_itemScroll = Vector2.zero;
        string m_itemSearchFilter = "";
        Item m_selectedItem = null;

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
            window.m_database = AssetDatabase.LoadAssetAtPath<BattleSystemDatabase>("Assets/BattleSystem/Resources/BattleSystemDatabase.asset");
        }

        void SortDatabase()
        {
            m_database.m_characters = m_database.m_characters.OrderBy(x => x.m_name).ToList();
            m_database.m_items = m_database.m_items.OrderBy(x => x.m_name).ToList();
            m_database.m_skills = m_database.m_skills.OrderBy(x => x.m_name).ToList();
            m_database.m_teams = m_database.m_teams.OrderBy(x => x.m_name).ToList();
        }

        void SaveDatabase()
        {
            EditorUtility.SetDirty(m_database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void OnGUI()
        {
            m_currentTab = GUILayout.Toolbar(m_currentTab, m_tabs);
            switch (m_currentTab)
            {
                case 0:
                    UnitsGUI();
                    break;
                case 1:
                    TeamsGUI();
                    break;
                case 2:
                    ItemsGUI();
                    break;
                case 3:
                    InventoryGUI();
                    break;
                case 4:
                    SkillsGUI();
                    break;
                default:
                    break;
            }
        }

        bool HaveUnit(string name)
        {
            foreach (CharacterSheet current in m_database.m_characters)
            {
                if (current.m_name == name)
                {
                    return true;
                }
            }
            return false;
        }

        void UnitsCharacterSheetGUI()
        {
            //Id
            GUILayout.Label("-----Character");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Name");
            string tmpName = GUILayout.TextField(m_selectedUnit.m_name);
            if (!HaveUnit(tmpName))
            {
                m_selectedUnit.m_name = tmpName;
                SortDatabase();
            }
            GUILayout.EndHorizontal();
            //Face
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Face");
            m_selectedUnit.m_face = (Sprite)EditorGUILayout.ObjectField(m_selectedUnit.m_face, typeof(Sprite));
            GUILayout.EndHorizontal();
            //Description
            /*GUIStyle descStyle = new GUIStyle(GUI.skin.textArea);
            descStyle.fixedHeight = 128.0f;
            EditorGUILayout.PrefixLabel("Description");
            GUILayout.TextArea("", descStyle);*/
            //Weapon
            GUILayout.Label("");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Weapon");
            GUI.contentColor = HaveItem(m_selectedUnit.m_weaponName) && IsWeapon(m_selectedUnit.m_weaponName) ? Color.green : Color.red;
            m_selectedUnit.m_weaponName = GUILayout.TextField(m_selectedUnit.m_weaponName);
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            //Max Life
            GUILayout.Label("");
            GUILayout.Label("-----Stats");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Life");
            m_selectedUnit.m_maxLife = EditorGUILayout.IntField(m_selectedUnit.m_maxLife);
            GUILayout.EndHorizontal();
            //Max Mana
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Mana");
            m_selectedUnit.m_maxMana = EditorGUILayout.IntField(m_selectedUnit.m_maxMana);
            GUILayout.EndHorizontal();
            //Strength
            GUILayout.Label("");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Str");
            m_selectedUnit.m_str = EditorGUILayout.IntField(m_selectedUnit.m_str);
            GUILayout.EndHorizontal();
            //Defense
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Def");
            m_selectedUnit.m_def = EditorGUILayout.IntField(m_selectedUnit.m_def);
            GUILayout.EndHorizontal();
            //Magic strength
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Mag Str");
            m_selectedUnit.m_magStr = EditorGUILayout.IntField(m_selectedUnit.m_magStr);
            GUILayout.EndHorizontal();
            //Magic defense
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Mag Def");
            m_selectedUnit.m_magDef = EditorGUILayout.IntField(m_selectedUnit.m_magDef);
            GUILayout.EndHorizontal();
            //Speed
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Sp");
            m_selectedUnit.m_sp = EditorGUILayout.IntField(m_selectedUnit.m_sp);
            GUILayout.EndHorizontal();
        }

        void UnitsAnimationsGUI()
        {
            GUILayout.Label("-----Controller");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Controller");
            m_selectedUnit.m_controller = (RuntimeAnimatorController)EditorGUILayout.ObjectField(m_selectedUnit.m_controller, typeof(RuntimeAnimatorController));
            GUILayout.EndHorizontal();

            GUILayout.Label("");
            GUILayout.Label("-----Idle");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Left");
            m_selectedUnit.m_idle[(int)AnimDirection.LEFT] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_idle[(int)AnimDirection.LEFT], typeof(AnimationClip));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Right");
            m_selectedUnit.m_idle[(int)AnimDirection.RIGHT] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_idle[(int)AnimDirection.RIGHT], typeof(AnimationClip));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Up");
            m_selectedUnit.m_idle[(int)AnimDirection.UP] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_idle[(int)AnimDirection.UP], typeof(AnimationClip));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Down");
            m_selectedUnit.m_idle[(int)AnimDirection.DOWN] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_idle[(int)AnimDirection.DOWN], typeof(AnimationClip));
            GUILayout.EndHorizontal();

            GUILayout.Label("");
            GUILayout.Label("-----Walk");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Left");
            m_selectedUnit.m_walk[(int)AnimDirection.LEFT] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_walk[(int)AnimDirection.LEFT], typeof(AnimationClip));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Right");
            m_selectedUnit.m_walk[(int)AnimDirection.RIGHT] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_walk[(int)AnimDirection.RIGHT], typeof(AnimationClip));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Up");
            m_selectedUnit.m_walk[(int)AnimDirection.UP] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_walk[(int)AnimDirection.UP], typeof(AnimationClip));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Down");
            m_selectedUnit.m_walk[(int)AnimDirection.DOWN] = (AnimationClip)EditorGUILayout.ObjectField(m_selectedUnit.m_walk[(int)AnimDirection.DOWN], typeof(AnimationClip));
            GUILayout.EndHorizontal();
        }

        void UnitsGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            m_unitSearchFilter = GUILayout.TextField(m_unitSearchFilter, EditorStyles.toolbarSearchField);
            //elements
            m_unitScroll = GUILayout.BeginScrollView(m_unitScroll, EditorStyles.helpBox);
            GUIStyle elemStyle = new GUIStyle(GUI.skin.button);
            elemStyle.alignment = TextAnchor.MiddleLeft;
            foreach (CharacterSheet current in m_database.m_characters)
            {
                if (current.m_name.Contains(m_unitSearchFilter) && GUILayout.Button(current.m_name, elemStyle))
                {
                    GUI.FocusControl(null);
                    m_selectedUnit = current;
                }
            }
            GUILayout.EndScrollView();
            //add new element
            if (GUILayout.Button("Add new"))
            {
                string tmpName = "New unit";
                int tmpCount = 1;
                while (HaveUnit(tmpName))
                {
                    tmpName = "New unit(" + tmpCount + ")";
                    ++tmpCount;
                }
                CharacterSheet tmpUnit = new CharacterSheet();
                tmpUnit.m_name = tmpName;
                m_database.m_characters.Add(tmpUnit);
                m_selectedUnit = tmpUnit;
                SortDatabase();
                SaveDatabase();
            }
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            if (m_selectedUnit != null)
            {
                m_currentUnitTab = GUILayout.Toolbar(m_currentUnitTab, m_unitTabs);
                switch (m_currentUnitTab)
                {
                    case 0:
                        UnitsCharacterSheetGUI();
                        break;
                    case 1:
                        UnitsAnimationsGUI();
                        break;
                    default:
                        break;
                }
                //remove element
                if (GUILayout.Button("Remove"))
                {
                    m_database.m_characters.Remove(m_selectedUnit);
                    m_selectedUnit = null;
                    SortDatabase();
                    SaveDatabase();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        bool HaveItem(string name)
        {
            foreach (Item current in m_database.m_items)
            {
                if (current.m_name == name)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsWeapon(string name)
        {
            foreach (Item current in m_database.m_items)
            {
                if (current.m_name == name)
                {
                    return current.m_itemType == ItemType.WEAPON;
                }
            }
            return false;
        }

        void ItemsGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            m_itemSearchFilter = GUILayout.TextField(m_itemSearchFilter, EditorStyles.toolbarSearchField);
            //elements
            m_itemScroll = GUILayout.BeginScrollView(m_itemScroll, EditorStyles.helpBox);
            GUIStyle elemStyle = new GUIStyle(GUI.skin.button);
            elemStyle.alignment = TextAnchor.MiddleLeft;
            foreach (Item current in m_database.m_items)
            {
                if (current.m_name.Contains(m_itemSearchFilter) && GUILayout.Button(current.m_name, elemStyle))
                {
                    GUI.FocusControl(null);
                    m_selectedItem = current;
                }
            }
            GUILayout.EndScrollView();
            //add new element
            if (GUILayout.Button("Add new"))
            {
                string tmpName = "New item";
                int tmpCount = 1;
                while (HaveItem(tmpName))
                {
                    tmpName = "New item(" + tmpCount + ")";
                    ++tmpCount;
                }
                Item tmpItem = new Item();
                tmpItem.m_name = tmpName;
                m_database.m_items.Add(tmpItem);
                m_selectedItem = tmpItem;
                SortDatabase();
                SaveDatabase();
            }
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            if (m_selectedItem != null)
            {
                //Id
                GUILayout.Label("-----Item");
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Name");
                string tmpName = GUILayout.TextField(m_selectedItem.m_name);
                if (!HaveItem(tmpName))
                {
                    m_selectedItem.m_name = tmpName;
                    SortDatabase();
                }
                GUILayout.EndHorizontal();
                //Item type
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Item type");
                m_selectedItem.m_itemType = (ItemType)EditorGUILayout.EnumPopup(m_selectedItem.m_itemType);
                GUILayout.EndHorizontal();
                switch(m_selectedItem.m_itemType)
                {
                    case ItemType.CONSUMABLE:
                        {
                            //Life regain
                            GUILayout.Label("");
                            GUILayout.Label("-----Consumable");
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Life regain");
                            m_selectedItem.m_life = EditorGUILayout.IntField(m_selectedItem.m_life);
                            GUILayout.EndHorizontal();
                            //Mana regain
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Mana regain");
                            m_selectedItem.m_mana = EditorGUILayout.IntField(m_selectedItem.m_mana);
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case ItemType.WEAPON:
                        {
                            //Weapon type
                            GUILayout.Label("");
                            GUILayout.Label("-----Weapon");
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Weapon type");
                            m_selectedItem.m_weaponType = (WeaponType)EditorGUILayout.EnumPopup(m_selectedItem.m_weaponType);
                            GUILayout.EndHorizontal();
                            //Weapon range
                            GUILayout.Label("");
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Weapon range");
                            m_selectedItem.m_range = EditorGUILayout.IntField(m_selectedItem.m_range);
                            GUILayout.EndHorizontal();
                            //Damages
                            GUILayout.Label("");
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Damages");
                            m_selectedItem.m_str = EditorGUILayout.IntField(m_selectedItem.m_str);
                            GUILayout.EndHorizontal();
                            //Magic damages
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Magic damages");
                            m_selectedItem.m_magStr = EditorGUILayout.IntField(m_selectedItem.m_magStr);
                            GUILayout.EndHorizontal();
                        }
                        break;
                    default:
                        break;
                }
                //remove element
                if (GUILayout.Button("Remove"))
                {
                    m_database.m_items.Remove(m_selectedItem);
                    m_selectedItem = null;
                    SortDatabase();
                    SaveDatabase();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void InventoryGUI()
        {
            GUILayout.Label("-----Inventory");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                m_database.m_baseInventory.m_itemsNames.Add("");
                m_database.m_baseInventory.m_itemsCount.Add(1);
            }
            if (GUILayout.Button("-") && m_database.m_baseInventory.m_itemsNames.Count > 0)
            {
                int pos = m_database.m_baseInventory.m_itemsNames.Count - 1;
                m_database.m_baseInventory.m_itemsNames.RemoveAt(pos);
                m_database.m_baseInventory.m_itemsCount.RemoveAt(pos);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginScrollView(Vector2.zero, EditorStyles.helpBox);
            for (int i = 0; i < m_database.m_baseInventory.m_itemsNames.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUI.contentColor = HaveItem(m_database.m_baseInventory.m_itemsNames[i]) ? Color.green : Color.red;
                EditorGUILayout.PrefixLabel("Item");
                m_database.m_baseInventory.m_itemsNames[i] = GUILayout.TextField(m_database.m_baseInventory.m_itemsNames[i]);
                GUI.contentColor = Color.white;
                EditorGUILayout.PrefixLabel("Count");
                int tmpCount = EditorGUILayout.IntField(m_database.m_baseInventory.m_itemsCount[i]);
                if (tmpCount > 0)
                {
                    m_database.m_baseInventory.m_itemsCount[i] = tmpCount;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
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
                    tmpName = "New skill(" + tmpCount + ")";
                    ++tmpCount;
                }
                Skill tmpSkill = new Skill();
                tmpSkill.m_name = tmpName;
                m_database.m_skills.Add(tmpSkill);
                m_selectedSkill = tmpSkill;
                SortDatabase();
                SaveDatabase();
            }
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            if (m_selectedSkill != null)
            {
                //id
                GUILayout.Label("-----Skill");
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Name");
                string tmpName = GUILayout.TextField(m_selectedSkill.m_name);
                if (!HaveSkill(tmpName))
                {
                    m_selectedSkill.m_name = tmpName;
                    SortDatabase();
                }
                GUILayout.EndHorizontal();
                //icon
                /*GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Icon");
                m_selectedSkill.m_icon = EditorGUILayout.ObjectField(m_selectedSkill.m_icon, typeof(Sprite)) as Sprite;
                GUILayout.EndHorizontal();*/
                //animation
                /*GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Animation");
                m_selectedSkill.m_animation = EditorGUILayout.ObjectField(m_selectedSkill.m_animation, typeof(Animation)) as Animation;
                GUILayout.EndHorizontal();*/
                //target
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Target");
                m_selectedSkill.m_target = (TargetType)EditorGUILayout.EnumPopup(m_selectedSkill.m_target);
                GUILayout.EndHorizontal();
                //weapon
                GUILayout.Label("");
                GUILayout.Label("-----Conditions");
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Weapon type");
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
                //range
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Range");
                m_selectedSkill.m_range = EditorGUILayout.IntField(m_selectedSkill.m_range);
                GUILayout.EndHorizontal();
                //life
                GUILayout.Label("");
                GUILayout.Label("-----Effects");
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Life regain");
                m_selectedSkill.m_life = EditorGUILayout.IntField(m_selectedSkill.m_life);
                GUILayout.EndHorizontal();
                //strength
                GUILayout.Label("");
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Strength");
                m_selectedSkill.m_str = EditorGUILayout.IntField(m_selectedSkill.m_str);
                GUILayout.EndHorizontal();
                //magic strength
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Magic strength");
                m_selectedSkill.m_magStr = EditorGUILayout.IntField(m_selectedSkill.m_magStr);
                GUILayout.EndHorizontal();
                //remove element
                if (GUILayout.Button("Remove"))
                {
                    m_database.m_skills.Remove(m_selectedSkill);
                    m_selectedSkill = null;
                    SortDatabase();
                    SaveDatabase();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        bool HaveTeam(string name)
        {
            foreach (Team current in m_database.m_teams)
            {
                if (current.m_name == name)
                {
                    return true;
                }
            }
            return false;
        }

        void TeamsGUI()
        {
            GUILayout.BeginHorizontal();
            //left panel
            GUIStyle searchStyle = new GUIStyle();
            searchStyle.fixedWidth = 256.0f;
            GUILayout.BeginVertical(searchStyle);
            //search
            m_teamSearchFilter = GUILayout.TextField(m_teamSearchFilter, EditorStyles.toolbarSearchField);
            //elements
            m_teamScroll = GUILayout.BeginScrollView(m_teamScroll, EditorStyles.helpBox);
            GUIStyle elemStyle = new GUIStyle(GUI.skin.button);
            elemStyle.alignment = TextAnchor.MiddleLeft;
            foreach (Team current in m_database.m_teams)
            {
                if (current.m_name.Contains(m_teamSearchFilter) && GUILayout.Button(current.m_name, elemStyle))
                {
                    GUI.FocusControl(null);
                    m_selectedTeam = current;
                }
            }
            GUILayout.EndScrollView();
            //add new element
            if (GUILayout.Button("Add new"))
            {
                string tmpName = "New team";
                int tmpCount = 1;
                while (HaveTeam(tmpName))
                {
                    tmpName = "New team(" + tmpCount + ")";
                    ++tmpCount;
                }
                Team tmpTeam = new Team();
                tmpTeam.m_name = tmpName;
                tmpTeam.m_unitsNames.Add("");
                m_database.m_teams.Add(tmpTeam);
                m_selectedTeam = tmpTeam;
                SortDatabase();
                SaveDatabase();
            }
            GUILayout.EndVertical();
            //right panel
            GUILayout.BeginVertical();
            if (m_selectedTeam != null)
            {
                //Id
                GUILayout.Label("-----Team");
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Name");
                string tmpName = GUILayout.TextField(m_selectedTeam.m_name);
                if (!HaveTeam(tmpName))
                {
                    m_selectedTeam.m_name = tmpName;
                    SortDatabase();
                }
                GUILayout.TextField("");
                GUILayout.EndHorizontal();
                GUILayout.Label("");
                GUILayout.Label("-----Units (9 max)");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+") && m_selectedTeam.m_unitsNames.Count < 9)
                {
                    m_selectedTeam.m_unitsNames.Add("");
                }
                if (GUILayout.Button("-") && m_selectedTeam.m_unitsNames.Count > 1)
                {
                    m_selectedTeam.m_unitsNames.RemoveAt(m_selectedTeam.m_unitsNames.Count - 1);
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginScrollView(Vector2.zero, EditorStyles.helpBox);
                for (int i = 0; i < m_selectedTeam.m_unitsNames.Count; ++i)
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = HaveUnit(m_selectedTeam.m_unitsNames[i]) ? Color.green : Color.red;
                    EditorGUILayout.PrefixLabel("Character");
                    m_selectedTeam.m_unitsNames[i] = GUILayout.TextField(m_selectedTeam.m_unitsNames[i]);
                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
                //remove element
                if (GUILayout.Button("Remove"))
                {
                    m_database.m_teams.Remove(m_selectedTeam);
                    m_selectedTeam = null;
                    SortDatabase();
                    SaveDatabase();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            SaveDatabase();
        }
    }
}