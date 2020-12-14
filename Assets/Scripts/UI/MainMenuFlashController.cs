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
    public Button normalPlayButton;
    public Button newCursedPlayButton;
    public ParticleSystem stars;

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

    void Start()
    {
        stars.gameObject.SetActive(true);
        if (normalPlayButton && newCursedPlayButton)
        {
            normalPlayButton.gameObject.SetActive(true);
            newCursedPlayButton.gameObject.SetActive(false);
        }
    }

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
        stars.gameObject.SetActive(false);

        if (normalPlayButton && newCursedPlayButton)
        {
            normalPlayButton.gameObject.SetActive(false);
            newCursedPlayButton.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(flashDuration);

        menuBackground.sprite = normalMenuBackground;
        creditsBackground.sprite = normalCreditsBackground;
        creditsText.sprite = normalCreditsText;
        stars.gameObject.SetActive(true);

        if (normalPlayButton && newCursedPlayButton)
        {
            normalPlayButton.gameObject.SetActive(true);
            newCursedPlayButton.gameObject.SetActive(false);
        }
    }
}
