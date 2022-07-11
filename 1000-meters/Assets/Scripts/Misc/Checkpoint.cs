using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager m_GameManager;
    private CameraController m_CameraContr;
    public Health healthBar;
    public ExhaustUp exhaustUpScr;
    [SerializeField] private ParticleSystem checkpointParticle;
    [SerializeField] private PointSystem pointSyst;
    [SerializeField] private Collider2D circCollider; 

    private float cameraXpos = -2.22f;

    private void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        m_CameraContr = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!m_CameraContr.playerIsDead)
            {
                //circCollider.enabled = false;
                m_GameManager.lastCheckPointPos = transform.position;
                m_GameManager.lastCheckPointPosCam = new Vector3(cameraXpos, transform.position.y, -10);

                m_GameManager.checkpointTotal = pointSyst.totalPoints;
                //healthBar.currentMaxHealth++;
                //healthBar.health++;
                //exhaustUpScr.Pickup(collision);
                
            }  
        }
    }
}