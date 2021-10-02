using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFIdleState : StateMachineBehaviour
{
    private Character m_character;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_character = animator.GetComponent<Character>();    
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 direction = m_character.direction;
        float tilt = direction.magnitude;
        direction.Normalize();
        
        m_character.SetVelocity(Vector3.zero);
        
        animator.SetFloat("tilt", tilt);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
