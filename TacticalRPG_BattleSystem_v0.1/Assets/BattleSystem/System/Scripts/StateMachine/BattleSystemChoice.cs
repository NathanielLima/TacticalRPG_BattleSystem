using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class BattleSystemChoice : StateMachineBehaviour
    {
        Animator m_animator = null;
        BattleSystem m_battleSystem = null;
        UnitVisual m_visual = null;
        Vector3 m_startPos = new Vector3(0.0f, 0.0f, 0.0f);
        int m_cursor = 0;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_animator = animator;
            m_battleSystem = animator.gameObject.GetComponent<BattleSystem>();
            m_battleSystem.m_choice.SetActive(true);
            m_visual = m_battleSystem.GetCurrentTurnUnitVisual();
            m_startPos = m_battleSystem.m_cursor.localPosition;
            m_cursor = 0;
            if (m_battleSystem.IsCurrentTurnUnitEnemy())
            {
                animator.SetTrigger("EndTurn");
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
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
                SelectOption();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_battleSystem.m_choice.SetActive(false);
            m_battleSystem.m_cursor.localPosition = m_startPos;
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
                m_cursor = 5;
            }
            if (m_cursor >= 6)
            {
                m_cursor = 0;
            }
            m_battleSystem.m_cursor.localPosition = m_startPos + m_cursor * new Vector3(0.0f, -32.0f, 0.0f);
        }

        void SelectOption()
        {
            switch((ActionType)m_cursor)
            {
                case ActionType.ATTACK:
                    if (m_visual.CanAttack())
                    {
                        m_animator.SetTrigger("Attack");
                    }
                    break;
                case ActionType.SKILL:
                    if (m_battleSystem.m_currentUnit.GetSkills().Count > 0)
                    {
                        m_animator.SetTrigger("Skill");
                    }
                    break;
                case ActionType.MOVE:
                    if (m_visual.CanMove())
                    {
                        m_animator.SetTrigger("Walk");
                    }
                    break;
                case ActionType.USE:
                    m_animator.SetTrigger("Item");
                    break;
                case ActionType.EQUIP:
                    m_animator.SetTrigger("Equip");
                    break;
                default:
                    m_animator.SetTrigger("EndTurn");
                    break;
            }
        }
    }
}