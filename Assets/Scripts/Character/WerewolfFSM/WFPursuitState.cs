using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFPursuitState : StateMachineBehaviour
{
    private Character m_character;
    private Transform m_target;

    [SerializeField] private float m_catchDistance = 1.0f;
    
    [SerializeField] private float m_minSpeed = 3.0f;
    [SerializeField] private float m_maxSpeed = 5.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_character = animator.GetComponent<Character>();
        
        m_target = m_character.npcDetector.nearestNPC.transform;
        m_character.StartPursuit();
        
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 direction = m_character.direction;
        float tilt = direction.magnitude;
        direction.Normalize();

        float ratio = Mathf.Clamp((1.0f + Vector3.Dot(m_character.getVelocity().normalized, m_character.direction.normalized)), 0.0f, 1.0f); 
        m_character.aiPath.maxSpeed = ratio * (m_maxSpeed - m_minSpeed) + m_minSpeed;
        
        m_character.UpdatePursuit();

        if (m_target != null && Vector3.Distance(m_target.position, animator.transform.position) < m_catchDistance)
        {
            m_character.ExitWerewolf(true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_character.StopPursuit();
    }
}
