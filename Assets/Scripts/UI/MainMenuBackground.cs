using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackground : MonoBehaviour
{
    Image background;
    public Sprite normalBackground;
    public Sprite cursedBackground;

    public float minWaitTime;
    public float maxWaitTime;
    public float flashDuration;

    float timer = 0;
    void Start()
    {
        background = GetComponent<Image>();
        AkSoundEngine.PostEvent("player_bathit", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= Random.Range(minWaitTime, maxWaitTime))
        {
            StartCoroutine(FlashBackground(flashDuration));
            timer = 0;
        }
    }

    IEnumerator FlashBackground(float flashDuration)
    {
        background.sprite = cursedBackground;

        yield return new WaitForSeconds(flashDuration);

        background.sprite = normalBackground;
    }
}
