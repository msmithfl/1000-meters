using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    public int totalPoints = 0;
    private int m_ClimbPointAmnt = 100;

    public TextMeshProUGUI totalPointText;
    
    [SerializeField] private AudioSource m_AudSource;
    [SerializeField] private AudioClip m_CoinSound;

    public PlayerController playerCntr;

    private void Start()
    {
        totalPointText.text = totalPoints.ToString();
    }

    public void UpdatePoints()
    {
        if (playerCntr.climbCombo > 1 && playerCntr.climbCombo <= 5)
        {
            totalPoints += m_ClimbPointAmnt * 1;
        }
        else if (playerCntr.climbCombo > 5 && playerCntr.climbCombo <= 10)
        {
            totalPoints += m_ClimbPointAmnt * 2;
        }
        else if (playerCntr.climbCombo > 10 && playerCntr.climbCombo <= 20)
        {
            totalPoints += m_ClimbPointAmnt * 4;
        }
        else if (playerCntr.climbCombo > 20 && playerCntr.climbCombo <= 50)
        {
            totalPoints += m_ClimbPointAmnt * 6;
        }
        else if (playerCntr.climbCombo > 50 && playerCntr.climbCombo <= 100)
        {
            totalPoints += m_ClimbPointAmnt * 8;
        }
        else if (playerCntr.climbCombo > 100 && playerCntr.climbCombo <= 200)
        {
            totalPoints += m_ClimbPointAmnt * 10;
        }

        totalPointText.text = totalPoints.ToString();
    }

    public void CoinCollected()
    {
        m_AudSource.PlayOneShot(m_CoinSound);
        totalPoints += 50;
        totalPointText.text = totalPoints.ToString();
    }
}
