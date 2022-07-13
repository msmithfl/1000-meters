using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_FallDeathDelay = 2.0f;

    [SerializeField] private GameObject m_PlayerObject;

    [SerializeField] private Collider2D m_FallCollider;

    public bool playerIsDead = false;

    [SerializeField] private ParticleSystem m_DeathParticles;

    [SerializeField] private AudioSource m_AudioSrc;
    [SerializeField] private AudioClip m_PlayerDeathSound;

    private PlayerController m_PlayerContr;
    private GameManager m_GameManager;
    private Animator m_PlayerAnimator;

    void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        m_PlayerContr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_PlayerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        transform.position = m_GameManager.lastCheckPointPosCam;

    }

    void Update()
    {
        if (m_PlayerContr.CameraMoveOn)
        {
            CameraMove();
        }
    }

    private void CameraMove()
    {
        if (m_PlayerContr.playerHasJumped || m_PlayerContr.playerHasClimbed)
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * m_MovementSpeed;
        }

        if (m_PlayerObject.transform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, m_PlayerObject.transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsDead = true;
            m_DeathParticles.transform.position = new Vector3(m_PlayerContr.transform.position.x, m_DeathParticles.transform.position.y, m_DeathParticles.transform.position.z);
            m_DeathParticles.Play();
            m_AudioSrc.PlayOneShot(m_PlayerDeathSound);
            m_FallCollider.enabled = false; 
            StartCoroutine(ResetToLastCheckpoint());
        }
    }

    IEnumerator ResetToLastCheckpoint()
    {
        yield return new WaitForSeconds(m_FallDeathDelay);
        m_PlayerContr.ResetPlayer();
        transform.position = m_GameManager.lastCheckPointPosCam;
        m_PlayerContr.playerHasJumped = false;
        m_PlayerContr.playerHasClimbed = false;
        m_FallCollider.enabled = true;
        playerIsDead = false;
        m_PlayerAnimator.SetBool("EntryIdleState", true);
    }
}