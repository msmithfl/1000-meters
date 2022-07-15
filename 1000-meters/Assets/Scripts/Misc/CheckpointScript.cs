using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private Collider2D m_BoxCollider;
    [SerializeField] private ParticleSystem m_CheckpointParticle;

    [SerializeField] private AudioSource m_AudSource;
    [SerializeField] private AudioClip m_ClapSound;

    private float m_CameraXpos = -2.22f;

    private GameManager m_GameManager;
    private PointSystem m_PointSystem;

    private void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        m_PointSystem = GameObject.FindGameObjectWithTag("PointSystem").GetComponent<PointSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_BoxCollider.enabled = false;
            m_GameManager.checkpointTotal = m_PointSystem.totalPoints;
            m_GameManager.lastCheckPointPos = transform.position;
            m_GameManager.lastCheckPointPosCam = new Vector3(m_CameraXpos, transform.position.y, -10);
            m_CheckpointParticle.Play();
            m_AudSource.PlayOneShot(m_ClapSound);
        }
    }
}