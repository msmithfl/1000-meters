using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("For Movement")]
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_AirSpeed;
    private float m_HorizontalInput;  
    private bool m_IsFacingRight = true;

    [Header("For Jumping")]
    [SerializeField] private float m_JumpForce;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private Transform m_GroundCheckPoint;
    [SerializeField] private Vector2 m_GroundCheckSize;
    private bool m_CanJump;
    private bool m_IsTouchingGround;

    [Header("For Climbing")]
    [SerializeField] private float m_ClimbSpeed;
    [SerializeField] private float m_AttackRate;
    private float m_NextAttackTime = 0f;
    [SerializeField] private LayerMask m_WallLayer;
    [SerializeField] private Transform m_WallCheckPoint;
    [SerializeField] private Vector2 m_WallCheckSize;   
    public Collider2D pickCollider;
    [SerializeField] private float m_PickCollierTime;
    private bool m_CanClimb;
    public bool m_IsTouchingWall;

    [Header("For UI")]
    //public Health healthBar;
    //[SerializeField] private int m_Coins = 0;

    [Header("For Camera")]
    public CameraController camContr;
    public bool CameraMoveOn = true;
    public bool playerHasJumped = false;
    public bool playerHasClimbed = false;

    [Header("For Pick Particles")]
    [SerializeField] private GameObject m_PickParticles;
    [SerializeField] private GameObject m_PickParticleSpawnPoint;
    private Vector3 m_PickPoint;
    private int m_YAxisParticleRot = 1;

    [Header("For Audio")]
    [SerializeField] private AudioClip m_ClimbSound;

    private Vector2 m_StartingPos;

    private Rigidbody2D m_Rb;
    private Animator m_Animator;
    private GameManager m_GameManager;
    private CameraShake m_Shake;
    private PointSystem m_PointSys;
    private CountdownController m_CountdownContr;
    private AudioSource m_AudioSource;

    public int climbCombo = 0;

    void Awake()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        m_StartingPos = m_GameManager.lastCheckPointPos;
        transform.position = m_StartingPos;

        m_Shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<CameraShake>();
        m_PointSys = GameObject.FindGameObjectWithTag("PointSystem").GetComponent<PointSystem>();
        m_CountdownContr = GameObject.FindGameObjectWithTag("GM").GetComponent<CountdownController>();
    }

    void Update()
    {
        if (!camContr.playerIsDead)
        {
            Inputs();
        }

        WorldChecks();
        ResetPlayerKey();

        //speeds up combo timer when grounded
        if (m_IsTouchingGround)
        {
            if (m_CountdownContr.currentComboTime > 0)
            {
                m_CountdownContr.currentComboTime -= Time.deltaTime * m_CountdownContr.timerSpeed;
            } 
        }

        if (m_IsTouchingGround && climbCombo == 1)
        {
            climbCombo = 0;
        }
    }

    private void FixedUpdate()
    {
        Movement();
        Climb();
        //Jump();
    }

    private void LateUpdate()
    {
        Animation();
    }

    private void Inputs()
    {
        //movement
        m_HorizontalInput = Input.GetAxis("Horizontal");

        //jumping
        if (Input.GetButtonDown("Jump") && m_IsTouchingGround)
        {
            m_CanJump = true;
        }

        //climbing
        if (Time.time >= m_NextAttackTime)
        {
            if (Input.GetButtonDown("Fire1") && m_IsTouchingWall || Input.GetButtonDown("Jump") && m_IsTouchingWall)
            {
                m_CanClimb = true;
                m_NextAttackTime = Time.time + 1f / m_AttackRate;
            }
        }
    }

    private void WorldChecks()
    {
        m_IsTouchingGround = Physics2D.OverlapBox(m_GroundCheckPoint.position, m_GroundCheckSize, 0, m_GroundLayer);
        m_IsTouchingWall = Physics2D.OverlapBox(m_WallCheckPoint.position, m_WallCheckSize, 0, m_WallLayer);
    }

    private void Movement()
    {
        //celeste style movement, constant speed, consistent animation, airSpeed variable
        //animator is there for front facing idle state upon respawn
        if (m_HorizontalInput > 0.3f)
        {
            m_Animator.SetBool("EntryIdleState", false);

            if (m_IsTouchingGround)
            {
                m_Rb.velocity = new Vector2(m_Speed, m_Rb.velocity.y);
            }
            else
            {
                m_Rb.velocity = new Vector2(m_AirSpeed, m_Rb.velocity.y);
            }           
        }
        else if (m_HorizontalInput < -0.3f)
        {
            m_Animator.SetBool("EntryIdleState", false);

            if (m_IsTouchingGround)
            {
                m_Rb.velocity = new Vector2(-m_Speed, m_Rb.velocity.y);
            }
            else
            {
                m_Rb.velocity = new Vector2(-m_AirSpeed, m_Rb.velocity.y);
            }           
        }
        else
        {
            m_Rb.velocity = new Vector2(0, m_Rb.velocity.y);
        }

        if (m_HorizontalInput < -0.3f && m_IsFacingRight)
        {
            SpriteFlip();
            m_YAxisParticleRot = 180;
        }
        else if (m_HorizontalInput > 0.3f && !m_IsFacingRight)
        {
            SpriteFlip();
            m_YAxisParticleRot = 0;
        }
    }

    private void SpriteFlip()
    {
        if (!m_IsTouchingWall)
        {
            m_IsFacingRight = !m_IsFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void Climb()
    {        
        if (m_CanClimb)
        {           
            m_Rb.velocity = new Vector2(m_Rb.velocity.x, m_ClimbSpeed);

            transform.Find("Ground Check").gameObject.SetActive(false);
            pickCollider.enabled = false;
            StartCoroutine(enableColliders());

            playerHasClimbed = true;

            m_CanClimb = false;

            m_PickPoint = m_PickParticleSpawnPoint.transform.position;

            m_Shake.CamShake();
            playPickParticles();
            m_AudioSource.PlayOneShot(m_ClimbSound);

            climbCombo++;
            m_PointSys.UpdatePoints();

            m_CountdownContr.currentComboTime = 1;
        }
    }

    private void playPickParticles()
    {
        Instantiate(m_PickParticles, m_PickPoint, Quaternion.Euler(0, m_YAxisParticleRot, 0));
    }

    IEnumerator enableColliders()
    {
        yield return new WaitForSeconds(m_PickCollierTime);
        pickCollider.enabled = true;
        transform.Find("Ground Check").gameObject.SetActive(true);
    }

    private void Animation()
    {
        m_Animator.SetFloat("Speed", Mathf.Abs(m_Rb.velocity.x));
        m_Animator.SetBool("OnGround", m_IsTouchingGround);

        if (m_CanClimb)
        {
            m_Animator.SetTrigger("Climb");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(m_GroundCheckPoint.position, m_GroundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(m_WallCheckPoint.position, m_WallCheckSize);
        
    }

    private void ResetPlayerKey()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.position = m_GameManager.lastCheckPointPos;

            m_Rb.velocity = Vector2.zero;
        }
    }

    public void ResetPlayer()
    {
            gameObject.transform.position = m_GameManager.lastCheckPointPos;
            m_PointSys.totalPoints = m_GameManager.checkpointTotal;
            m_PointSys.totalPointText.text = m_GameManager.checkpointTotal.ToString();
            m_Rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            string itemType = collision.gameObject.GetComponent<CoinScript>().itemType;

            //print("Collected a " + itemType);

            m_PointSys.CoinCollected();

            if (itemType == "coin")
            {
                //m_Coins++;
            }

            Destroy(collision.gameObject);
        }
    }

    private void Jump()
    {
        if (m_CanJump && m_IsTouchingGround)
        {
            m_Rb.velocity = new Vector2(m_Rb.velocity.x, m_JumpForce);
            m_CanJump = false;
            playerHasJumped = true;
        }
    }
}