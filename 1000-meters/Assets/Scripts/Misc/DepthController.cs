using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepthController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_DepthText;
    private PlayerController m_PlayerController;

    void Start()
    {
        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        float depthInMeters = 999 - m_PlayerController.transform.position.y * 4;

        m_DepthText.text = depthInMeters.ToString("F0") + "m";
    }
}
