using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHazard : MonoBehaviour
{
    public PlayerController playerContr;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDeath(other);
        }
    }

    void PlayerDeath(Collider2D player)
    {
        //Destroy(player.gameObject);
        playerContr.ResetPlayer();
    }
}
