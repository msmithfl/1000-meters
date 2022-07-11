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

    [SerializeField] private GameObject comboDial;
    [SerializeField] private PlayerController playerContr;

    //note: combo timer is faster when player is grounded,
    //      due to duplicate code (comboTime -= time) in here and player script

    void Start()
    {
        comboDial.SetActive(false);
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
            playerContr.climbCombo = 0;
        }

        comboTimerImage.fillAmount = currentComboTime;
        comboCounterText.text = playerContr.climbCombo.ToString();

        if (playerContr.climbCombo >= 2)
        {
            currentComboTime -= Time.deltaTime * timerSpeed;
        }
    }

    private void DisplayComboTimer()
    {
        if (playerContr.climbCombo >= 2)
        {
            comboDial.SetActive(true);
        }
        else
        {
            comboDial.SetActive(false);
        }
    }
}