using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int currentMaxHealth;
    public int totalHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("For Pick Regen")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize;
    private bool m_IsTouchingGround;

    public CameraController camContr;
    public PlayerController plyrContr;
    [SerializeField] private float uiFadeOffset = 2f;

    private void Update()
    {
        m_IsTouchingGround = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);

        if (m_IsTouchingGround == true && health <= currentMaxHealth && !camContr.playerIsDead)
        {
            health += 1;
        }

        if (health > currentMaxHealth)
        {
            health = currentMaxHealth;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < totalHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        if (plyrContr.transform.position.y < camContr.transform.position.y - uiFadeOffset)
        {
            FadeOutUI();
        }
        else
        {
            FadeInUI();
        }
    }

    public void FadeOutUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            var tempColor = hearts[i].color;
            tempColor.a = 0.5f;
            hearts[i].color = tempColor;
        }
    }

    public void FadeInUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            var tempColor = hearts[i].color;
            tempColor.a = 1f;
            hearts[i].color = tempColor;
        }
    }
}