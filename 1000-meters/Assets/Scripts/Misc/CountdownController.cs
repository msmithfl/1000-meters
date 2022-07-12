using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public float currentComboTime = 1;
    public float timerSpeed;
    public bool comboIsActive = false;
    public TextMeshProUGUI comboCounterText;
    public Image comboTimerImage;

    [SerializeField] private GameObject m_ComboDial;
    [SerializeField] private PlayerController m_PlayerContr;

    //note: combo timer is faster when player is grounded,
    //      due to duplicate code (comboTime -= time) in here and player script

    void Start()
    {
        m_ComboDial.SetActive(false);
    }

    void Update()
    {
        DisplayComboTimer();

        if (currentComboTime <= 1 && currentComboTime >= 0)
        {
            comboIsActive = true;
        }
        else
        {
            comboIsActive = false;
        }

        if (!comboIsActive)
        {
            m_PlayerContr.climbCombo = 0;
        }

        comboTimerImage.fillAmount = currentComboTime;
        comboCounterText.text = m_PlayerContr.climbCombo.ToString();

        if (m_PlayerContr.climbCombo >= 2)
        {
            currentComboTime -= Time.deltaTime * timerSpeed;
        }
    }

    private void DisplayComboTimer()
    {
        if (m_PlayerContr.climbCombo >= 2)
        {
            m_ComboDial.SetActive(true);
        }
        else
        {
            m_ComboDial.SetActive(false);
        }
    }
}