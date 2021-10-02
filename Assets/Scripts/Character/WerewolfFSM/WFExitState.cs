using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFExitState : StateMachineBehaviour
{
    private Character m_character;
    [SerializeField] private float m_exitDuration = 1.0f;

    private float m_duration = 0.0f;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_duration = m_exitDuration;
        m_character = animator.GetComponent<Character>();   
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_character.SetVelocity(Vector3.zero);

        m_duration -= Time.deltaTime;
        if(m_duration <= 0.0f)
            m_character.ChangeToHuman();
           
    }
}
