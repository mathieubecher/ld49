using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDetector : MonoBehaviour
{
    private const float TOLERANCE = 0.001f;
    
    [SerializeField] private float m_maxDistanceDetectedPlayer = 5.0f;
    [SerializeField] private NPC m_npcOwner;
    
    public Character character;
    
    [Header("Debug")]
    [SerializeField] public bool m_detectPlayer = false;
    [SerializeField] public float m_lostPlayerTimer = 0.0f;
    [SerializeField] private List<NPC> m_npcs;
    
    void Start()
    {
        character = FindObjectOfType<Character>();
        m_npcs = new List<NPC>();
    }

    void Update()
    {
        // Player
        Vector3 playerDirection = character.transform.position - transform.position;
        float playerDistance = playerDirection.magnitude;
        playerDirection.Normalize();
        
        bool detect = false;
        if ((character.isWerewolf || Math.Abs(Mathf.Sign(playerDirection.x) - Mathf.Sign(transform.parent.localScale.x)) < TOLERANCE)
            &&  playerDistance <= m_maxDistanceDetectedPlayer)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, m_maxDistanceDetectedPlayer, LayerMask.GetMask("Obstacle", "Player"));

            if (hit.collider != null && hit.collider.gameObject == character.gameObject)
            {
                detect = true;
                Debug.DrawLine(transform.position, hit.point, Color.green);
            }
            else
            {
                Debug.DrawLine(transform.position, hit.collider != null? (Vector3)hit.point : character.transform.position, Color.red);
            }
        }
        
        if(detect)
        {
            m_detectPlayer = true;
            m_lostPlayerTimer = 0.0f;
        }
        else if(m_detectPlayer)
        {
            m_lostPlayerTimer += Time.deltaTime;
            m_detectPlayer = m_lostPlayerTimer > m_maxDistanceDetectedPlayer;
        }
        
        // NPC
        for (int i = m_npcs.Count - 1; i >= 0; --i)
        {
            if(m_npcs[i] == null)
                m_npcs.RemoveAt(i);
        }
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("NPC") || _other.gameObject == transform.parent.gameObject)
            return;
        
        NPC npc;
        if (_other.gameObject.TryGetComponent(out npc))
        {
            m_npcs.Add(npc);
        }
        
    }
    void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("NPC") || _other.gameObject == transform.parent.gameObject)
            return;
        
        NPC npc;
        if (_other.gameObject.TryGetComponent(out npc))
        {
            m_npcs.Remove(npc);
        }
        
    }
}
