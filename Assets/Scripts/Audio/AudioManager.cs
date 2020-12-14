using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;

    // Update is called once per frame
    void Update()
    {
        AkSoundEngine.SetRTPCValue("SFX_VOLUME", sfxSlider.value);
        AkSoundEngine.SetRTPCValue("MUSIC_VOLUME", musicSlider.value);
    }
}
