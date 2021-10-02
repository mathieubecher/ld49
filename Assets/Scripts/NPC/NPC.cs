using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private const float DEADZONE = 0.1f;

    private Rigidbody2D m_rigidbody;
    private Animator m_FSM;
    private AIDestinationSetter m_destinationSetter;
    private AIPath m_aiPath;

    [SerializeField] private CharacterDetector m_detector;
    [SerializeField] private Animator m_animator;
    
    [HideInInspector] public bool alerted = false;
    [HideInInspector] public bool reconize = false;
    [HideInInspector] public bool armed = false;

    public bool attack => armed && m_detector.m_detectPlayer && (reconize || m_detector.character.isWerewolf);
    public bool run => !armed && m_detector.m_detectPlayer && m_detector.character.isWerewolf;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_FSM = GetComponent<Animator>();
        
        m_aiPath = GetComponent<AIPath>();
        m_destinationSetter = GetComponent<AIDestinationSetter>();
        m_aiPath.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = m_aiPath.enabled ? m_aiPath.velocity : (Vector3)m_rigidbody.velocity;
        if (Mathf.Abs(velocity.x) > DEADZONE)
        {
            m_animator.gameObject.transform.localScale = new Vector3(Mathf.Sign(m_rigidbody.velocity.x),1.0f,1.0f);
        }

        
        m_FSM.SetBool("attack", attack);
        m_FSM.SetBool("run", run);
    }
    
    public Vector3 getVelocity()
    {
        return (m_aiPath.enabled)? m_aiPath.velocity : (Vector3) m_rigidbody.velocity;
    }
    public void SetVelocity(Vector3 _velocity)
    {
        m_rigidbody.velocity = _velocity;
    }

    public Character GetCharacter()
    {
        return m_detector.character;
    }
}
