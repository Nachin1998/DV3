using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100f;
    public Image healthBar;
    
    [HideInInspector] public bool isDead = false;

    public Light flashlight;
    public Image bloodScreen;

    void Start()
    {
        healthBar.fillAmount = currentHealth / 100;
        bloodScreen.gameObject.SetActive(false);

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        healthBar.fillAmount = currentHealth / 100;

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.gameObject.SetActive(!flashlight.gameObject.activeSelf);
        }

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(ScreenDamage(1));
    }

    public IEnumerator ScreenDamage(float bloodDuration)
    {
        bloodScreen.gameObject.SetActive(true);
        bloodScreen.CrossFadeAlpha(0, bloodDuration, false);
        yield return new WaitForSeconds(bloodDuration);
        bloodScreen.gameObject.SetActive(false);
    }
}
