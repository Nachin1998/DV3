using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100f;
    
    [HideInInspector] public bool isDead = false;

    public Light flashlight;
    public bool tookDamage = false;

    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.gameObject.SetActive(!flashlight.gameObject.activeSelf);
        }

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
        AkSoundEngine.SetRTPCValue("player_health", currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(TookDamage(0.5f));
        if(currentHealth <= 0)
        {
            AkSoundEngine.PostEvent("player_dead", gameObject);
            AkSoundEngine.PostEvent("game_over", gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("player_hurt", gameObject);
        }
    }

    public IEnumerator TookDamage(float bloodDuration)
    {
        tookDamage = true;
        yield return new WaitForSeconds(bloodDuration);
        tookDamage = false;
    }
}
