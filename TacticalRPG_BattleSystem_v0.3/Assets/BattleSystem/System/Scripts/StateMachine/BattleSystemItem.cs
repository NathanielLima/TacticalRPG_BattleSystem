using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class BattleSystemItem : StateMachineBehaviour
    {
        BattleSystem m_battleSystem = null;
        UnitVisual m_unitVisual = null;
        List<string> m_itemList = new List<string>();
        List<GameObject> m_itemNames = new List<GameObject>();
        Vector3 m_startPos = new Vector3(0.0f, 0.0f, 0.0f);
        int m_cursor = 0;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_battleSystem = animator.gameObject.GetComponent<BattleSystem>();
            m_unitVisual = m_battleSystem.GetCurrentTurnUnitVisual();
            RectTransform rectTransform = m_battleSystem.m_dynamicPanel.GetComponent<RectTransform>();
            m_itemList = m_battleSystem.m_inventory.m_itemNames;
            m_startPos = m_battleSystem.m_dynamicCursor.localPosition;
            m_startPos += new Vector3(0.0f, 32.0f * m_itemList.Count / 2.0f - 16.0f, 0.0f);
            m_battleSystem.m_dynamicCursor.localPosition = m_startPos;
            rectTransform.sizeDelta = new Vector2(256.0f, m_itemList.Count * 32.0f);
            for (int i = 0; i < m_itemList.Count; i++)
            {
                GameObject dynamicText = Instantiate(m_battleSystem.m_dynamicTextPrefab, m_battleSystem.m_dynamicPanel.transform);
                dynamicText.GetComponent<Text>().text = m_itemList[i] + " : " + m_battleSystem.m_inventory.GetItemCount(m_itemList[i]);
                dynamicText.GetComponent<RectTransform>().localPosition = new Vector2(0.0f, 32.0f * (m_itemList.Count / 2.0f - i) - 16.0f);
                m_itemNames.Add(dynamicText);
            }
            m_battleSystem.m_dynamicPanel.SetActive(true);
            m_cursor = 0;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Backspace))
            {
                animator.SetTrigger("Choice");
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
            {
                MoveCursorPos(-1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                MoveCursorPos(1);
            }
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                SelectTarget();
                animator.SetTrigger("EndTurn");
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_battleSystem.m_dynamicPanel.SetActive(false);
            m_startPos = m_battleSystem.m_dynamicCursor.localPosition = new Vector2(0.0f, 0.0f);
            foreach (GameObject current in m_itemNames)
            {
                Destroy(current);
            }
            m_itemNames.Clear();
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}

        void MoveCursorPos(int _move)
        {
            m_cursor += _move;
            if (m_cursor < 0)
            {
                m_cursor = m_itemList.Count - 1;
            }
            if (m_cursor >= m_itemList.Count)
            {
                m_cursor = 0;
            }
            m_battleSystem.m_dynamicCursor.localPosition = m_startPos + m_cursor * new Vector3(0.0f, -32.0f, 0.0f);
        }

        void SelectTarget()
        {
            LogManager.GetSingleton().Log(m_battleSystem.m_currentUnit.m_name + " use " + m_itemList[m_cursor]);
            Item item = m_battleSystem.m_inventory.RemoveItem(m_itemList[m_cursor]);
            m_battleSystem.m_currentUnit.HealLife(item.m_life);
            if (item.m_life > 0)
            {
                EffectManager.GetSingleton().CreateNumberEffect(item.m_life, Color.green, m_unitVisual.transform.position);
            }
            m_battleSystem.m_currentUnit.HealMana(item.m_mana);
            if (item.m_mana > 0)
            {
                EffectManager.GetSingleton().CreateNumberEffect(item.m_mana, Color.blue, m_unitVisual.transform.position);
            }
            if (m_battleSystem.m_currentUnit.Act(ActionType.USE))
            {
                m_battleSystem.UpdateCurrentTurnUnitStats();
            }
        }
    }
}