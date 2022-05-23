using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class BattleSystemWalk : StateMachineBehaviour
    {
        BattleSystem m_battleSystem = null;
        UnitVisual m_visual = null;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_battleSystem = animator.gameObject.GetComponent<BattleSystem>();
            m_visual = m_battleSystem.GetCurrentTurnUnitVisual();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!m_visual.CanMove() || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Backspace))
            {
                animator.SetTrigger("Choice");
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
            {
                if (m_visual.CanWalkTo(Vector2.up))
                {
                    m_visual.ComputeNextMove(Vector2.up);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
            {
                if (m_visual.CanWalkTo(Vector2.left))
                {
                    m_visual.ComputeNextMove(Vector2.left);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (m_visual.CanWalkTo(Vector2.down))
                {
                    m_visual.ComputeNextMove(Vector2.down);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (m_visual.CanWalkTo(Vector2.right))
                {
                    m_visual.ComputeNextMove(Vector2.right);
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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
    }
}