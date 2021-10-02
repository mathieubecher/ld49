using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCDetector : MonoBehaviour
{
    [SerializeField] private List<NPC> m_npcs;

    public NPC nearestNPC
    {
        get
        {
            if (m_npcs.Count > 0)
            {
                NPC nearest = m_npcs.First();
                foreach (var npc in m_npcs)
                {
                    if ((nearest.transform.position - transform.position).magnitude >
                        (npc.transform.position - transform.position).magnitude)
                    {
                        nearest = npc;
                    }
                }
                return nearest;
            }
            return null;
            
        }
    }

    void Start()
    {
        m_npcs = new List<NPC>();
    }

    void Update()
    {
        for (int i = m_npcs.Count - 1; i >= 0; --i)
        {
            if(m_npcs[i] == null)
                m_npcs.RemoveAt(i);
        }
    }
    public bool FindNPC()
    {
        return m_npcs.Count > 0;
    }
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("NPC"))
            return;
        
        NPC npc;
        if (_other.gameObject.TryGetComponent(out npc))
        {
            m_npcs.Add(npc);
        }
        
    }
    void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("NPC"))
            return;
        
        NPC npc;
        if (_other.gameObject.TryGetComponent(out npc))
        {
            m_npcs.Remove(npc);
        }
        
    }
}
