using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuFlashController : MonoBehaviour
{
    [Header("Main menu")]
    public Image menuBackground;
    public Sprite normalMenuBackground;
    public Sprite cursedMenuBackground;

    [Header("Credits")]
    public Image creditsBackground;
    public Image creditsText;
    public Sprite normalCreditsBackground;
    public Sprite cursedCreditsBackground;
    public Sprite normalCreditsText;
    public Sprite cursedCreditsText;

    [Space]

    public float minWaitTime;
    public float maxWaitTime;
    public float flashDuration;

    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= Random.Range(minWaitTime, maxWaitTime))
        {
            StartCoroutine(FlashScreen(flashDuration));
            timer = 0;
        }
    }

    IEnumerator FlashScreen(float flashDuration)
    {
        menuBackground.sprite = cursedMenuBackground;
        creditsBackground.sprite = cursedCreditsBackground;
        creditsText.sprite = cursedCreditsText;

        yield return new WaitForSeconds(flashDuration);

        menuBackground.sprite = normalMenuBackground;
        creditsBackground.sprite = normalCreditsBackground;
        creditsText.sprite = normalCreditsText;
    }
}
