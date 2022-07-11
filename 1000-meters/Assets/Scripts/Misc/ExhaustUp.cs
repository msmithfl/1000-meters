using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustUp : MonoBehaviour
{
    private GameManager m_GameManager;
    private PointSystem m_PointSystem;
    public Health healthBar;
    [SerializeField] private Collider2D boxCollider;
    [SerializeField] private ParticleSystem checkpointParticle;

    [SerializeField] private AudioSource audSource;
    [SerializeField] private AudioClip clapSound;

    private float cameraXpos = -2.22f;

    private void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        m_PointSystem = GameObject.FindGameObjectWithTag("PointSystem").GetComponent<PointSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boxCollider.enabled = false;
            healthBar.currentMaxHealth++;
            healthBar.health++;
            m_GameManager.checkpointTotal = m_PointSystem.totalPoints;
            m_GameManager.lastCheckPointPos = transform.position;
            m_GameManager.lastCheckPointPosCam = new Vector3(cameraXpos, transform.position.y, -10);
            checkpointParticle.Play();
            audSource.PlayOneShot(clapSound);
        }
    }
}