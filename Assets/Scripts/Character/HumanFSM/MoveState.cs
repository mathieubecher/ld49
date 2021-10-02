using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : StateMachineBehaviour
{
    private Character m_character;
    [SerializeField] private float m_walkSpeed = 3.0f;
    [SerializeField] private float m_runSpeed = 5.0f;
    [SerializeField] private AnimationCurve m_lifeDeccel;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_character = animator.GetComponent<Character>();    
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 direction = m_character.direction;
        float tilt = direction.magnitude;
        direction.Normalize();

        Vector3 velocity = direction * (m_character.isRunning ? m_runSpeed : m_walkSpeed);
        if (!m_character.isRunning)
        {
            velocity *= m_lifeDeccel.Evaluate(m_character.lightDetection.life);
        }
                    
        m_character.SetVelocity(velocity);
        
        animator.SetFloat("tilt", tilt);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

}
