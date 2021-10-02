using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private const float DEADZONE = 0.1f;

    private Rigidbody2D m_rigidbody;
    private LightDetection m_lightDetection;
    private AIDestinationSetter m_destinationSetter;
    private AIPath m_aiPath;
    private Animator m_FSM;
    
    [SerializeField] private NPCDetector m_NPCDetector;
    
    [SerializeField] private AnimatorController m_humanFSM;
    [SerializeField] private AnimatorController m_werewolfFSM;
    
    [Header("Debug")]
    [SerializeField] private bool m_isRunning;
    [SerializeField] private Vector2 m_direction;

    [Header("Animation")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private AnimatorController m_humanAnimatorController;
    [SerializeField] private AnimatorController m_werewolfAnimatorController;
    
    //GETTER
    public Animator animator => m_animator;
    public AIPath aiPath => m_aiPath;
    public LightDetection lightDetection => m_lightDetection;
    public NPCDetector npcDetector => m_NPCDetector;
    public Vector2 direction => m_direction;
    public bool isRunning => m_isRunning;
    public bool isWerewolf => m_lightDetection.life <= 0.0f;
    
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_lightDetection = GetComponent<LightDetection>();
        m_FSM = GetComponent<Animator>();

        m_aiPath = GetComponent<AIPath>();
        m_destinationSetter = GetComponent<AIDestinationSetter>();
        m_aiPath.enabled = false;
    }

    void Update()
    {
        
        m_FSM.SetBool("hasTarget",npcDetector.FindNPC());
        SetAnimationInput();
    }

    private void SetAnimationInput()
    {
        Vector3 velocity = getVelocity();
        if (Mathf.Abs(velocity.x) > DEADZONE)
        {
            m_animator.gameObject.transform.localScale = new Vector3(Mathf.Sign(velocity.x),1.0f,1.0f);
        }
        
        m_animator.SetFloat("speed", velocity.magnitude);
    }

    public Vector3 getVelocity()
    {
        return (m_aiPath.enabled)? m_aiPath.velocity : (Vector3) m_rigidbody.velocity;
    }
    public void SetVelocity(Vector3 _velocity)
    {
        m_rigidbody.velocity = _velocity;
    }
    

    public void ReadDirection(InputAction.CallbackContext _context)
    {
        m_direction = _context.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext _context)
    {
        m_isRunning = _context.performed;
    }

    public void ChangeToWerewolf()
    {
        m_animator.runtimeAnimatorController = m_werewolfAnimatorController;
        m_FSM.runtimeAnimatorController = m_werewolfFSM;
    }

    public void ChangeToHuman()
    {
        m_lightDetection.ResetLife();
        m_animator.runtimeAnimatorController = m_humanAnimatorController;
        m_FSM.runtimeAnimatorController = m_humanFSM;
    }

    public void ExitWerewolf(bool _eat = false)
    {
        m_FSM.SetTrigger("Exit");

        if (_eat)
        {
            m_animator.SetTrigger("Eat");
            m_destinationSetter.target = null;
            Destroy(m_NPCDetector.nearestNPC.gameObject);
        }
        else m_animator.SetTrigger("Exit");

    }

    public void StartPursuit()
    {
        m_aiPath.enabled = UpdatePursuit();
    }
    
    public void StopPursuit()
    {
        m_aiPath.enabled = false;
        m_destinationSetter.target = null;
    }

    public bool UpdatePursuit()
    {
        var npc = m_NPCDetector.nearestNPC;
        if (npc == null) return false;
        
        m_destinationSetter.target = npc.transform;

        return true;
    }
}
