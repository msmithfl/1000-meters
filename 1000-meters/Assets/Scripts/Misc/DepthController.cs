using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepthController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_DepthText;
    private PlayerController m_PlayerController;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float depthInMeters = 999 - m_PlayerController.transform.position.y * 4;

        m_DepthText.text = depthInMeters.ToString("F0") + "m";
    }
}
