using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    public int totalPoints = 0;
    private int climbPointAmnt = 100;

    public TextMeshProUGUI totalPointText;
    
    [SerializeField] private AudioSource audSource;
    [SerializeField] private AudioClip coinSound;

    public PlayerController playerCntr;

    private void Start()
    {
        totalPointText.text = totalPoints.ToString();
    }

    public void UpdatePoints()
    {
        if (playerCntr.climbCombo > 1 && playerCntr.climbCombo <= 5)
        {
            totalPoints += climbPointAmnt * 1;
        }
        else if (playerCntr.climbCombo > 5 && playerCntr.climbCombo <= 10)
        {
            totalPoints += climbPointAmnt * 2;
        }
        else if (playerCntr.climbCombo > 10 && playerCntr.climbCombo <= 20)
        {
            totalPoints += climbPointAmnt * 4;
        }
        else if (playerCntr.climbCombo > 20 && playerCntr.climbCombo <= 50)
        {
            totalPoints += climbPointAmnt * 6;
        }
        else if (playerCntr.climbCombo > 50 && playerCntr.climbCombo <= 100)
        {
            totalPoints += climbPointAmnt * 8;
        }
        else if (playerCntr.climbCombo > 100 && playerCntr.climbCombo <= 200)
        {
            totalPoints += climbPointAmnt * 10;
        }

        totalPointText.text = totalPoints.ToString();
    }

    public void CoinCollected()
    {
        audSource.PlayOneShot(coinSound);
        totalPoints += 50;
        totalPointText.text = totalPoints.ToString();
    }
}
