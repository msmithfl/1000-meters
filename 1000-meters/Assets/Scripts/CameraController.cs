using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float fallDeathDelay = 2.0f;

    [SerializeField] private GameObject playerObject;

    [SerializeField] private Collider2D fallCollider;

    public bool playerIsDead = false;

    [SerializeField] private ParticleSystem deathParticles;

    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClip playerDeathSound;

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
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * movementSpeed;
        }

        if (playerObject.transform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, playerObject.transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsDead = true;
            deathParticles.transform.position = new Vector3(m_PlayerContr.transform.position.x, deathParticles.transform.position.y, deathParticles.transform.position.z);
            deathParticles.Play();
            audioSrc.PlayOneShot(playerDeathSound);
            fallCollider.enabled = false;    
            StartCoroutine(ResetToLastCheckpoint());
        }
    }

    IEnumerator ResetToLastCheckpoint()
    {
        yield return new WaitForSeconds(fallDeathDelay);
        m_PlayerContr.ResetPlayer();
        transform.position = m_GameManager.lastCheckPointPosCam;
        m_PlayerContr.playerHasJumped = false;
        m_PlayerContr.playerHasClimbed = false;
        fallCollider.enabled = true;
        playerIsDead = false;
        m_PlayerAnimator.SetBool("EntryIdleState", true);
    }
}