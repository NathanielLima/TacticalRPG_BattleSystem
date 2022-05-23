using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class BattleSystemAttack : StateMachineBehaviour
    {
        BattleSystem m_battleSystem = null;
        UnitVisual m_visual = null;
        List<string> m_targetList = new List<string>();
        List<GameObject> m_targetNames = new List<GameObject>();
        Vector3 m_startPos = new Vector3(0.0f, 0.0f, 0.0f);
        int m_cursor = 0;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_battleSystem = animator.gameObject.GetComponent<BattleSystem>();
            m_visual = m_battleSystem.GetCurrentTurnUnitVisual();
            RectTransform rectTransform = m_battleSystem.m_dynamicPanel.GetComponent<RectTransform>();
            m_targetList = m_visual.GetAttackTargetList();
            m_startPos = m_battleSystem.m_dynamicCursor.localPosition;
            m_startPos += new Vector3(0.0f, 32.0f * m_targetList.Count / 2.0f - 16.0f, 0.0f);
            m_battleSystem.m_dynamicCursor.localPosition = m_startPos;
            rectTransform.sizeDelta = new Vector2(256.0f, m_targetList.Count * 32.0f);
            for (int i = 0; i < m_targetList.Count; i++)
            {
                GameObject dynamicText = Instantiate(m_battleSystem.m_dynamicTextPrefab, m_battleSystem.m_dynamicPanel.transform);
                dynamicText.GetComponent<Text>().text = m_targetList[i];
                dynamicText.GetComponent<RectTransform>().localPosition = new Vector2(0.0f, 32.0f * (m_targetList.Count / 2.0f - i) - 16.0f);
                m_targetNames.Add(dynamicText);
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
            foreach(GameObject current in m_targetNames)
            {
                Destroy(current);
            }
            m_targetNames.Clear();
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
                m_cursor = m_targetList.Count - 1;
            }
            if (m_cursor >= m_targetList.Count)
            {
                m_cursor = 0;
            }
            m_battleSystem.m_dynamicCursor.localPosition = m_startPos + m_cursor * new Vector3(0.0f, -32.0f, 0.0f);
        }

        void SelectTarget()
        {
            LogManager.GetSingleton().Log(m_battleSystem.m_currentUnit.m_name + " attack " + m_targetList[m_cursor]);
            if (m_battleSystem.m_currentUnit.Act(ActionType.ATTACK))
            {
                m_battleSystem.UpdateCurrentTurnUnitStats();
            }
            m_battleSystem.CurrentTurnUnitFace(m_targetList[m_cursor]);
            m_battleSystem.AttackOnUnit(m_targetList[m_cursor], ActionType.HIT, m_battleSystem.m_currentUnit.GetStat(StatName.STRENGTH));
        }
    }
}