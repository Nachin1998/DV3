﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Player player;
    PlayerMovement pm;
    TopDownMovement tdm;

    [Space]

    public Image healthBar;
    public Image bloodScreen;
    public Image sprintBar;

    void Start()
    {
        if(player.GetComponent<PlayerMovement>() != null)
        {
            pm = player.GetComponent<PlayerMovement>();
        }
        else
        {
            tdm = player.GetComponent<TopDownMovement>();
        }

        healthBar.fillAmount = player.currentHealth / 100;
        bloodScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player.isDead)
        {
            healthBar.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
            bloodScreen.gameObject.SetActive(true);
            return;
        }

        healthBar.fillAmount = player.currentHealth / 100;
        if (pm)
        {
            sprintBar.fillAmount = pm.currentSprint / 100;
            if (pm.canSprint)
            {
                sprintBar.color = Color.white;
            }
            else
            {
                sprintBar.color = Color.red;
            }
        }
        else if(tdm)
        {
            sprintBar.fillAmount = tdm.currentSprint / 100;
            if (tdm.canSprint)
            {
                sprintBar.color = Color.white;
            }
            else
            {
                sprintBar.color = Color.red;
            }
        }
        
        AkSoundEngine.SetRTPCValue("HEALTH", player.currentHealth);

        if (player.tookDamage)
        {
            StartCoroutine(ScreenDamage(1));
        }
    }

    public IEnumerator ScreenDamage(float bloodDuration)
    {
        bloodScreen.gameObject.SetActive(true);
        bloodScreen.CrossFadeAlpha(0, bloodDuration, false);
        yield return new WaitForSeconds(bloodDuration);
        bloodScreen.gameObject.SetActive(false);
    }
}
