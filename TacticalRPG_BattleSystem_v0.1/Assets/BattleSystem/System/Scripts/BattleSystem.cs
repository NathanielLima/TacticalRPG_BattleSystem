using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] Team m_playerTeam = null;
        [SerializeField] Team m_enemyTeam = null;
        public Inventory m_inventory = null;
        [SerializeField] GameObject m_visualPrefab = null;

        [Header("UI - Choice")]
        public GameObject m_choice = null;
        public RectTransform m_cursor = null;

        [Header("UI - Unit Data")]
        public GameObject m_unitData = null;
        public Text m_name = null;
        public Text m_weapon = null;
        public Slider m_life = null;
        public Slider m_mana = null;
        public List<Text> m_stats = new List<Text>();

        [Header("UI - Dynamic Panel")]
        public GameObject m_dynamicPanel = null;
        public RectTransform m_dynamicCursor = null;
        public GameObject m_dynamicTextPrefab = null;

        [NonSerialized] public Team m_allUnits = null;
        Dictionary<string, UnitVisual> m_visuals = new Dictionary<string, UnitVisual>();

        [NonSerialized] public Unit m_currentUnit = null;

        [NonSerialized] public bool[,] m_zone = new bool[9, 9];

        // Start is called before the first frame update
        void Start()
        {
            InitZone();
            GatherAllUnits();
            InitAllUnits();
            CreateVisuals();
            m_inventory.SetBaseInventory();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void InitZone()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    m_zone[y, x] = false;
                }
            }
        }

        void GatherAllUnits()
        {
            m_allUnits = ScriptableObject.CreateInstance<Team>();
            foreach(Unit current in m_playerTeam.m_units)
            {
                //m_allUnits.AddUnit(Unit.CreateCopy(current));
                m_allUnits.AddUnit(current);
            }
            foreach(Unit current in m_enemyTeam.m_units)
            {
                //m_allUnits.AddUnit(Unit.CreateCopy(current));
                m_allUnits.AddUnit(current);
            }
        }

        void InitAllUnits()
        {
            foreach(Unit current in m_allUnits.m_units)
            {
                current.InitMastery();
                current.SetBaseStats();
            }
        }

        void CreateVisuals()
        {
            Vector2 allyPos = new Vector2(-m_playerTeam.m_units.Count / 2, -2);
            Vector2 enemyPos = new Vector2(-m_enemyTeam.m_units.Count / 2, 2);
            int countAlly = 0;
            int countEnemy = 0;
            foreach(Unit current in m_allUnits.m_units)
            {
                UnitVisual visual = Instantiate(m_visualPrefab).GetComponent<UnitVisual>();
                Vector2 currentPos = new Vector2(0.0f, 0.0f);
                if (m_playerTeam.m_units.Contains(current))
                //if (m_allUnits.m_units.IndexOf(current) < m_playerTeam.m_units.Count)
                {
                    currentPos = allyPos + countAlly * Vector2.right;
                    visual.SetStartPosition(currentPos);
                    ++countAlly;
                }
                else
                {
                    currentPos = enemyPos + countEnemy * Vector2.right;
                    visual.SetStartPosition(currentPos);
                    ++countEnemy;
                }
                m_zone[(int)currentPos.y + 4, (int)currentPos.x + 4] = true;
                visual.m_name = current.m_name;
                visual.m_battleSystem = this;
                visual.SetActive(false);
                m_visuals.Add(current.name, visual);
            }
        }

        public void ComputeCurrentTurnUnit()
        {
            if (m_currentUnit)
            {
                GetCurrentTurnUnitVisual().SetActive(false);
            }
            m_currentUnit = m_allUnits.GetCurrentTurnUnit();
            GetCurrentTurnUnitVisual().ResetSteps();
            GetCurrentTurnUnitVisual().SetActive(true);
        }

        public UnitVisual GetCurrentTurnUnitVisual()
        {
            return m_visuals[m_currentUnit.m_name];
        }

        public bool IsCurrentTurnUnitEnemy()
        {
            return m_enemyTeam.m_units.Contains(m_currentUnit);
        }

        public void UpdateCurrentTurnUnitStats()
        {
            List<int> stats = m_currentUnit.GetStats();
            m_name.text = m_currentUnit.m_name;
            if (m_currentUnit.m_weapon)
            {
                m_weapon.text = "Weapon : " + m_currentUnit.m_weapon.m_name;
            }
            else
            {
                m_weapon.text = "Weapon : None";
            }
            m_life.value = (float)m_currentUnit.GetCurrentLife() / m_currentUnit.GetMaxLife();
            if (m_currentUnit.GetMaxMana() == 0)
            {
                m_mana.value = 0.0f;
            }
            else
            {
                m_mana.value = (float)m_currentUnit.GetCurrentMana() / m_currentUnit.GetMaxMana();
            }
            for(int i = 0; i < m_stats.Count; i++)
            {
                m_stats[i].text = stats[i].ToString();
            }
        }

        public bool IsUnitAlive(string _name)
        {
            foreach(Unit current in m_allUnits.m_units)
            {
                if (current.m_name == _name)
                {
                    return current.IsAlive();
                }
            }
            return false;
        }

        public void AttackOnUnit(string _targetName, ActionType _hitType, int _str)
        {
            foreach(Unit current in m_allUnits.m_units)
            {
                if (current.m_name == _targetName)
                {
                    int display = _str;
                    current.Act(_hitType);
                    switch(_hitType)
                    {
                        case ActionType.HIT:
                            current.AddDamages(_str);
                            display -= current.GetDefense();
                            break;
                        case ActionType.MAGIC_HIT:
                            current.AddMagicDamages(_str);
                            display -= current.GetMagicDefense();
                            break;
                        default:
                            break;
                    }
                    EffectManager.GetSingleton().CreateNumberEffect(display < 0 ? 0 : display, Color.red, m_visuals[current.m_name].transform.position);
                    return;
                }
            }
        }

        public bool AreAlliesAlive()
        {
            foreach (Unit current in m_playerTeam.m_units)
            {
                if (current.IsAlive())
                {
                    return true;
                }
            }
            return false;
        }

        public bool AreEnemiesAlive()
        {
            foreach (Unit current in m_enemyTeam.m_units)
            {
                if (current.IsAlive())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
