using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class BattleSystemEquip : StateMachineBehaviour
    {
        BattleSystem m_battleSystem = null;
        List<string> m_weaponList = new List<string>();
        List<GameObject> m_weaponNames = new List<GameObject>();
        Vector3 m_startPos = new Vector3(0.0f, 0.0f, 0.0f);
        int m_cursor = 0;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_battleSystem = animator.gameObject.GetComponent<BattleSystem>();
            RectTransform rectTransform = m_battleSystem.m_dynamicPanel.GetComponent<RectTransform>();
            m_weaponList = m_battleSystem.m_inventory.m_weaponNames;
            m_startPos = m_battleSystem.m_dynamicCursor.localPosition;
            m_startPos += new Vector3(0.0f, 32.0f * m_weaponList.Count / 2.0f - 16.0f, 0.0f);
            m_battleSystem.m_dynamicCursor.localPosition = m_startPos;
            rectTransform.sizeDelta = new Vector2(256.0f, m_weaponList.Count * 32.0f);
            for (int i = 0; i < m_weaponList.Count; i++)
            {
                GameObject dynamicText = Instantiate(m_battleSystem.m_dynamicTextPrefab, m_battleSystem.m_dynamicPanel.transform);
                dynamicText.GetComponent<Text>().text = m_weaponList[i] + " : " + m_battleSystem.m_inventory.GetWeaponCount(m_weaponList[i]);
                dynamicText.GetComponent<RectTransform>().localPosition = new Vector2(0.0f, 32.0f * (m_weaponList.Count / 2.0f - i) - 16.0f);
                m_weaponNames.Add(dynamicText);
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
            foreach (GameObject current in m_weaponNames)
            {
                Destroy(current);
            }
            m_weaponNames.Clear();
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
                m_cursor = m_weaponList.Count - 1;
            }
            if (m_cursor >= m_weaponList.Count)
            {
                m_cursor = 0;
            }
            m_battleSystem.m_dynamicCursor.localPosition = m_startPos + m_cursor * new Vector3(0.0f, -32.0f, 0.0f);
        }

        void SelectTarget()
        {
            LogManager.GetSingleton().Log(m_battleSystem.m_currentUnit.m_name + " equip " + m_weaponList[m_cursor]);
            if (m_battleSystem.m_currentUnit.m_weapon)
            {
                m_battleSystem.m_inventory.AddWeapon(Weapon.CreateCopy(m_battleSystem.m_currentUnit.m_weapon));
            }
            m_battleSystem.m_currentUnit.m_weapon = m_battleSystem.m_inventory.RemoveWeapon(m_weaponList[m_cursor]);
        }
    }
}