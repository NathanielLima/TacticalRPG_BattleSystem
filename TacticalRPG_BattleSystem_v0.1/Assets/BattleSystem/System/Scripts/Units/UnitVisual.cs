using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class UnitVisual : MonoBehaviour
    {
        [NonSerialized] public string m_name = "";
        Vector2 m_pos = new Vector2(0.0f, 0.0f);
        Vector2 m_newPos = new Vector2(0.0f, 0.0f);
        int m_steps = 3;
        float m_currentTime = 0.0f;
        float m_maxMoveTime = 0.5f;

        [NonSerialized] public BattleSystem m_battleSystem = null;

        // Start is called before the first frame update
        void Start()
        {
            SetStartPosition(m_pos);
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePos();
        }

        public void SetStartPosition(Vector2 _position)
        {
            m_pos = _position;
            m_newPos = _position;
            transform.position = _position;
        }

        public void SetActive(bool _isActive)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (_isActive)
            {
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
        }

        public void ResetSteps()
        {
            m_steps = 3;
        }

        public bool CanMove()
        {
            return m_steps > 0;
        }

        public bool CanWalkTo(Vector2 _direction)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 newPos = position + _direction;
            if ((int)newPos.x + 4 < 0 || (int)newPos.x + 4 >= 9 || (int)newPos.y + 4 < 0 || (int)newPos.y + 4 >= 9)
            {
                return false;
            }
            return !m_battleSystem.m_zone[(int)newPos.y + 4, (int)newPos.x + 4];
        }

        public void ComputeNextMove(Vector2 _direction)
        {
            if (transform.position.x == m_newPos.x && transform.position.y == m_newPos.y)
            {
                Vector2 position = new Vector2(transform.position.x, transform.position.y);
                m_pos = position;
                m_newPos = position + _direction;
                m_battleSystem.m_zone[(int)m_pos.y + 4, (int)m_pos.x + 4] = false;
                m_currentTime = 0.0f;
                --m_steps;
                LogManager.GetSingleton().Log(m_name + " moved");
                if (m_battleSystem.m_currentUnit.Act(ActionType.MOVE))
                {
                    m_battleSystem.UpdateCurrentTurnUnitStats();
                    EffectManager.GetSingleton().CreateTextEffect("SP", new Color(214 / 255.0f, 150 / 255.0f, 49 / 255.0f, 1.0f), transform.position);
                }
            }
        }

        void UpdatePos()
        {
            m_currentTime += Time.deltaTime;
            transform.position = Vector2.Lerp(m_pos, m_newPos, m_currentTime / m_maxMoveTime);
            if (m_currentTime >= m_maxMoveTime)
            {
                m_pos = m_newPos;
                transform.position = m_newPos;
                m_battleSystem.m_zone[(int)m_newPos.y + 4, (int)m_newPos.x + 4] = true;
            }
        }

        float GetDistTo(UnitVisual _target)
        {
            return Mathf.Abs(_target.m_pos.x - m_pos.x) + Mathf.Abs(_target.m_pos.y - m_pos.y);
        }

        public bool CanAttack()
        {
            UnitVisual[] visuals = FindObjectsOfType<UnitVisual>();
            for (int i = 0; i < visuals.Length; i++)
            {
                if (visuals[i] != this && m_battleSystem.IsUnitAlive(visuals[i].m_name))
                {
                    /*if (m_pos + Vector2.up == visuals[i].m_pos || m_pos + Vector2.left == visuals[i].m_pos 
                        || m_pos + Vector2.down == visuals[i].m_pos || m_pos + Vector2.right == visuals[i].m_pos)*/
                    if (GetDistTo(visuals[i]) <= m_battleSystem.m_currentUnit.m_weapon.m_range)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<string> GetAttackTargetList()
        {
            List<string> result = new List<string>();
            UnitVisual[] visuals = FindObjectsOfType<UnitVisual>();
            for (int i = 0; i< visuals.Length; i++)
            {
                if (visuals[i] != this && m_battleSystem.IsUnitAlive(visuals[i].m_name))
                {
                    /*if (m_pos + Vector2.up == visuals[i].m_pos || m_pos + Vector2.left == visuals[i].m_pos
                        || m_pos + Vector2.down == visuals[i].m_pos || m_pos + Vector2.right == visuals[i].m_pos)*/
                    if (GetDistTo(visuals[i]) <= m_battleSystem.m_currentUnit.m_weapon.m_range)
                    {
                        result.Add(visuals[i].m_name);
                    }
                }
            }
            return result;
        }
    }
}
