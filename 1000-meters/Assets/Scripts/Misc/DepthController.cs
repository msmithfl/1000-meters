using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepthController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI depthText;
    private PlayerController m_playerController;

    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float depthInMeters = 999 - m_playerController.transform.position.y * 4;

        depthText.text = depthInMeters.ToString("F0") + "m";
    }
}
