using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    private Character m_character;
    [SerializeField] private float m_werewolfDuration = 5.0f;
    [SerializeField] private float m_shadowRecovery = 0.5f;
    [Header("Debug")]
    [SerializeField] private float m_life = 1.0f;
    [SerializeField] private float m_werewolf = 0.0f;
    [SerializeField] private float m_damage = 0.0f;
    
    private List<MoonLight> m_moonLights;

    public float life => m_life;
    public float werewolf => m_werewolf / m_werewolfDuration;

    void Start()
    {
        m_character = GetComponent<Character>();
        m_moonLights = new List<MoonLight>();   
    }
    
    void Update()
    {
        if (m_life > 0.0f)
        {
            if (m_damage <= 0.0f)
                m_life = Mathf.Clamp(m_life + m_shadowRecovery * Time.deltaTime, 0.0f, 1.0f);
            else
            {
                m_life -= m_damage * Time.deltaTime;
                if (m_life <= 0)
                {
                    m_character.ChangeToWerewolf();
                    ResetWerewolf();
                }
            }
        }
        else if(m_damage <= 0.0f)
        {
            m_werewolf = Mathf.Clamp(m_werewolf - Time.deltaTime, 0.0f, m_werewolfDuration);
            if (m_werewolf <= 0.0f)
            {
                m_character.ExitWerewolf();
                ResetLife();
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;
        MoonLight moonLight;
        if (_other.gameObject.TryGetComponent(out moonLight))
        {
            m_moonLights.Add(moonLight);
            m_damage = Mathf.Max(moonLight.damage, m_damage);
            
        }
        
    }
    void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;
        MoonLight moonLight;
        if (_other.gameObject.TryGetComponent(out moonLight))
        {
            m_moonLights.Remove(moonLight);
            m_damage = GetMaxDamage();
        }
        
    }
    private float GetMaxDamage()
    {
        float damage = 0.0f;
        foreach (var moonLight in m_moonLights)
        {
            damage = Mathf.Max(moonLight.damage, damage);
        }

        return damage;
    }

    public void ResetLife()
    {
        m_life = 1.0f;
        m_werewolf = 0.0f;
    } 
    public void ResetWerewolf()
    {
        m_werewolf = m_werewolfDuration;
        m_life = 0.0f;
    } 
    
}
