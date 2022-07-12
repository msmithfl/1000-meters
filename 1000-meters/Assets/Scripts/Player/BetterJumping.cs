using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    [SerializeField] private float m_FallMultiplier = 2.5f;
    [SerializeField] private float m_LowJumpMultiplier = 2f;
    private Rigidbody2D m_Rb;

    public PlayerController playerContr;

    void Awake()
    {
        m_Rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (m_Rb.velocity.y < 0)
        {
            m_Rb.gravityScale = m_FallMultiplier;
        }
        else if (m_Rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            m_Rb.gravityScale = m_LowJumpMultiplier;
        }
        else if (Input.GetButton("Jump") && playerContr.m_IsTouchingWall|| Input.GetButton("Jump") && !playerContr.pickCollider.enabled)
        {
            m_Rb.gravityScale = m_LowJumpMultiplier;
        }
        else
        {
            m_Rb.gravityScale = 1f;
        }   
    }
}