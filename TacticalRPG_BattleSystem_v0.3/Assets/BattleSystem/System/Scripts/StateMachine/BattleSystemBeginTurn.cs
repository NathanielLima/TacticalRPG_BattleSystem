using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class BattleSystemBeginTurn : StateMachineBehaviour
    {
        BattleSystem m_battleSystem = null;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_battleSystem = animator.gameObject.GetComponent<BattleSystem>();
            m_battleSystem.ComputeCurrentTurnUnit();
            m_battleSystem.UpdateCurrentTurnUnitStats();
            if (m_battleSystem.m_currentUnit.IsAlive())
            {
                LogManager.GetSingleton().Log("Turn of " + m_battleSystem.m_currentUnit.m_name);
                animator.SetTrigger("Choice");
            }
            else
            {
                animator.SetTrigger("EndTurn");
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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